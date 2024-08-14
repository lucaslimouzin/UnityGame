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
public class GameManagerBassin : MonoBehaviour
{
    // ajout v2
    public Image imageScore;
    public GameObject panelComplémentReponse;
    public TextMeshProUGUI questionTextReponse;
    public TextMeshProUGUI textComplementBonneReponse;
    public TextMeshProUGUI textComplementReponse;
    
    public TextMeshProUGUI boutonTextReponse;
    private string nomDoc;
    //fin ajout v2

    public GameObject wall;
    public GameObject panelBassinHead;
    public Sprite[] headCharacter;
    public Image headAffiche;
    private GameObject playerSphere;
    public GameObject playerSpherePrefab;
    private GameObject mjSphere;
    public GameObject mjSpherePrefab;
    //public GameObject panelInstructions;
    public GameObject panelInfoMJ;
    public GameObject panelBassin;
    public TextMeshProUGUI MJText;
    public TextMeshProUGUI textVieVerreMJ;
     public TextMeshProUGUI textVieVerreJoueur;

    private bool tourJoueur = true;
    private bool aRelacher = false;
    private float vieDuVerreMj;
    private float vieDuVerreJoueur;
    private bool finDuJeu = false;
    public bool isMoving = false;

    private float seuilHauteurY = 1.30f;
 
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
     public Sprite[] imagesButton;
    public GameObject buttonFermer;
    public TextMeshProUGUI gagnePerduText;
    private bool aJuste = false;
    private int numQuestions = 0;
    

    [System.Serializable]
    public class QuestionData
    {
        public int idQuestion;
        public string question;
        public string[] propositions;
        public string reponseCorrecte;
        public string complementReponse;

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
    private void HandleScoreUpdated(int scorePaires, int scoreBaton, int scoreClou, int scoreBassin, int scoreEnigmes)
    {
        // Faire quelque chose avec le nouveau score
        MainGameManager.Instance.scoreReco = scorePaires + scoreBaton + scoreClou + scoreBassin + scoreEnigmes; 
       
    }
    //-------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        //ajout v2
         if(MainGameManager.Instance.niveauSelect =="Normal"){
            imageScore.sprite= MainGameManager.Instance.imageScore[0];
        }else{
            imageScore.sprite= MainGameManager.Instance.imageScore[1];
        }
        
        wall.SetActive(false);
        switch (MainGameManager.Instance.selectedCharacter) {
            case 0: 
                headAffiche.sprite = headCharacter[0];
                break;
            case 1: 
                headAffiche.sprite = headCharacter[1];
                break;
            case 2: 
                headAffiche.sprite = headCharacter[2];
                break;

        }   
        gagnePerduText.gameObject.SetActive(false); // Masque le texte

        //charge la coroutine qui va récupérer le fichier Json 
        //StartCoroutine(LoadJsonFromNetwork()); //a activer lors du déploiment
        //charge la coroutine qui va récupérer le fichier Json 
        StartCoroutine(LoadJsonFromLocal());

        vieDuVerreMj = 3;
        vieDuVerreJoueur= 3;
        
        textVieVerreJoueur.text = vieDuVerreJoueur.ToString() + " billes restantes \navant que votre verre \n ne coule ";
        textVieVerreMJ.text = vieDuVerreMj.ToString() + " billes restantes \navant que son verre ne coule ";
        //on affiche le panneau des régles
        //PanneauRegle();
        RetraitPanneauRegle();
    }

    //fonction qui charge les questions depuis local
    IEnumerator LoadJsonFromLocal(){
        if(MainGameManager.Instance.niveauSelect =="Normal"){
            nomDoc = "BassinQuestions";
        }else{
            nomDoc = "BassinQuestionsFacile";//Mode facile
        }
        // Charger le fichier JSON Normal
        TextAsset jsonFile = Resources.Load<TextAsset>(nomDoc);
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
        VerifierHauteurPlayerSphere();
    }

    //affichage du panneau des règles
    // private void PanneauRegle (){
    //     panelInstruction.SetActive(true);
    // }
    
    // retrait panneau des règles
    //affichage du panneau de la règle
    public void RetraitPanneauRegle (){
        //panelInstruction.SetActive(false);
       if (MainGameManager.Instance.quiCommence == "Player"){
            //On affiche la question
            Invoke("AfficheLaQuestion",0f);
        }
        else {
            
            //tour du MJ  
            panelInfoMJ.SetActive(true);
            MJText.text = "le Maître du jeu a gagné aux dés, il dépose une bille dans votre verre";
            tourJoueur = false;
            TourDuMj();
        } 
    }

    //affichage de la question   
    private void AfficheLaQuestion(){

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
        // if (panelInstruction.activeSelf){
        //     panelInstruction.SetActive(false);
        // }
        if (panelInfoMJ.activeSelf){
            panelInfoMJ.SetActive(false);
        }
        panelQuestions.SetActive(true);
        panelBassinHead.SetActive(false);

        QuestionData question = listQuestions.questions[numQuestions];
        //affichage des données
        questionText.text = question.question;
        propositionAtext.text = question.propositions[0];
        propositionBtext.text = question.propositions[1];
        propositionCtext.text = question.propositions[2];

        //ajout v2
        if (MainGameManager.Instance.niveauSelect == "Facile"){
            questionTextReponse.text = question.question;
             //on recolor juste la réponse juste
            switch (question.reponseCorrecte)
            {
                case "A":
                    textComplementBonneReponse.text = question.propositions[0];
                    break;
                case "B":
                    textComplementBonneReponse.text = question.propositions[1];
                    break;
                case "C":
                    textComplementBonneReponse.text = question.propositions[2];
                    break;
            }
            
            textComplementReponse.text = question.complementReponse;
        }

        //met le fond des boutons en noir
        buttonA.image.sprite = imagesButton[0]; // "black"
        buttonB.image.sprite = imagesButton[0]; // "black"
        buttonC.image.sprite = imagesButton[0]; // "black"
        //desactive le boutonFermer
        buttonFermer.SetActive(false);

        buttonA.onClick.AddListener(() => OnButtonClick("A", question.reponseCorrecte));
        buttonB.onClick.AddListener(() => OnButtonClick("B", question.reponseCorrecte));
        buttonC.onClick.AddListener(() => OnButtonClick("C", question.reponseCorrecte));
        
    }

    //fonction qui check le bouton enfoncé
    private void OnButtonClick(String choix, string reponseCorrecte){
        // Supprimer tous les écouteurs d'événements du bouton
        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.RemoveAllListeners();
        buttonC.onClick.RemoveAllListeners();

        if (choix == reponseCorrecte){
            aJuste = true;
            //on met tout en rouge 
            buttonA.image.sprite = imagesButton[1]; // "red"
            buttonB.image.sprite = imagesButton[1]; // "red"
            buttonC.image.sprite = imagesButton[1]; // "red"
            // Mettre en vert le texte qu'il a cliqué
            switch (choix)
            {
                case "A":
                    buttonA.image.sprite = imagesButton[2]; // "green"
                    break;
                case "B":
                    buttonB.image.sprite = imagesButton[2]; // "green"
                    break;
                case "C":
                    buttonC.image.sprite = imagesButton[2]; // "green"
                    break;
            }
        }
        else {
            aJuste = false;
            // Mettre en rouge le texte qu'il a cliqué et en vert le texte de la bonne réponse
            //on met tout en rouge 
            buttonA.image.sprite = imagesButton[1]; // "red"
            buttonB.image.sprite = imagesButton[1]; // "red"
            buttonC.image.sprite = imagesButton[1]; // "red"
            
            //on recolor juste la réponse juste
            switch (reponseCorrecte)
            {
                case "A":
                    buttonA.image.sprite = imagesButton[2]; // "green"
                    break;
                case "B":
                    buttonB.image.sprite = imagesButton[2]; // "green"
                    break;
                case "C":
                    buttonC.image.sprite = imagesButton[2]; // "green"
                    break;
            }
            
        }
        // affiche le bouton fermer & v2
        if (MainGameManager.Instance.niveauSelect == "Facile"){
            boutonTextReponse.text = "Suivant";
        }
        buttonFermer.SetActive(true);
        
    }

    //action du bouton fermer v2
    public void clicBoutonPanelReponse(){
        panelComplémentReponse.SetActive(false);
        panelBassinHead.SetActive(true);
        RetraitPanneauQuestions(aJuste);
        
    }

     //action du bouton fermer
    public void clicBoutonPanelQuestion(){
        //si on a choisi le mode Facile
        if (MainGameManager.Instance.niveauSelect == "Facile"){
            panelComplémentReponse.SetActive(true);
        }else {
            panelBassinHead.SetActive(true);
            RetraitPanneauQuestions(aJuste);
        }
    }

    //retrait panneau Question
    private void RetraitPanneauQuestions(bool reponseJuste){
        panelQuestions.SetActive(false);
        panelInfoMJ.SetActive(true);
        if(reponseJuste){
            MJText.text = "Vous avez bien répondu ! \nVous pouvez donc déplacer une bille, jusqu'à ce qu'elle devienne verte, dans son verre.";
            tourJoueur = true;
            TourDuJoueur();
            
        } 
        else {
            MJText.text = "Vous n'avez pas donné la bonne réponse...\nLe Maître du jeu va placer une bille dans votre verre.";
            tourJoueur = false;
            TourDuMj();
        }
        
    }



    private void TourDuJoueur(){
        if(tourJoueur) {
            isMoving = true;
            wall.SetActive(true);
            ////debug.Log("Debut tour joueur");
            AfficherPlayerSphere();
            
            
        }  
    }

    private void TourDuMj(){
        if (!tourJoueur) {
            isMoving = true;
            wall.SetActive(false);
            ////debug.Log("Debut tour Mj");
            LancerMouvementMjSphere();
        }        
    }  
    
    // Fonction pour afficher le prefab PlayerSpheres
    private void AfficherPlayerSphere()
    {
        // Vérifier si le prefab est assigné
        if (playerSpherePrefab != null)
        {
            // Instantier le prefab
            GameObject nouvelleSpherePlayer = Instantiate(playerSpherePrefab);
            playerSphere = nouvelleSpherePlayer;
        }
        else
        {
            Debug.LogError("PlayerSpheresPrefab n'est pas assigné dans l'inspecteur!");
        }
    }

    // Vérifie la hauteur de la playerSphere
    private void VerifierHauteurPlayerSphere()
    {
        
        if (playerSphere != null && tourJoueur && isMoving)
        {   
            if (playerSphere.transform.position.y <= seuilHauteurY)
            {   
                isMoving = false;
                vieDuVerreMj -= 1;
                textVieVerreMJ.text = vieDuVerreMj.ToString() + " billes restantes \navant que son verre ne coule";
                //Debug.Log(vieDuVerre);
                //vérifie si le verre peut encore recevoir une bille
                if (vieDuVerreMj > 0){

                    Invoke("AfficheLaQuestion",1.5f);
                }
                else {
                    FinDuJeu();
                }
                
            }
        }
        if (mjSphere != null && !tourJoueur && isMoving)
        {
            if (mjSphere.transform.position.y <= seuilHauteurY)
            {   
                isMoving = false;
                vieDuVerreJoueur -= 1;
                textVieVerreJoueur.text = vieDuVerreJoueur.ToString() + " billes restantes \navant que votre verre \n ne coule";
                //Debug.Log(vieDuVerre);
                //vérifie si le verre peut encore recevoir une bille
                if (vieDuVerreJoueur > 0){
                    Invoke("AfficheLaQuestion",1.5f);
                }
                else {
                    FinDuJeu();
                }
            }
        }
    }

    private void LancerMouvementMjSphere()
    {
        GameObject nouvelleSphereMj = Instantiate(mjSpherePrefab);
        mjSphere = nouvelleSphereMj;
        StartCoroutine(MouvementMjSphere());
    }

    private IEnumerator MouvementMjSphere()
    {
        // Monter en Y
        yield return Monter(2.0f, 1.0f); // Monter à la hauteur 2 en 1 seconde

        // Se déplacer sur X
        yield return DeplacerX(2.8f, 1.25f); // Se déplacer jusqu'à 2.8 en 1.25 seconde

        // Faire tomber
        // Vous pouvez ajouter ici la logique pour faire tomber la sphère
    }

    private IEnumerator Monter(float hauteur, float duree)
    {
        Vector3 startPosition = mjSphere.transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, hauteur, startPosition.z);
        float elapsedTime = 0;

        while (elapsedTime < duree)
        {
            mjSphere.transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / duree));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mjSphere.transform.position = endPosition;
    }

    private IEnumerator DeplacerX(float positionX, float duree)
    {
        Vector3 startPosition = mjSphere.transform.position;
        Vector3 endPosition = new Vector3(positionX, startPosition.y, startPosition.z);
        float elapsedTime = 0;

        while (elapsedTime < duree)
        {
            mjSphere.transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / duree));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mjSphere.transform.position = endPosition;
        Renderer renderer = mjSphere.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.green;
        }
    }

    IEnumerator ShowAndHideGagneText()
    {
        gagnePerduText.gameObject.SetActive(true); // Affiche le texte
        gagnePerduText.text = "vous avez gagné !";
        // Change la couleur du texte en vert
        gagnePerduText.color = Color.green;
        yield return new WaitForSeconds(1f);  // Attend 1 seconde
    }
    IEnumerator ShowAndHidePerduText()
    {
        gagnePerduText.gameObject.SetActive(true); // Affiche le texte
        gagnePerduText.text = "vous avez perdu !";
        // Change la couleur du texte en vert
        gagnePerduText.color = Color.red;
        yield return new WaitForSeconds(1f);  // Attend 1 seconde
    }


    //fin du jeu 
    private void FinDuJeu(){
        ////debug.Log("GameOver");
        finDuJeu = true;
        //si c'est tourJoueur = false alors le player a gagné
        if (tourJoueur) {
            //ajout v2
            if(MainGameManager.Instance.niveauSelect =="Normal"){
                MJText.text = "Bravo vous avez remporté une recommandation";
            }else{
                MJText.text = "Bravo vous avez remporté le duel";
            }
            //envoi vers le Main Game Manager le scoreClou 
                MainGameManager.Instance.UpdateScore(MainGameManager.Instance.scoreRecobassin+= 1);
                StartCoroutine(ShowAndHideGagneText());
        }
        else {
            if(MainGameManager.Instance.niveauSelect =="Normal"){
                MJText.text = "Vous avez échoué, je détruis une recommandation";
            }else{
                MJText.text = "Vous avez échoué ce duel";
            }
            StartCoroutine(ShowAndHidePerduText());
        }
        MainGameManager.Instance.nbPartieBassinJoue += 1;
        
        if(MainGameManager.Instance.nbPartieBassinJoue > MainGameManager.Instance.nbPartieBassin ){
            MainGameManager.Instance.gameBassinFait = true;
            StartCoroutine(LoadSceneAfterDelay("SalleBassins", 4f));
        }
        else
        {
            StartCoroutine(LoadSceneAfterDelay("SalleDes", 4f));
        }

    }
    // Coroutine pour charger la scène après un délai
    private IEnumerator LoadSceneAfterDelay(string sceneName, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene(sceneName);
    }
}

