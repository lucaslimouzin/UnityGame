using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerBaton : MonoBehaviour
{
    public GameObject[] baton; 
    public GameObject panelInstruction;
    public GameObject panelQuestions;
    private Vector3 batonPosition;
    public Button buttonA;
    public Button buttonB;
    public Button buttonC;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI propositionAtext;
    public TextMeshProUGUI propositionBtext;
    public TextMeshProUGUI propositionCtext;
    private bool aJuste = false;
    private bool isMoving = false;
    private int numQuestions = 0;
    

    

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

    // Start is called before the first frame update
    void Start()
    {
        // Charger le fichier JSON (assurez-vous de placer le fichier dans le dossier Resources)
        TextAsset jsonFile = Resources.Load<TextAsset>("BatonQuestions");
        // Désérialiser les données JSON
        listBatonQuestions = JsonUtility.FromJson<BatonQuestions>(jsonFile.ToString());
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
        Invoke("AfficherPanneauQuestions",1f);
    }


    //affichage de la question
    private void AfficherPanneauQuestions(){
        Debug.Log("lancement de la fonction AfficherPanneauQuestions");
        if (panelInstruction.activeSelf){
            panelInstruction.SetActive(false);
        }
        panelQuestions.SetActive(true);
        Questions(numQuestions);

    }

    

    private void Questions(int num){
        Debug.Log("lancement de la fonction Questions");
        QuestionData question = listBatonQuestions.questions[num];

        //affichage des données
        questionText.text = question.question;
        propositionAtext.text = question.propositions[0];
        propositionBtext.text = question.propositions[1];
        propositionCtext.text = question.propositions[2];

        buttonA.onClick.AddListener(() => OnButtonClick("A", question.reponseCorrecte, question.nbRetraitBaton));
        buttonB.onClick.AddListener(() => OnButtonClick("B", question.reponseCorrecte, question.nbRetraitBaton));
        buttonC.onClick.AddListener(() => OnButtonClick("C", question.reponseCorrecte, question.nbRetraitBaton));
        
    }

    //fonction qui check le bouton enfoncé
    private void OnButtonClick(String choix, string reponseCorrecte, int nbRetraitBaton){
        if (choix == reponseCorrecte){
            aJuste = true;
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
        MoveBaton(reponseJuste, nbRetraitBaton);
    }

    private void MoveBaton(bool reponseJuste, int nbRetraitBaton)
    {
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(MoveBatonCoroutine(reponseJuste, nbRetraitBaton));
        }
    }

    private IEnumerator MoveBatonCoroutine(bool reponseJuste, int nbRetraitBaton)
    {
        float moveSpeed = 2f;
        float elapsedTime = 0f;

        // Calculer l'indice de départ pour le retrait des batons
        int startIndex = baton.Length - nbRetraitBaton;

        // Vérifier si l'indice de départ est valide
        if (startIndex < 0)
        {
            Debug.LogError("Nombre de batons à retirer supérieur au nombre total de batons.");
            yield break; // Sortir de la coroutine si l'indice de départ n'est pas valide
        }

        while (elapsedTime < 1f) // Modifier le temps selon vos besoins
        {
            // Déplacer les batons à partir de l'indice de départ
            for (int i = startIndex; i < baton.Length; i++)
            {
                if(reponseJuste){
                    baton[i].transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
                } 
                else {
                    baton[i].transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
                }
                
            }

            elapsedTime += Time.deltaTime;
            yield return null; // Attendre la prochaine trame
        }

        // Détruire les batons à partir de l'indice de départ
        for (int i = startIndex; i < baton.Length; i++)
        {
            Destroy(baton[i]);
        }

        // Arrêter le mouvement après 5 secondes
        isMoving = false;
        Debug.Log("Mouvement terminé");

        numQuestions++;
        if (numQuestions < listBatonQuestions.questions.Length)
        {
            // Il reste des questions, affichez la suivante
            Invoke("AfficherPanneauQuestions",1f);
        }
        else
        {
            // Toutes les questions ont été posées, fin du jeu
            FinDuJeu();
        }
    }

    //fin du jeu 
    private void FinDuJeu(){
        Debug.Log("fin du jeu");

    }
}
