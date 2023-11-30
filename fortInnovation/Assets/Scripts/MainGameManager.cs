using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager Instance;
    private TextMeshProUGUI scoreTextReco;
    public int scoreReco = 0;

    // variables pour jeu du baton
    public int scoreRecoBaton = 0;
    public int nbPartieBatonJoue = 0;
    public int nbPartieBaton = 4;
    public bool gameBatonFait = false;

    // variables pour jeu du clou
    public int scoreRecoClou = 0;
    public int nbPartieClouJoue = 0;
    public int nbPartieClou = 3;
    public bool gameClouFait = false;

    //variables pour les dés afin de déterminer qui commence
    public bool checkFaitDesMj = true;
    public bool checkFaitDesPlayer = true;
    public int scoreDesMj;
    public int scoreDesPlayer;
    public string quiCommence;
    //--------------------------------
    public string jeuEnCours;

    // Définir un événement pour signaler les mises à jour du score
    public delegate void ScoreUpdated(int newScoreBaton, int newScoreClou);
    public static event ScoreUpdated OnScoreUpdated;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        checkFaitDesMj = true;
        checkFaitDesPlayer = true;
        // Trouver le GameObject avec le nom "scoreTextReco" au démarrage
        FindScoreTextObject();
        UpdateScoreText();
    }

    private void Update()
    {
        // Vérifier périodiquement si le composant TextMeshProUGUI a été trouvé
        if (scoreTextReco == null)
        {
            FindScoreTextObject();
        }

        // Mettre à jour le texte si le composant a été trouvé
        UpdateScoreText();
    }

    private void FindScoreTextObject()
    {
        GameObject scoreTextObject = GameObject.Find("scoreTextReco");
        if (scoreTextObject != null)
        {
            // Récupérer le composant TextMeshProUGUI du GameObject trouvé
            scoreTextReco = scoreTextObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("GameObject avec le nom 'scoreTextReco' non trouvé.");
        }
    }

    private void UpdateScoreText()
    {
        if (scoreTextReco != null)
        {
            
            scoreTextReco.text = scoreReco.ToString() + "/17";
        }
        else
        {
            Debug.LogError("TextMeshProUGUI non trouvé. Assurez-vous que le GameObject 'scoreTextReco' a un composant TextMeshProUGUI.");
        }
    }

    // Méthode pour mettre à jour le score
    public void UpdateScore(int newScore)
    {
        //scoreReco = newScore;

        // Déclencher l'événement OnScoreUpdated
        OnScoreUpdated?.Invoke(scoreRecoBaton, scoreRecoClou);
    }
}
