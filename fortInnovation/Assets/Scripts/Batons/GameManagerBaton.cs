using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManagerBaton : MonoBehaviour
{
    public GameObject[] baton; 
    public GameObject panelInstruction;
    public GameObject panelQuestions;
    public GameObject panelInfoMJ;
    public GameObject panelButtonBaton;
    public Button buttonA;
    public Button buttonB;
    public Button buttonC;
    public Button button1Baton;
    public Button button2Baton;
    public Button button3Baton;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI propositionAtext;
    public TextMeshProUGUI propositionBtext;
    public TextMeshProUGUI propositionCtext;
    public TextMeshProUGUI MJText;
    private bool aJuste = false;
    private bool isMoving = false;
    private int numQuestions = 0;
    private int batonTailleTab = 0;
    private bool win = false;
    private bool firstTimeMj;
    

    [System.Serializable]
    public class QuestionData
    {
        public int idQuestion;
        public string question;
        public string[] propositions;
        public string reponseCorrecte;
        public int nbRetraitBaton;
    }

    [System.Serializable]
    public class BatonQuestions
    {
        public QuestionData[] questions;
    }

    private BatonQuestions listBatonQuestions; 

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
    private void HandleScoreUpdated(int scoreBaton, int scoreClou)
    {
        // Faire quelque chose avec le nouveau score
        MainGameManager.Instance.scoreReco = scoreBaton + scoreClou; 
       
    }
    //-------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        // Charger le fichier JSON (assurez-vous de placer le fichier dans le dossier Resources)
        TextAsset jsonFile = Resources.Load<TextAsset>("BatonQuestions");
        // Désérialiser les données JSON
        listBatonQuestions = JsonUtility.FromJson<BatonQuestions>(jsonFile.ToString());
        batonTailleTab = baton.Length;
        firstTimeMj = false;
      //on affiche le panneau des régles
        PanneauRegle();
    }

    // Update is called once per frame
    void Update()
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

        if (MainGameManager.Instance.quiCommence == "Player"){
            //On affiche la question
            Invoke("AfficherPanneauQuestions",0f);
        }
        else {
            panelInfoMJ.SetActive(true);
            firstTimeMj = true;
            MJText.text = "Maitre du jeu : Je commence à retirer des bâtons !";
            TourDuMj(false);
        }
        
    }


    //affichage de la question
    private void AfficherPanneauQuestions(){
        //choisi les questions de 0 à 19 (inclus)
        numQuestions = UnityEngine.Random.Range(0, 20);

        Questions(numQuestions);
        Debug.Log("lancement de la fonction AfficherPanneauQuestions");
        if (panelInstruction.activeSelf){
            panelInstruction.SetActive(false);
        }
        if (panelInfoMJ.activeSelf){
            panelInfoMJ.SetActive(false);
        }
        panelQuestions.SetActive(true);
        

    }

    

    private void Questions(int num){
        Debug.Log("lancement de la fonction Questions");
        QuestionData question = listBatonQuestions.questions[num];

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
            MJText.text = "Maitre du jeu : Bien répondu, vous pouvez retirer des bâtons !";
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
            if (!firstTimeMj) {
                MJText.text = "Maitre du jeu : Ce n'est pas la bonne réponse, je retire des bâtons !";
            }
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
        //pour faire en sorte d'ajouter un peu de complexité
        if (batonTailleTab >= 4) {
            //retire un nombre aléatoire entre 1 et 3 (inclus)
            nbRetraitBaton = UnityEngine.Random.Range(1, 4);
        }
        else if (batonTailleTab == 3){
            //retire un nombre aléatoire entre 1 et 2 (inclus)
            nbRetraitBaton = UnityEngine.Random.Range(1, 3);
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
        Debug.Log(nbRetraitBaton);
        float moveSpeed = 2f;
        float elapsedTime = 0f;

        Debug.Log("startIndex = " + (batonTailleTab - nbRetraitBaton));
        Debug.Log("Nb retrait baton " + nbRetraitBaton);
        // Calculer l'indice de départ pour le retrait des batons
        int startIndex = batonTailleTab - nbRetraitBaton;
    
        // Vérifier si l'indice de départ est valide
        if (startIndex < 0)
        {
            Debug.LogError("Nombre de batons à retirer supérieur au nombre total de batons.");
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
        Debug.Log("Mouvement terminé");
        button1Baton.onClick.RemoveAllListeners();
        button2Baton.onClick.RemoveAllListeners();
        button3Baton.onClick.RemoveAllListeners();

        //choisi les questions de 0 à 19 (inclus)
        numQuestions = UnityEngine.Random.Range(0, 20);
        batonTailleTab = startIndex;
        if (batonTailleTab > 1)
        {
            // Il reste des batons
            Invoke("AfficherPanneauQuestions",1f);
        }
        else
        {
            if (win){
                MJText.text = "Maitre du jeu : bravo vous avez remporté l'Epreuve et une recommandation !";
                //envoi vers le Main Game Manager le scoreBaton 
                MainGameManager.Instance.UpdateScore(MainGameManager.Instance.scoreRecoBaton += 1);
            }
            else {
                MJText.text = "Maitre du jeu : Dommage, vous avez échoué si prêt du but je détruis la recommandation !";
            }
            // Toutes les questions ont été posées, fin du jeu
            MainGameManager.Instance.nbPartieBatonJoue += 1;
            Invoke("FinDuJeu", 2f);
        }
    }

    //fin du jeu 
    private void FinDuJeu(){
        if(MainGameManager.Instance.nbPartieBatonJoue == 4 ){
            MainGameManager.Instance.gameBatonFait = true;
            SceneManager.LoadScene("SalleBatons");
        }
        else {
            SceneManager.LoadScene("SalleDes");
        }
        

    }
}
