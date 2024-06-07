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
using System.Linq;

public class GameManagerInstructions : MonoBehaviour
{



    public GameObject panelInstruction;
    public GameObject panelInfoMJ;
    public GameObject panelEnigmes;
    public GameObject buttonTextEnigmes;
    public TextMeshProUGUI MJText;
    private bool tourJoueur = true;
    private bool finDuJeu = false;
    public bool isMoving = false;

    //variables damier
    public List<LetterButton> allLetterButtons; // Liste de tous les boutons de lettres
    public string wordToFind = "ECOSYSTEME"; // Le mot à trouver
 
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
    private void HandleScoreUpdated(int scorePaires, int scoreBaton, int scoreClou, int scoreBassin, int scoreEnigmes)
    {
        // Faire quelque chose avec le nouveau score
        MainGameManager.Instance.scoreReco = scoreBaton + scoreClou + scoreBassin + scoreEnigmes; 
       
    }
    //-------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        
        

        //charge la coroutine qui va récupérer le fichier Json 
        //StartCoroutine(LoadJsonFromNetwork()); //a activer lors du déploiment
        //charge la coroutine qui va récupérer le fichier Json 
        StartCoroutine(LoadJsonFromLocal());
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

    private void Update()
    {
       
    }

    //affichage du panneau des règles
    private void PanneauRegle (){
        panelInstruction.SetActive(true);
    }
    
    // retrait panneau des règles
    //affichage du panneau de la règle
    public void RetraitPanneauRegle (){
        panelInstruction.SetActive(false);
        TourDuJoueur();
    //    if (MainGameManager.Instance.quiCommence == "Player"){
    //         panelInfoMJ.SetActive(true);
    //         MJText.text = "Maître du jeu : J'ai perdu aux dés, je dépose une bille dans le verre";
    //         tourJoueur = false;
    //         TourDuMj();
    //     }
    //     else {
    //         //On affiche la question
    //         Invoke("AfficheLaQuestion",0f);
    //         //tour du player   
    //     } 
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
            MJText.text = "Maître du jeu : Bien répondu, je dépose une bille dans le verre";
            tourJoueur = false;
            
            TourDuMj();
            
        } 
        else {
            MJText.text = "Maître du jeu : Ce n'est pas la bonne réponse, déposez une bille dans le verre";
            tourJoueur = true;
            
            TourDuJoueur();
        }
        
    }



    private void TourDuJoueur(){
        if(tourJoueur) {
            isMoving = true;
            buttonTextEnigmes.SetActive(true);
            ////debug.Log("Debut tour joueur");  
        }  
    }

    private void TourDuMj(){
        if (!tourJoueur) {
            isMoving = true;
            buttonTextEnigmes.SetActive(false);
            ////debug.Log("Debut tour Mj");    
        }        
    }  
    
    
     public void ValidateWord()
    {
        string selectedWord = "";
        foreach (LetterButton letterButton in allLetterButtons)
        {
            if (letterButton.isSelected)
            {
                selectedWord += letterButton.gameObject.name; // Utilisez le nom du GameObject ou une propriété personnalisée pour la lettre
                Debug.Log(selectedWord);
            }
        }

        if (IsWordCorrect(selectedWord, wordToFind))
        {
            Debug.Log("Mot correct !");
            // Actions pour un mot correct
            tourJoueur = false;
            FinDuJeu();
        }
        else
        {
            Debug.Log("Mot incorrect.");
            // Actions pour un mot incorrect
            tourJoueur = true;
            FinDuJeu();
        }
    }

    bool IsWordCorrect(string selectedWord, string wordToFind)
    {
        return selectedWord.OrderBy(c => c).SequenceEqual(wordToFind.OrderBy(c => c));
    }





    //fin du jeu 
    private void FinDuJeu(){
        ////debug.Log("GameOver");
        buttonTextEnigmes.SetActive(false);
        finDuJeu = true;
        //si c'est tourJoueur = false alors le player a gagné
        if (!tourJoueur) {
            MJText.text = "Maître du jeu : Bravo le mot était bien Ecosystème, vous avez remporté une recommandation";
            //envoi vers le Main Game Manager le scoreEnigme
                MainGameManager.Instance.UpdateScore(MainGameManager.Instance.scoreRecoEnigmes+= 2);
        }
        else {
            MJText.text = "Maître du jeu : Vous avez échoué, je détruis une recommandation";
        }
        MainGameManager.Instance.nbPartieEnigmesJoue += 1;
        
        if(MainGameManager.Instance.nbPartieEnigmesJoue == 1 ){
            MainGameManager.Instance.gameEnigmesFait = true;
            StartCoroutine(LoadSceneAfterDelay("SalleEnigmes", 2f));
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

