using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Nécessaire pour accéder aux informations de la scène
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject spoonyPrefab;
    public Sprite[] bodySprites, faceSprites, brasDroitSprites, brasGaucheSprites;
    public Image uiCorps, uiFace, uiBrasDroit, uiBrasGauche; // Références aux éléments UI 
    public Image uiCorpsSearch, uiFaceSearch, uiBrasDroitSearch, uiBrasGaucheSearch; // Références aux éléments UI Search
    public TMP_Text levelText;
    public TMP_Text levelTextSearch;
    public TMP_Text timerText; // Référence au composant TextMeshProUGUI pour le timer
    public TMP_Text timerWin; // Référence au composant TextMeshProUGUI pour afficher le temps de victoire
    private float timeRemaining;
    private bool timerRunning = false;
    private float timeAtStart = 180f; // Durée initiale du timer, par exemple 60 secondes
    public GameObject panelWin;
    public GameObject panelSearch;

    private GameObject selectedSpoonyForUI;
    public Image star1;
    public Image star2;
    public Image star3;

    public Image star1Win;
    public Image star2Win;
    public Image star3Win;
    private int earnedStars;

    void Start()
    {
        panelWin.SetActive(false);
        panelSearch.SetActive(true);
        string sceneName = SceneManager.GetActiveScene().name;

        // Supposer que le nom de la scène est toujours sous la forme "Level_XXX"
        if (sceneName.StartsWith("Level_"))
        {
            string levelNumberStr = sceneName.Substring("Level_".Length);
            if (int.TryParse(levelNumberStr, out int levelNumber))
            {
                levelText.text = "Level " + levelNumber; // Affiche "Level X" où X est le numéro de la scène
                levelTextSearch.text = "Level " + levelNumber; // Affiche "Level X" où X est le numéro de la scène
            }
        }

        int numberOfSpoonies = DetermineNumberOfSpoonies();
        List<GameObject> spawnedSpoonies = new List<GameObject>();

        for (int i = 0; i < numberOfSpoonies; i++)
        {
            spawnedSpoonies.Add(SpawnSpoony());
        }

        // Sélectionnez aléatoirement l'un des Spoonies
        selectedSpoonyForUI = spawnedSpoonies[Random.Range(0, spawnedSpoonies.Count)];
        UpdateUI(selectedSpoonyForUI);

    }

    IEnumerator TimerCoroutine()
    {
        while (timerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();
                yield return null;
            }
            else
            {
                Debug.Log("Temps écoulé!");
                timerRunning = false;
                // Vous pouvez ajouter ici d'autres actions à exécuter lorsque le temps est écoulé
            }
        }
    }
    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        float timeElapsed = timeAtStart - timeRemaining;

        // Mettre à jour l'affichage des étoiles
        if (timeElapsed > 90f) // Plus de 1 min 30 sec
        {
            star1.enabled = star2.enabled = star3.enabled = star1Win.enabled = star2Win.enabled = star3Win.enabled = false;
            earnedStars = 0;
        }
        else if (timeElapsed > 60f) // Plus de 1 min
        {
            star1.enabled = star1Win.enabled = true;
            star2.enabled = star3.enabled = star2Win.enabled = star3Win.enabled = false;
            earnedStars = 1;
        }
        else if (timeElapsed > 30f) // Plus de 30 sec
        {
            star1.enabled = star2.enabled = star1Win.enabled = star2Win.enabled = true;
            star3.enabled= star3Win.enabled = false;
            earnedStars = 2;
        }
        else // Moins de 30 sec
        {
            star1.enabled = star2.enabled = star3.enabled = star1Win.enabled = star2Win.enabled = star3Win.enabled = true;
            earnedStars = 3;
        }
    }

    int DetermineNumberOfSpoonies()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Level_"))
        {
            if (int.TryParse(sceneName.Substring("Level_".Length), out int levelNumber))
            {
                return levelNumber + 1; // Par exemple Level_001 pour 2 Spoonies
            }
        }
        return 2; // Valeur par défaut si le nom de la scène ne correspond pas
    }

    GameObject SpawnSpoony()
    {
        // Définir une position aléatoire basée sur les dimensions données
        float randomX = Random.Range(-1.21f, 1.21f);
        float randomY = Random.Range(-2.11f, 2.75f);
        Vector3 randomPosition = new Vector3(randomX, randomY, 0);

        // Instancier le prefab Spoony
        GameObject newSpoony = Instantiate(spoonyPrefab, randomPosition, Quaternion.identity);

        // Attribuer des sprites aléatoires
        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);

        return newSpoony;
    }

    void AssignRandomSprite(GameObject obj, Sprite[] sprites)
    {
        if (sprites.Length > 0)
        {
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
            }
        }
    }

    void UpdateUI(GameObject spoony)
    {
        // Mettre à jour chaque élément d'UI avec les sprites correspondants du Spoony
        uiCorps.sprite = spoony.transform.Find("Body").GetComponent<SpriteRenderer>().sprite;
        uiFace.sprite = spoony.transform.Find("Face").GetComponent<SpriteRenderer>().sprite;
        uiBrasDroit.sprite = spoony.transform.Find("BrasDroit").GetComponent<SpriteRenderer>().sprite;
        uiBrasGauche.sprite = spoony.transform.Find("BrasGauche").GetComponent<SpriteRenderer>().sprite;
        // Mettre à jour chaque élément d'UI Searchavec les sprites correspondants du Spoony
        uiCorpsSearch.sprite = spoony.transform.Find("Body").GetComponent<SpriteRenderer>().sprite;
        uiFaceSearch.sprite = spoony.transform.Find("Face").GetComponent<SpriteRenderer>().sprite;
        uiBrasDroitSearch.sprite = spoony.transform.Find("BrasDroit").GetComponent<SpriteRenderer>().sprite;
        uiBrasGaucheSearch.sprite = spoony.transform.Find("BrasGauche").GetComponent<SpriteRenderer>().sprite;
    }

    public void CheckSpoonySelection(GameObject clickedSpoony)
    {
        if (clickedSpoony == selectedSpoonyForUI)
        {
            Win();
        }
        else
        {
            Debug.Log("Lose");
        }
    }

    void Win()
    {
        // Arrêter le timer
        timerRunning = false;

        // Mettre à jour le texte de victoire avec le temps actuel
        timerWin.text = timerText.text;

        int levelNumber = GetCurrentLevelNumber();
        SaveStars(levelNumber, earnedStars);
        // Activer le panneau de victoire
        panelWin.SetActive(true);
    }

    int GetCurrentLevelNumber()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Level_"))
        {
            if (int.TryParse(sceneName.Substring("Level_".Length), out int levelNumber))
            {
                return levelNumber;
            }
        }
        return 0; // Retourne 0 si le numéro de niveau ne peut pas être déterminé
    }

    void SaveStars(int levelNumber, int starsEarned)
    {
        string key = "Level_" + levelNumber + "_Stars";
        int currentStars = PlayerPrefs.GetInt(key, 0);
        if (starsEarned > currentStars)
        {
            PlayerPrefs.SetInt(key, starsEarned);
            PlayerPrefs.Save();
        }
    }

    int LoadStars(int levelNumber)
    {
        string key = "Level_" + levelNumber + "_Stars";
        return PlayerPrefs.GetInt(key, 0);
    }

    public void PushStart (){
        panelSearch.SetActive(false);
        // Configurez et démarrez votre timer ici
        timeRemaining = 180f; // Par exemple, pour un timer de 180 secondes
        timerRunning = true;
        StartCoroutine(TimerCoroutine());
    }

    public void ReturnHome() {
        // Chargez la scène suivante
            SceneManager.LoadScene("Home");
    }
    public void NextLevel()
    {
        // Obtenez l'index de la scène actuelle
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // Calculez l'index de la scène suivante
        int nextSceneIndex = currentSceneIndex + 1;

        // Vérifiez si la scène suivante existe dans les paramètres de build
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Chargez la scène suivante
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Il n'y a pas d'autres niveaux à charger.");
            // Gérez le cas où il n'y a pas de niveau suivant, par exemple, affichez un écran de fin de jeu
        }
    }
}
