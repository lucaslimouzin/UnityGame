// GestionInteractions.cs
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GestionInteractions : MonoBehaviour
{
    public static GestionInteractions instance;

    private int objetsEnCours = 0;

    private GameObject premierObjetEnCollision;
    private GameObject deuxiemeObjetEnCollision;
    private Rigidbody2D objetRigidbody;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI scoreText;
    private int score = 0;
    private int bestScore = 0;
    private bool isGamePaused = false;
    public GameObject gameOverPanel;
    public AudioClip sonSpawn;
    Interstitial interstitial;

    private void Start()
    {
        interstitial = GameObject.FindGameObjectWithTag("TagAds").GetComponent<Interstitial>();
        bestScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        if (gameObject.layer == LayerMask.NameToLayer("ObjetSpawned"))
        {

            objetRigidbody = gameObject.GetComponent<Rigidbody2D>();
            objetRigidbody.bodyType = RigidbodyType2D.Dynamic;
            objetRigidbody.gravityScale = 1; 
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        // Mettre � jour le texte du score � chaque frame

        scoreText.text = "" + score.ToString();
        if (gameObject.layer == LayerMask.NameToLayer("ObjetSpawned"))
        {

            objetRigidbody = gameObject.GetComponent<Rigidbody2D>();
            objetRigidbody.bodyType = RigidbodyType2D.Dynamic;
            objetRigidbody.gravityScale = 1;
        }

    }

    public void GererCollisionObjet(GameObject objetEnCollision, GameObject objetCiblePrefab, int points)
    {
        // Incr�menter le nombre d'objets en collision
        objetsEnCours++;

        // Enregistrer les objets en collision
        if (objetsEnCours == 1)
        {
            premierObjetEnCollision = objetEnCollision;
        }
        else if (objetsEnCours == 2)
        {
            deuxiemeObjetEnCollision = objetEnCollision;

            // R�cup�rer la position du premier objet en collision
            Vector3 positionSpawn = premierObjetEnCollision.transform.position;
            // Ajouter les points au score
            score += points;

            // D�truire les deux objets en collision
            Destroy(premierObjetEnCollision);
            Destroy(deuxiemeObjetEnCollision);

            // R�initialiser le nombre d'objets en collision
            objetsEnCours = 0;

            // Invoquer l'objet cible � la position du premier objet en collision
            SpawnObjetCible(objetCiblePrefab, positionSpawn);
        }
    }

    private void SpawnObjetCible(GameObject objetCiblePrefab, Vector3 positionSpawn)
    {
        // r�cup�re composant AudioSource
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        // Configurer les param�tres audio
        audioSource.clip = sonSpawn;
        audioSource.playOnAwake = false; // Ne pas jouer automatiquement lors de la cr�ation
        audioSource.Play(); // Jouer le son maintenant
        // Spawner l'objet cible � la position du premier objet en collision
        Instantiate(objetCiblePrefab, positionSpawn, Quaternion.identity);
        objetRigidbody = objetCiblePrefab.GetComponent<Rigidbody2D>();
        if (objetRigidbody != null)
        {
            objetRigidbody.bodyType = RigidbodyType2D.Dynamic;
            // Changer la couche � "ObjetSpawned"
            objetRigidbody.gameObject.layer = LayerMask.NameToLayer("ObjetSpawned");
            objetRigidbody.gravityScale = 1;
        }
       
    }

    public void GameOver()
    {
        //gestion du meilleurs score
        if (score > bestScore)
        {
            bestScoreText.text = "" + score.ToString();
            PlayerPrefs.SetInt("HighScore", score);
        }
        //gestion du score final 
        finalScoreText.text = "" + score.ToString();
        // Ajoutez le code pour afficher le panel et mettre le jeu en pause
        DisplayGameOverPanel();
        //PauseGame();
    }

    private void DisplayGameOverPanel()
    {
        // Activez le composant Renderer de l'objet de game over pour l'afficher.
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    private void PauseGame()
    {
        // Inverser l'�tat de la pause
        isGamePaused = !isGamePaused;

        // Mettre en pause ou reprendre le temps selon l'�tat actuel
        Time.timeScale = isGamePaused ? 0f : 1f;
    }

    public void RechargerNiveau()
    {
        // V�rifiez si l'action doit �tre ex�cut�e avec une chance sur 4
        if (ShouldShowInterstitialAd())
        {
            interstitial.ShowAd();
        }
        // Obtenez l'index du niveau actuel
        int indexNiveauActuel = SceneManager.GetActiveScene().buildIndex;
        // Inverser l'�tat de la pause
        isGamePaused = false;
        // Mettre en pause ou reprendre le temps selon l'�tat actuel
        Time.timeScale = 1f;
        // Rechargez le niveau actuel
        SceneManager.LoadScene(indexNiveauActuel);
    }

    private bool ShouldShowInterstitialAd()
    {
        // G�n�re un nombre al�atoire entre 1 et 3 (inclus)
        int randomValue = Random.Range(1, 4);

        // Si le nombre al�atoire est �gal � 2, retourne true (1 chance sur 3)
        return randomValue == 2;
    }

    public void QuitterJeu()
    {
        // Quittez l'application
        Application.Quit();

        // Cette ligne ne sera atteinte que dans l'�diteur Unity
        // pour simuler l'arr�t du jeu dans l'�diteur
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
