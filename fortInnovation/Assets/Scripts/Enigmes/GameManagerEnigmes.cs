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

public class GameManagerEnigmes : MonoBehaviour
{
    //public GameObject panelInstruction;
    public GameObject panelInfoMJ;
    public GameObject panelEnigmes;
    public GameObject buttonTextEnigmes;
    public TextMeshProUGUI MJText;
    public TextMeshProUGUI textEnigmeInfo;
    private bool tourJoueur = true;
    private bool finDuJeu = false;
    

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
    public TextMeshProUGUI gagnePerduText;
    
    

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
        MainGameManager.Instance.scoreReco = scorePaires + scoreBaton + scoreClou + scoreBassin + scoreEnigmes; 
       
    }
    //-------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        
        
        gagnePerduText.gameObject.SetActive(false); // Masque le texte

        //charge la coroutine qui va récupérer le fichier Json 
        //StartCoroutine(LoadJsonFromNetwork()); //a activer lors du déploiment
        //charge la coroutine qui va récupérer le fichier Json 
        StartCoroutine(LoadJsonFromLocal());
        //on affiche le panneau des régles
        //PanneauRegle();
        RetraitPanneauRegle();
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
    // private void PanneauRegle (){
    //     panelInstruction.SetActive(true);
    // }
    
    // retrait panneau des règles
    //affichage du panneau de la règle
    public void RetraitPanneauRegle (){
        // panelInstruction.SetActive(false);
        TourDuJoueur();
    }

    private void TourDuJoueur(){
        if(tourJoueur) {
            buttonTextEnigmes.SetActive(true);
            ////debug.Log("Debut tour joueur");  
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
        buttonTextEnigmes.SetActive(false);
        textEnigmeInfo.gameObject.SetActive(false);
        finDuJeu = true;
        panelInfoMJ.SetActive(true);
        //si c'est tourJoueur = false alors le player a gagné
        if (!tourJoueur) {
            MJText.text = "Maître du jeu : Bravo le mot était bien Ecosystème, vous avez remporté l'épreuve et une recommandation";
            //envoi vers le Main Game Manager le scoreEnigme
            MainGameManager.Instance.UpdateScore(MainGameManager.Instance.scoreRecoEnigmes+= 2);
            StartCoroutine(ShowAndHideGagneText());
        }
        else {
            MJText.text = "Maître du jeu : Vous avez échoué, je détruis une recommandation";
            StartCoroutine(ShowAndHidePerduText());
        }
        MainGameManager.Instance.nbPartieEnigmesJoue += 1;
        
        if(MainGameManager.Instance.nbPartieEnigmesJoue == 1 ){
            MainGameManager.Instance.gameEnigmesFait = true;
            StartCoroutine(LoadSceneAfterDelay("SalleEnigmes", 4f));
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

