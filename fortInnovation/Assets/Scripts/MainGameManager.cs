using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;


public class MainGameManager : MonoBehaviour
{
    public static MainGameManager Instance;
    private TextMeshProUGUI scoreTextReco;
    public int scoreReco = 0;

    // Variables de jeu
    public int nbPartiePaires;
    public int nbPartieBaton;
    public int nbPartieClou;
    public int nbPartieBassin;
    public int nbPartieEnigmes;
    public int scoreRecoPaires;
    public int nbPartiePairesJoue;
    public bool gamePairesFait;
    public List<int> questionsPairesPosees = new List<int>();

    public int scoreRecoBaton;
    public int nbPartieBatonJoue;
    public bool gameBatonFait;
    public List<int> questionsBatonPosees = new List<int>();

    public int scoreRecoClou;
    public int nbPartieClouJoue;
    public bool gameClouFait;
    public List<int> questionsClouPosees = new List<int>();

    public int scoreRecobassin;
    public int nbPartieBassinJoue;
    public bool gameBassinFait;
    public List<int> questionsBassinPosees = new List<int>();

    public int scoreRecoEnigmes;
    public int nbPartieEnigmesJoue;
    public bool gameEnigmesFait;
    public List<int> questionsEnigmesPosees = new List<int>();

    public bool checkFaitDesMj;
    public bool checkFaitDesPlayer;
    public int scoreDesMj;
    public int scoreDesPlayer;
    public string quiCommence;
    public string jeuEnCours;
    public string cinematiqueEnCours;
    public int selectedCharacter;
    public bool panelUiMobile;
    public int tutoCompteur;
    public string nbRecoMax;

    // Dialogues
    public List<string> dialogueSallePaires = new List<string>();
    public List<string> dialogueSalleBaton = new List<string>();
    public List<string> dialogueSalleClou = new List<string>();
    public List<string> dialogueSalleBassin = new List<string>();
    public List<string> dialogueSalleEnigmes = new List<string>();
    public List<string> dialogueSalleIntroduction = new List<string>();
    public List<string> dialogueSalleAccueil = new List<string>();
    public List<string> dialogueSalleFin = new List<string>();
    public List<string> dialogueJeuPaires = new List<string>();
    public List<string> dialogueJeuBaton = new List<string>();
    public List<string> dialogueJeuClou = new List<string>();
    public List<string> dialogueJeuBassin = new List<string>();
    public List<string> dialogueJeuEnigme = new List<string>();

    // Définir un événement pour signaler les mises à jour du score
    public delegate void ScoreUpdated(int newScorePaires, int newScoreBaton, int newScoreClou, int newScoreBassin, int newScoreEnigmes);
    public static event ScoreUpdated OnScoreUpdated;
    public bool settingsLoaded = false;

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
            //Debug.LogError("GameObject avec le nom 'scoreTextReco' non trouvé.");
        }
    }

    private void UpdateScoreText()
    {
        if (scoreTextReco != null)
        {
            scoreTextReco.text = scoreReco.ToString() + nbRecoMax;
        }
        else
        {
            //Debug.LogError("TextMeshProUGUI non trouvé. Assurez-vous que le GameObject 'scoreTextReco' a un composant TextMeshProUGUI.");
        }
    }

    // Méthode pour mettre à jour le score
    public void UpdateScore(int newScore)
    {
        //scoreReco = newScore;

        // Déclencher l'événement OnScoreUpdated
        OnScoreUpdated?.Invoke(scoreRecoPaires, scoreRecoBaton, scoreRecoClou, scoreRecobassin, scoreRecoEnigmes);
    }

    public void ActivationUiMobile()
    {
        panelUiMobile = !panelUiMobile; // Basculer l'état de panelUiMobile
    }

    public IEnumerator LoadSettings(string difficulty)
    {
            yield return StartCoroutine(LoadSettingsCoroutine(difficulty));
            settingsLoaded = true;
    }

    private IEnumerator LoadSettingsCoroutine(string difficulty)
    {
        string path = Path.Combine(Application.streamingAssetsPath, difficulty);

    #if UNITY_WEBGL && !UNITY_EDITOR
        path = Application.streamingAssetsPath + "/" + difficulty;
    #endif

        UnityWebRequest request = UnityWebRequest.Get(path);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            LoadJson(json);
        }
        else
        {
            Debug.LogError("Failed to load settings file: " + request.error);
        }
    }


    private void LoadJson(string json)
    {
        var settings = JsonUtility.FromJson<Settings>(json);

        // Assign loaded settings to respective variables
        nbPartiePaires = settings.reglagesJeux.nbPartiePaires;
        nbPartieBaton = settings.reglagesJeux.nbPartieBaton;
        nbPartieClou = settings.reglagesJeux.nbPartieClou;
        nbPartieBassin = settings.reglagesJeux.nbPartieBassin;
        nbPartieEnigmes = settings.reglagesJeux.nbPartieEnigmes;

        scoreRecoPaires = settings.reglagesJeux.scoreRecoPaires;
        nbPartiePairesJoue = settings.reglagesJeux.nbPartiePairesJoue;
        gamePairesFait = settings.reglagesJeux.gamePairesFait;
        questionsPairesPosees = settings.reglagesJeux.questionsPairesPosees;

        scoreRecoBaton = settings.reglagesJeux.scoreRecoBaton;
        nbPartieBatonJoue = settings.reglagesJeux.nbPartieBatonJoue;
        gameBatonFait = settings.reglagesJeux.gameBatonFait;
        questionsBatonPosees = settings.reglagesJeux.questionsBatonPosees;

        scoreRecoClou = settings.reglagesJeux.scoreRecoClou;
        nbPartieClouJoue = settings.reglagesJeux.nbPartieClouJoue;
        gameClouFait = settings.reglagesJeux.gameClouFait;
        questionsClouPosees = settings.reglagesJeux.questionsClouPosees;

        scoreRecobassin = settings.reglagesJeux.scoreRecobassin;
        nbPartieBassinJoue = settings.reglagesJeux.nbPartieBassinJoue;
        gameBassinFait = settings.reglagesJeux.gameBassinFait;
        questionsBassinPosees = settings.reglagesJeux.questionsBassinPosees;

        scoreRecoEnigmes = settings.reglagesJeux.scoreRecoEnigmes;
        nbPartieEnigmesJoue = settings.reglagesJeux.nbPartieEnigmesJoue;
        gameEnigmesFait = settings.reglagesJeux.gameEnigmesFait;
        questionsEnigmesPosees = settings.reglagesJeux.questionsEnigmesPosees;

        checkFaitDesMj = settings.reglagesJeux.checkFaitDesMj;
        checkFaitDesPlayer = settings.reglagesJeux.checkFaitDesPlayer;
        scoreDesMj = settings.reglagesJeux.scoreDesMj;
        scoreDesPlayer = settings.reglagesJeux.scoreDesPlayer;
        quiCommence = settings.reglagesJeux.quiCommence;

        jeuEnCours = settings.reglagesJeux.jeuEnCours;
        cinematiqueEnCours = settings.reglagesJeux.cinematiqueEnCours;

        selectedCharacter = settings.reglagesJeux.selectedCharacter;
        panelUiMobile = settings.reglagesJeux.panelUiMobile;
        tutoCompteur = settings.reglagesJeux.tutoCompteur;
        nbRecoMax = settings.reglagesJeux.nbRecoMax;

        dialogueSallePaires = settings.dialogueSallePaires;
        dialogueSalleBaton = settings.dialogueSalleBaton;
        dialogueSalleClou = settings.dialogueSalleClou;
        dialogueSalleBassin = settings.dialogueSalleBassin;
        dialogueSalleEnigmes = settings.dialogueSalleEnigmes;
        dialogueSalleIntroduction = settings.dialogueSalleIntroduction;
        dialogueSalleAccueil = settings.dialogueSalleAccueil;
        dialogueSalleFin = settings.dialogueSalleFin;
        dialogueJeuPaires = settings.dialogueJeuPaires;
        dialogueJeuBaton = settings.dialogueJeuBaton;
        dialogueJeuClou = settings.dialogueJeuClou;
        dialogueJeuBassin = settings.dialogueJeuBassin;
        dialogueJeuEnigme = settings.dialogueJeuEnigme;

        
       

    }

    [System.Serializable]
    public class ReglagesJeux
    {
        public int nbPartiePaires;
        public int nbPartieBaton;
        public int nbPartieClou;
        public int nbPartieBassin;
        public int nbPartieEnigmes;
        public int scoreRecoPaires;
        public int nbPartiePairesJoue;
        public bool gamePairesFait;
        public List<int> questionsPairesPosees;
        public int scoreRecoBaton;
        public int nbPartieBatonJoue;
        public bool gameBatonFait;
        public List<int> questionsBatonPosees;
        public int scoreRecoClou;
        public int nbPartieClouJoue;
        public bool gameClouFait;
        public List<int> questionsClouPosees;
        public int scoreRecobassin;
        public int nbPartieBassinJoue;
        public bool gameBassinFait;
        public List<int> questionsBassinPosees;
        public int scoreRecoEnigmes;
        public int nbPartieEnigmesJoue;
        public bool gameEnigmesFait;
        public List<int> questionsEnigmesPosees;
        public bool checkFaitDesMj;
        public bool checkFaitDesPlayer;
        public int scoreDesMj;
        public int scoreDesPlayer;
        public string quiCommence;
        public string jeuEnCours;
        public string cinematiqueEnCours;
        public int selectedCharacter;
        public bool panelUiMobile;
        public int tutoCompteur;
        public string nbRecoMax;
    }
[System.Serializable]
public class Settings
{
    public ReglagesJeux reglagesJeux;
    public List<string> dialogueSallePaires;
    public List<string> dialogueSalleBaton;
    public List<string> dialogueSalleClou;
    public List<string> dialogueSalleBassin;
    public List<string> dialogueSalleEnigmes;
    public List<string> dialogueSalleIntroduction;
    public List<string> dialogueSalleAccueil;
    public List<string> dialogueSalleFin;
    public List<string> dialogueJeuPaires;
    public List<string> dialogueJeuBaton;
    public List<string> dialogueJeuClou;
    public List<string> dialogueJeuBassin;
    public List<string> dialogueJeuEnigme;
}
}
