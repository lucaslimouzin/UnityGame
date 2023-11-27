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
    public Button buttonA;
    public Button buttonB;
    public Button buttonC;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI propositionAtext;
    public TextMeshProUGUI propositionBtext;
    public TextMeshProUGUI propositionCtext;
    public TextMeshProUGUI nbBatonRetraitText;
    public TextMeshProUGUI MJText;
    private bool aJuste = false;
    private bool isMoving = false;
    private int numQuestions = 0;
    private int batonTailleTab = 0;
    private bool win = false;
    

    

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
    private void HandleScoreUpdated(int newScore)
    {
        // Faire quelque chose avec le nouveau score
        Debug.Log("Nouveau score : " + newScore);
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
        //On affiche la question
        Invoke("AfficherPanneauQuestions",0.5f);
    }


    //affichage de la question
    private void AfficherPanneauQuestions(){
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
        nbBatonRetraitText.text = question.nbRetraitBaton.ToString();

        buttonA.onClick.AddListener(() => OnButtonClick("A", question.reponseCorrecte, question.nbRetraitBaton));
        buttonB.onClick.AddListener(() => OnButtonClick("B", question.reponseCorrecte, question.nbRetraitBaton));
        buttonC.onClick.AddListener(() => OnButtonClick("C", question.reponseCorrecte, question.nbRetraitBaton));
        
    }

    //fonction qui check le bouton enfoncé
    private void OnButtonClick(String choix, string reponseCorrecte, int nbRetraitBaton){
        // Supprimer tous les écouteurs d'événements du bouton
        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.RemoveAllListeners();
        buttonC.onClick.RemoveAllListeners();

        if (choix == reponseCorrecte){
            aJuste = true;
            
            //envoi vers le Main Game Manager le score 
            MainGameManager.Instance.UpdateScore(MainGameManager.Instance.scoreReco += 1);
        }
        else {
            aJuste = false;
        }
        //on enlève le panneau des questions
        RetraitPanneauQuestions(aJuste, nbRetraitBaton);
    }

    //retrait panneau Question
    private void RetraitPanneauQuestions(bool reponseJuste, int nbRetraitBaton){
        panelQuestions.SetActive(false);
        panelInfoMJ.SetActive(true);
        
        MoveBaton(reponseJuste, nbRetraitBaton);
    }

    private void MoveBaton(bool reponseJuste, int nbRetraitBaton)
    {
        Debug.Log(nbRetraitBaton);
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
                    MJText.text = "Maitre du jeu : Bien répondu, vous retirez des bâtons !";
                    baton[i].transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
                    win = true;
                } 
                else {
                    MJText.text = "Maitre du jeu : Ce n'est pas la bonne réponse, je retire des bâtons !";
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

        numQuestions++;
        if (numQuestions < listBatonQuestions.questions.Length)
        {
            batonTailleTab = startIndex;
            // Il reste des questions, affichez la suivante
            Invoke("AfficherPanneauQuestions",2.5f);
        }
        else
        {
            if (win){
                MJText.text = "Maitre du jeu : bravo vous avez remporté l'Epreuve !";
            }
            else {
                MJText.text = "Maitre du jeu : Dommage, vous avez échoué si prêt du but !";
            }
            // Toutes les questions ont été posées, fin du jeu
            Invoke("FinDuJeu", 3f);
        }
    }

    //fin du jeu 
    private void FinDuJeu(){
        MainGameManager.Instance.gameBatonFait = true;
       SceneManager.LoadScene("salleBatons");

    }
}
