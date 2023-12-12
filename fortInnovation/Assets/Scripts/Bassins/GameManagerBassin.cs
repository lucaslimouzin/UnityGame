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
    private GameObject playerSphere;
    public GameObject playerSpherePrefab;
    private GameObject mjSphere;
    public GameObject mjSpherePrefab;
    public GameObject panelInstruction;
    public GameObject panelInfoMJ;
    public GameObject panelBassin;
    public GameObject buttonTextBassin;
    public TextMeshProUGUI MJText;

    private bool tourJoueur = true;
    private bool aRelacher = false;
    private float vieDuVerre;
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
    private bool aJuste = false;
    private int numQuestions = 0;
    

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
    private void HandleScoreUpdated(int scoreBaton, int scoreClou, int scoreBassin)
    {
        // Faire quelque chose avec le nouveau score
        MainGameManager.Instance.scoreReco = scoreBaton + scoreClou + scoreBassin; 
       
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

        vieDuVerre = 6;
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
        VerifierHauteurPlayerSphere();
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
            panelInfoMJ.SetActive(true);
            MJText.text = "Maître du jeu : J'ai perdu aux dés, je dépose une bille dans le verre";
            tourJoueur = false;
            TourDuMj();
        }
        else {
            //On affiche la question
            Invoke("AfficheLaQuestion",0f);
            //tour du player   
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
        }
        else {
            aJuste = false;
        }
        //on enlève le panneau des questions
        RetraitPanneauQuestions(aJuste);
    }

    //retrait panneau Question
    private void RetraitPanneauQuestions(bool reponseJuste){
        panelQuestions.SetActive(false);
        panelInfoMJ.SetActive(true);
        if(reponseJuste){
            MJText.text = "Maitre du jeu : Bien répondu, je dépose une bille dans le verre";
            tourJoueur = false;
            
            TourDuMj();
            
        } 
        else {
            MJText.text = "Maitre du jeu : Ce n'est pas la bonne réponse, déposez une bille dans le verre";
            tourJoueur = true;
            
            TourDuJoueur();
        }
        
    }



    private void TourDuJoueur(){
        if(tourJoueur) {
            isMoving = true;
            buttonTextBassin.SetActive(true);
            ////debug.Log("Debut tour joueur");
            AfficherPlayerSphere();
            
            
        }  
    }

    private void TourDuMj(){
        if (!tourJoueur) {
            isMoving = true;
            buttonTextBassin.SetActive(false);
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
                vieDuVerre -= 1;
                //Debug.Log(vieDuVerre);
                //vérifie si le verre peut encore recevoir une bille
                if (vieDuVerre > 0){

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
                vieDuVerre -= 1;
                //Debug.Log(vieDuVerre);
                //vérifie si le verre peut encore recevoir une bille
                if (vieDuVerre > 0){
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
        yield return DeplacerX(1.6f, 1.0f); // Se déplacer jusqu'à 1.6 en 1 seconde

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

    //fin du jeu 
    private void FinDuJeu(){
        ////debug.Log("GameOver");
        buttonTextBassin.SetActive(false);
        finDuJeu = true;
        //si c'est tourJoueur = false alors le player a gagné
        if (!tourJoueur) {
            MJText.text = "Maître du jeu : Bravo vous avez remporté l'épreuve et une recommandation";
            //envoi vers le Main Game Manager le scoreClou 
                MainGameManager.Instance.UpdateScore(MainGameManager.Instance.scoreRecobassin+= 1);
        }
        else {
            MJText.text = "Maître du jeu : Vous avez échoué, je détruis une recommandation";
        }
        MainGameManager.Instance.nbPartieBassinJoue += 1;
        
        if(MainGameManager.Instance.nbPartieBassinJoue == 3 ){
            MainGameManager.Instance.gameBassinFait = true;
            StartCoroutine(LoadSceneAfterDelay("SalleBassins", 2f));
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

