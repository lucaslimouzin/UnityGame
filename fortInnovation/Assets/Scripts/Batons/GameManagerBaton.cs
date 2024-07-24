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
public class GameManagerBaton : MonoBehaviour
{

    // ajout v2
    public Image imageScore;
    public GameObject panelComplémentReponse;
    public TextMeshProUGUI questionTextReponse;
    public TextMeshProUGUI textComplementReponse;
    
    public TextMeshProUGUI boutonTextReponse;
    private string nomDoc;
    //fin ajout v2

    public GameObject[] baton; 
    //public GameObject panelInstruction;
    public GameObject panelInfoMJ;
    public GameObject panelButtonBaton;

    public Button button1Baton;
    public Button button2Baton;
    public Button button3Baton;
    public TextMeshProUGUI MJText;
   
    private bool isMoving = false;
   
    private int batonTailleTab = 0;
    private bool win = false;
    private bool firstTimeMj;
    
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
        gagnePerduText.gameObject.SetActive(false); // Masque le texte

        //charge la coroutine qui va récupérer le fichier Json 
        //StartCoroutine(LoadJsonFromNetwork()); //a activer lors du déploiment
        //charge la coroutine qui va récupérer le fichier Json 
        StartCoroutine(LoadJsonFromLocal());
        
        batonTailleTab = baton.Length;
        firstTimeMj = false;
      //on affiche le panneau des régles
        //PanneauRegle();
        RetraitPanneauRegle();
    }

    //fonction qui charge les questions depuis local
    IEnumerator LoadJsonFromLocal(){
        if(MainGameManager.Instance.niveauSelect =="Normal"){
            nomDoc = "BatonQuestions";
        }else{
            nomDoc = "BatonQuestionsFacile";//Mode facile
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
            panelInfoMJ.SetActive(true);
            firstTimeMj = true;
            MJText.text = "Maître du jeu : Je commence à retirer des bâtons !";
            TourDuMj(false);
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
        } while (MainGameManager.Instance.questionsBatonPosees.Contains(numQuestions));

        MainGameManager.Instance.questionsBatonPosees.Add(numQuestions);
        // Restreindre le nombre total de questions posées
        if (MainGameManager.Instance.questionsBatonPosees.Count >= listQuestions.questions.Length)
        {
            // Si toutes les questions ont été posées, réinitalisation de la liste
            MainGameManager.Instance.questionsBatonPosees.Clear();
        }

        //gestion des panneaux
        // if (panelInstruction.activeSelf){
        //     panelInstruction.SetActive(false);
        // }
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

        //ajout v2
        if (MainGameManager.Instance.niveauSelect == "Facile"){
            questionTextReponse.text = question.question;
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
        RetraitPanneauQuestions(aJuste);
        
    }

    //action du bouton fermer
    public void clicBoutonPanelQuestion(){
        //si on a choisi le mode Facile v2
        if (MainGameManager.Instance.niveauSelect == "Facile"){
            panelComplémentReponse.SetActive(true);
        }else {
            RetraitPanneauQuestions(aJuste);
        }
        
    }

    //retrait panneau Question
    private void RetraitPanneauQuestions(bool reponseJuste){
        panelQuestions.SetActive(false);
        panelInfoMJ.SetActive(true);
        if(reponseJuste){
            MJText.text = "Vous avez bien répondu !\nVous pouvez retirer 1, 2 ou 3 bâtonnets.";
            win = true;
            panelButtonBaton.SetActive(true);
            if (batonTailleTab >= 4) {
            //active les bons buttons
            button1Baton.interactable = true;
            button2Baton.interactable = true;
            button3Baton.interactable = true;
            }
            else if (batonTailleTab == 3){
                //active les bons buttons
                button1Baton.interactable = true;
                button2Baton.interactable = true;
                button3Baton.interactable = false;
            } else {
                //active les bons buttons
                button1Baton.interactable = true;
                button2Baton.interactable = false;
                button3Baton.interactable = false;
            }
            TourDuJoueur(reponseJuste);
        } 
        else {
            
            MJText.text = "Vous n'avez pas donné la bonne réponse...\nC'est donc à moi de retirer des bâtonnets.";
            
            win = false;
            panelButtonBaton.SetActive(false);
            TourDuMj(reponseJuste);
        }
        
    }


    private void TourDuJoueur(bool reponseJuste){
        button1Baton.onClick.AddListener(() => MoveBaton(reponseJuste,1));
        button2Baton.onClick.AddListener(() => MoveBaton(reponseJuste,2));
        button3Baton.onClick.AddListener(() => MoveBaton(reponseJuste,3));
    }

    private void TourDuMj(bool reponseJuste){
        int nbRetraitBaton;
        if (batonTailleTab >= 4) {
            nbRetraitBaton = 3;
        }
        else if (batonTailleTab == 3){
            nbRetraitBaton = 2;
        } else {
            nbRetraitBaton = 1;
        }
        
        MoveBaton(reponseJuste, nbRetraitBaton);
    }



    private void MoveBaton(bool reponseJuste, int nbRetraitBaton)
    {
        
        panelButtonBaton.SetActive(false);
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(MoveBatonCoroutine(reponseJuste, nbRetraitBaton));
        }
    }

    private IEnumerator MoveBatonCoroutine(bool reponseJuste, int nbRetraitBaton)
    {
        ////debug.Log(nbRetraitBaton);
        float moveSpeed = 2f;
        float elapsedTime = 0f;

        ////debug.Log("startIndex = " + (batonTailleTab - nbRetraitBaton));
        ////debug.Log("Nb retrait baton " + nbRetraitBaton);
        // Calculer l'indice de départ pour le retrait des batons
        int startIndex = batonTailleTab - nbRetraitBaton;
    
        // Vérifier si l'indice de départ est valide
        if (startIndex < 0)
        {
            ////debug.LogError("Nombre de batons à retirer supérieur au nombre total de batons.");
            yield break; // Sortir de la coroutine si l'indice de départ n'est pas valide
        }

        while (elapsedTime < 1f) // Modifier le temps selon vos besoins
        {
            // Déplacer les batons à partir de l'indice de départ
            for (int i = startIndex; i < batonTailleTab; i++)
            {
                
                if(reponseJuste){
                    baton[i].transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
                    win = true;
                } 
                else {
                    baton[i].transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
                    win = false;
                }
                
            }

            elapsedTime += Time.deltaTime;
            yield return null; // Attendre la prochaine trame
        }

        // Détruire les batons à partir de l'indice de départ
        for (int i = startIndex; i < batonTailleTab; i++)
        {
            Destroy(baton[i]);
        }

        // Arrêter le mouvement après 5 secondes
        isMoving = false;
        //debug.Log("Mouvement terminé");
        button1Baton.onClick.RemoveAllListeners();
        button2Baton.onClick.RemoveAllListeners();
        button3Baton.onClick.RemoveAllListeners();

        batonTailleTab = startIndex;
        if (batonTailleTab > 1)
        {
            // Il reste des batons
            Invoke("AfficheLaQuestion",1f);
        }
        else
        {
            if (win){
                MJText.text = "Maître du jeu : bravo vous avez remporté une recommandation !";
                //envoi vers le Main Game Manager le scoreBaton 
                MainGameManager.Instance.UpdateScore(MainGameManager.Instance.scoreRecoBaton += 1);
                StartCoroutine(ShowAndHideGagneText());
            }
            else {
                MJText.text = "Maître du jeu : Dommage, vous avez échoué si près du but je détruis la recommandation !";
                StartCoroutine(ShowAndHidePerduText());
            }
            // Toutes les questions ont été posées, fin du jeu
            MainGameManager.Instance.nbPartieBatonJoue += 1;
            Invoke("FinDuJeu",4f);
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
        if(MainGameManager.Instance.nbPartieBatonJoue > MainGameManager.Instance.nbPartieBaton ){
            MainGameManager.Instance.gameBatonFait = true;
            SceneManager.LoadScene("SalleBatons");
        }
        else {
            SceneManager.LoadScene("SalleDes");
        }
        

    }
}
