using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager Instance;
    private TextMeshProUGUI scoreTextReco;
    public int scoreReco = 0;
    public bool gameBatonFait = false;

    // Définir un événement pour signaler les mises à jour du score
    public delegate void ScoreUpdated(int newScore);
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
        scoreReco = newScore;

        // Déclencher l'événement OnScoreUpdated
        OnScoreUpdated?.Invoke(scoreReco);
    }
}
