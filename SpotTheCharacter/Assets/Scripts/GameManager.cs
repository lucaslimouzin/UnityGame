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
    public TMP_Text levelText;

    public GameObject panelWin;

    private GameObject selectedSpoonyForUI;

    void Start()
    {
        panelWin.SetActive(false);
        string sceneName = SceneManager.GetActiveScene().name;

        // Supposer que le nom de la scène est toujours sous la forme "Level_XXX"
        if (sceneName.StartsWith("Level_"))
        {
            string levelNumberStr = sceneName.Substring("Level_".Length);
            if (int.TryParse(levelNumberStr, out int levelNumber))
            {
                levelText.text = "Level " + levelNumber; // Affiche "Level X" où X est le numéro de la scène
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

    void Win(){
        panelWin.SetActive(false);
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
