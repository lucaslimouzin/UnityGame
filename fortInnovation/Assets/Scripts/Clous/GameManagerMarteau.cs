using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
//pour récupérer en réseau le fichier Json
using UnityEngine.Networking;
public class GameManagerMarteau : MonoBehaviour
{
    public GameObject clou; 
    public GameObject marteauPlayer;
    public GameObject marteauMj;
    public GameObject panelInstruction;
    public GameObject panelInfoMJ;
    public GameObject panelJauge;
    public GameObject buttonTextMarteau;
    public TextMeshProUGUI MJText;
    public TextMeshProUGUI textVieClou;
    public Button buttonJauge;
    public Transform pivotPointMarteauPlayer;
    public Transform pivotPointMarteauMj;
    float rotationSpeed = 60;
    Vector3 currentEulerAnglesPlayer;
    Vector3 currentEulerAnglesMj;
    float z;
    public float playerForce;
    public Slider forceMarteau;
    private int maxForce = 100;
    private bool up =false;
    private bool tourJoueur = false;
    private bool aRelacher = false;
    private float vieDuClou;
    private bool phaseQuestion = false;
    private bool ignoreInput = false;
    private bool finDuJeu = false;
    public bool isMoving = false;

   //variables pour les questions//////////////////////
   // URL du fichier JSON sur le réseau
    private string jsonURL = "https://givrosgaming.fr/fortInnov/BatonQuestions.json";
    public GameObject panelQuestions;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI propositionAtext;
    public TextMeshProUGUI propositionBtext;
    public TextMeshProUGUI propositionCtext;
    public Button buttonA;
    public Button buttonB;
    public Button buttonC;
    private bool aJuste = false;
    private int numQuestions = 0;

    private bool isSpaceEnabled = false;
    

    [System.Serializable]
    public class QuestionData
    {
        public int idQuestion;
        public string question;
        public string[] propositions;
        public string reponseCorrecte;
    }

    [System.Serializable]
    public class Questions
    {
        public QuestionData[] questions;
    }

    private Questions listQuestions; 

    //fin variables pour les questions ////////////////////

    //--------pour mettre à jour le score --------------------------------------
    private void OnEnable()
    {
        // S'abonner à l'événement OnScoreUpdated
        MainGameManager.OnScoreUpdated += HandleScoreUpdated;
    }

    private void OnDisable()
    {
        // Se désabonner de l'événement OnScoreUpdated lors de la désactivation du script
         MainGameManager.OnScoreUpdated -= HandleScoreUpdated;
    }

    // Méthode appelée lorsque le score est mis à jour
    private void HandleScoreUpdated(int scoreBaton, int scoreClou, int scoreBassin, int scoreEnigmes)
    {
        // Faire quelque chose avec le nouveau score
        MainGameManager.Instance.scoreReco = scoreBaton + scoreClou + scoreBassin + scoreEnigmes; 
       
    }
    //-------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //charge la coroutine qui va récupérer le fichier Json 
        //StartCoroutine(LoadJsonFromNetwork()); //a activer lors du déploiment
        //charge la coroutine qui va récupérer le fichier Json 
        StartCoroutine(LoadJsonFromLocal());

        forceMarteau.value = 0;
        playerForce = 10;
        vieDuClou = 500f;
        //affichage de la vie du clou
        textVieClou.text = vieDuClou.ToString();
        isSpaceEnabled = false;
        phaseQuestion = true;
        //Debug.Log("PhaseQ 1 = " + phaseQuestion);
       // Debug.Log("Espace 1 = " + isSpaceEnabled);
        ResetGauge();
      //on affiche le panneau des régles
        PanneauRegle();
    }

    //fonction qui charge les questions depuis local
    IEnumerator LoadJsonFromLocal(){
        // Charger le fichier JSON (assurez-vous de placer le fichier dans le dossier Resources)
        TextAsset jsonFile = Resources.Load<TextAsset>("BatonQuestions");
        // Désérialiser les données JSON
        listQuestions = JsonUtility.FromJson<Questions>(jsonFile.ToString());
        yield return null;
    }
    //fonction qui charge les questions depuis le réseau
    IEnumerator LoadJsonFromNetwork()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(jsonURL))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Erreur lors du chargement du fichier JSON depuis le réseau: " + www.error);
            }
            else
            {
                // Les données JSON ont été téléchargées avec succès
                string jsonText = www.downloadHandler.text;

                // Désérialiser les données JSON
                listQuestions = JsonUtility.FromJson<Questions>(jsonText);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ignoreInput)
        {
            return;
        }
         if (!phaseQuestion){
                // Vérifier si c'est le tour du joueur et si le jeu n'est pas fini
            if (tourJoueur && !finDuJeu)
            {
                if (isSpaceEnabled)
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        GestionAppuiToucheEspace();
                    }

                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        Input.ResetInputAxes(); // reset toutes les entrées utilisateur
                        ignoreInput = true; // ignore les inputs 
                        phaseQuestion = true;
                        isSpaceEnabled = false;
                        StartCoroutine(Wait());
                    }
                }
            }
         }       
    }

    private void GestionAppuiToucheEspace()
    {
        // Logique pour augmenter ou diminuer la force
        if (aJuste) {
            maxForce = 100;
        } 
        else {
            maxForce = 50;
        }

        if (playerForce == maxForce) {
            up = false; 
        } 
        else if (playerForce == 10) {
            up = true;
        }
        
        if (up){
            playerForce += 1;
        }
        else {
            playerForce -= 1;
        }
        Slider();
    }
   
    public void Slider() {
        forceMarteau.value = playerForce;
    }

    public void ResetGauge(){
        playerForce = 10;
        forceMarteau.value = 0;
    }
    IEnumerator Wait(){
        yield return new WaitForSeconds(0f);
        Input.ResetInputAxes(); // reset toutes les entrées utilisateur
        ignoreInput = true; // ignore les inputs 
        aRelacher = true;
        phaseQuestion = true;
        isSpaceEnabled = false; //on desactive la touche espace
        //Debug.Log("Espace 2= " + isSpaceEnabled);
        TourDuJoueur();
        
    }


    //affichage du panneau des règles
    private void PanneauRegle (){
        panelInstruction.SetActive(true);
    }
    
    // retrait panneau des règles
    //affichage du panneau de la règle
    public void RetraitPanneauRegle (){
        panelInstruction.SetActive(false);
       if (MainGameManager.Instance.quiCommence == "Player"){
            //On affiche la question
            Invoke("AfficheLaQuestion",0f);
            //tour du player     
        }
        else {
            panelInfoMJ.SetActive(true);
            tourJoueur = false;
            MJText.text = "Maitre du jeu : Je commence à frapper !";
            TourDuMj();
        } 
    }

    //affichage de la question   
    private void AfficheLaQuestion(){
        Input.ResetInputAxes(); // reset toutes les entrées utilisateur
        ignoreInput = true; // ignore les inputs 
        phaseQuestion = true;
        //Debug.Log("PhaseQ 2 = " + phaseQuestion);
        tourJoueur = false;
        isSpaceEnabled = false;
        //Debug.Log("Espace 3 = " + isSpaceEnabled);
        ResetGauge();
        //choisi les questions de 1 à taille Json
        //comme on vise un tableau on est obligé de commencer à 0
        //et pour range on va jusqu'à taille Json
        do
        {
            numQuestions = UnityEngine.Random.Range(0, listQuestions.questions.Length);
        } while (MainGameManager.Instance.questionsClouPosees.Contains(numQuestions));

        MainGameManager.Instance.questionsClouPosees.Add(numQuestions);
        // Restreindre le nombre total de questions posées
        if (MainGameManager.Instance.questionsClouPosees.Count >= listQuestions.questions.Length)
        {
            // Si toutes les questions ont été posées, réinitalisation de la liste
            MainGameManager.Instance.questionsClouPosees.Clear();
        }

        //gestion des panneaux
        if (panelInstruction.activeSelf){
            panelInstruction.SetActive(false);
        }
        if (panelInfoMJ.activeSelf){
            panelInfoMJ.SetActive(false);
        }
        panelQuestions.SetActive(true);

        QuestionData question = listQuestions.questions[numQuestions];
        //affichage des données
        questionText.text = question.question;
        propositionAtext.text = question.propositions[0];
        propositionBtext.text = question.propositions[1];
        propositionCtext.text = question.propositions[2];

        if (Input.GetKey(KeyCode.Space)) {

        } else {
            buttonA.onClick.AddListener(() => OnButtonClick("A", question.reponseCorrecte));
            buttonB.onClick.AddListener(() => OnButtonClick("B", question.reponseCorrecte));
            buttonC.onClick.AddListener(() => OnButtonClick("C", question.reponseCorrecte));
        }
        
        
    }

    //fonction qui check le bouton enfoncé
    private void OnButtonClick(String choix, string reponseCorrecte){
        // Supprimer tous les écouteurs d'événements du bouton
        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.RemoveAllListeners();
        buttonC.onClick.RemoveAllListeners();

        if (choix == reponseCorrecte){
            aJuste = true;
        }
        else {
            aJuste = false;
        }
        //on enlève le panneau des questions
        RetraitPanneauQuestions(aJuste);
    }

    //retrait panneau Question
    private void RetraitPanneauQuestions(bool reponseJuste){
        Input.ResetInputAxes(); // reset toutes les entrées utilisateur
        ignoreInput = true; // ignore les inputs 
        panelQuestions.SetActive(false);
        panelInfoMJ.SetActive(true);
        if(reponseJuste){
            tourJoueur= true;
            phaseQuestion = false;
            ignoreInput = false; // autorise les inputs
            //Debug.Log("PhaseQ 3 = " + phaseQuestion);
            MJText.text = "Maitre du jeu : Bien répondu, votre marteau est chargé à 100%";
            isSpaceEnabled = true; //active la touche espace
            //Debug.Log("Espace 4 = " + isSpaceEnabled);
            TourDuJoueur();
        } 
        else {
            tourJoueur = false;
            isSpaceEnabled = false; // désactive la touche espace
          //  Debug.Log("Espace 5 = " + isSpaceEnabled);
            MJText.text = "Maitre du jeu : Ce n'est pas la bonne réponse, c'est à moi de jouer";
            TourDuMj();
        }
        
    }



    private void TourDuJoueur(){
        if(tourJoueur) {
            buttonTextMarteau.SetActive(true);
            ////debug.Log("Debut tour joueur");
            //MJText.text = "Maître du jeu : A vous de jouer";
            //ResetGauge();
            if(aRelacher){
                aRelacher = false;
                isSpaceEnabled = false;//on desactive la touche espace
                //Debug.Log("Espace 6 = " + isSpaceEnabled);
                Invoke("MoveMarteau",1f);
            }
        }  
    }

    private void TourDuMj(){
        if (!tourJoueur) {
            buttonTextMarteau.SetActive(false);
            ////debug.Log("Debut tour Mj");
            //MJText.text = "Maître du jeu : A mon tour de jouer";
            //ResetGauge();
            Invoke("MoveMarteau",1f);
        }        
    }

    private void MoveMarteau(){
        phaseQuestion = true;
        Input.ResetInputAxes(); // reset toutes les entrées utilisateur
        ignoreInput = true; // ignore les inputs 
        //Debug.Log("PhaseQ 4 = " + phaseQuestion);
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(MoveMarteauCoroutine());
        }
    }

    private void MoveClou (float currentHP){
        // Assurez-vous que votre objet clou existe
    if (clou != null)
    {   float minY = 0.926f;
        float maxY = 1.498f;
        float maxHP = 800f;
        // Calculez la position Y en utilisant une règle de trois
        float normalizedY = Mathf.Lerp(minY, maxY, currentHP / maxHP);

        // Récupérez la position actuelle du clou
        Vector3 newPosition = clou.transform.position;

        //si le clou dépasse la zone cible alors on le met au minY
        if (currentHP <= 0){
            normalizedY = minY;
        }
        // Modifiez la position Y avec la valeur souhaitée
        newPosition.y = normalizedY;

        // Appliquez la nouvelle position au clou
        clou.transform.position = newPosition;
        
        if (vieDuClou <=0) {
            vieDuClou = 0;
        }
        //affichage de la vie du clou
        textVieClou.text = vieDuClou.ToString();
    }
    else
    {
        ////debug.LogError("Clou n'est pas défini. Assurez-vous de le définir correctement.");
    }
    }

    private IEnumerator MoveMarteauCoroutine(){
        int mjForce;
        
            //si on a faux alors le mj tapera entre 50 et 101
            mjForce = UnityEngine.Random.Range(50,101);
            isSpaceEnabled = false; //on desactive la touche espace 
            //Debug.Log("Espace = 7 " + isSpaceEnabled);
            //faire tourner le marteau du player
            if (tourJoueur){
                //on attribue la valeur de la Force à Z
                while (currentEulerAnglesPlayer.z < 70f){
                    z = 0.05f * playerForce;
                    currentEulerAnglesPlayer += new Vector3(0,0,z) * Time.deltaTime * rotationSpeed;
                    pivotPointMarteauPlayer.localEulerAngles = currentEulerAnglesPlayer;
                    yield return null; //Attendre la prochaine trame
                }
                //on descend la vie du clou 
                vieDuClou -= playerForce; 
                MoveClou(vieDuClou);
            }
            //faire tourner le marteau du Mj
            else {
                while (currentEulerAnglesMj.z > -70f ){
                    z = -0.05f * mjForce;
                    currentEulerAnglesMj += new Vector3(0,0,z) * Time.deltaTime * rotationSpeed;
                    pivotPointMarteauMj.localEulerAngles = currentEulerAnglesMj;
                    yield return null; //Attendre la prochaine trame
                }
                //on descend la vie du clou
                vieDuClou -= mjForce; 
                MoveClou(vieDuClou);
            }
        //quand c fini replace les marteaux
        //faire remonter le marteau du player
            if (tourJoueur){
                while (currentEulerAnglesPlayer.z > 0f){
                    z = -0.05f * playerForce;
                    currentEulerAnglesPlayer += new Vector3(0,0,z) * Time.deltaTime * rotationSpeed;
                    pivotPointMarteauPlayer.localEulerAngles = currentEulerAnglesPlayer;
                    yield return null; //Attendre la prochaine trame
                }
                tourJoueur = false;
            }
            //faire remonter le marteau du Mj
            else {
                while (currentEulerAnglesMj.z < -0f ){
                    z = 0.05f * mjForce;
                    currentEulerAnglesMj += new Vector3(0,0,z) * Time.deltaTime * rotationSpeed;
                    pivotPointMarteauMj.localEulerAngles = currentEulerAnglesMj;
                    yield return null ; //Attendre la prochaine trame
                }
            }
            //on attend 2 secondes la fin de la coroutine 
            isSpaceEnabled = false; //on desactive la touche espace 
            //Debug.Log("Espace  8 = " + isSpaceEnabled);
            yield return new WaitForSeconds(1f); 
            isMoving = false;
            isSpaceEnabled = false; //on desactive la touche espace 
            //Debug.Log("Espace 9 = " + isSpaceEnabled);
            ////debug.Log(isMoving);
            ////debug.Log("Tour joueur : " + tourJoueur);
           
            
            if (vieDuClou <= 0){
                FinDuJeu();
            } 
            else {
                //On affiche la question suivante 
                tourJoueur = false;
                Invoke("AfficheLaQuestion",1f);
            }
    }
    
    //fin du jeu 
    private void FinDuJeu(){
        ////debug.Log("GameOver");
        buttonTextMarteau.SetActive(false);
        finDuJeu = true;
        //si c'est tourJoueur = false alors le player a gagné
        if (tourJoueur) {
            MJText.text = "Maître du jeu : Bravo vous avez remporté l'épreuve et une recommandation";
            //envoi vers le Main Game Manager le scoreClou 
                MainGameManager.Instance.UpdateScore(MainGameManager.Instance.scoreRecoClou+= 1);
        }
        else {
            MJText.text = "Maître du jeu : Vous avez échoué, je détruis une recommandation";
        }
        MainGameManager.Instance.nbPartieClouJoue += 1;
        
        if(MainGameManager.Instance.nbPartieClouJoue == 3 ){
            MainGameManager.Instance.gameClouFait = true;
            StartCoroutine(LoadSceneAfterDelay("SalleClous", 2f));
        }
        else
        {
            StartCoroutine(LoadSceneAfterDelay("SalleDes", 2f));
        }

    }
    // Coroutine pour charger la scène après un délai
    private IEnumerator LoadSceneAfterDelay(string sceneName, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene(sceneName);
    }
}
