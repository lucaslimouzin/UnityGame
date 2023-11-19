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
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI scoreText;
    private int score = 0;
    private int bestScore = 0;
    private bool isGamePaused = false;
    public GameObject gameOverPanel;

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
        // Mettre à jour le texte du score à chaque frame

        scoreText.text = "" + score.ToString();
        
    }

    public void GererCollisionObjet(GameObject objetEnCollision, GameObject objetCiblePrefab, int points)
    {
        // Incrémenter le nombre d'objets en collision
        objetsEnCours++;

        // Enregistrer les objets en collision
        if (objetsEnCours == 1)
        {
            premierObjetEnCollision = objetEnCollision;
        }
        else if (objetsEnCours == 2)
        {
            deuxiemeObjetEnCollision = objetEnCollision;

            // Récupérer la position du premier objet en collision
            Vector3 positionSpawn = premierObjetEnCollision.transform.position;
            // Ajouter les points au score
            score += points;

            // Détruire les deux objets en collision
            Destroy(premierObjetEnCollision);
            Destroy(deuxiemeObjetEnCollision);

            // Réinitialiser le nombre d'objets en collision
            objetsEnCours = 0;

            // Invoquer l'objet cible à la position du premier objet en collision
            SpawnObjetCible(objetCiblePrefab, positionSpawn);
        }
    }

    private void SpawnObjetCible(GameObject objetCiblePrefab, Vector3 positionSpawn)
    {
        // Spawner l'objet cible à la position du premier objet en collision
        Instantiate(objetCiblePrefab, positionSpawn, Quaternion.identity);
    }

    public void GameOver()
    {
        //gestion du meilleurs score
        if (score > bestScore)
        {
            bestScoreText.text = "" + score.ToString();
        }
        //gestion du score final 
        finalScoreText.text = "" + score.ToString();
        // Ajoutez le code pour afficher le panel et mettre le jeu en pause
        DisplayGameOverPanel();
        PauseGame();
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
        // Inverser l'état de la pause
        isGamePaused = !isGamePaused;

        // Mettre en pause ou reprendre le temps selon l'état actuel
        Time.timeScale = isGamePaused ? 0f : 1f;
    }

    public void RechargerNiveau()
    {
        // Obtenez l'index du niveau actuel
        int indexNiveauActuel = SceneManager.GetActiveScene().buildIndex;

        // Rechargez le niveau actuel
        SceneManager.LoadScene(indexNiveauActuel);
    }

    public void QuitterJeu()
    {
        // Quittez l'application
        Application.Quit();

        // Cette ligne ne sera atteinte que dans l'éditeur Unity
        // pour simuler l'arrêt du jeu dans l'éditeur
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
