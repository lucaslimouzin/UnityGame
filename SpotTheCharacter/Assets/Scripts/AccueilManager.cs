using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Nécessaire pour accéder aux informations de la scène

[System.Serializable]
    public class LevelUI
    {
        public GameObject levelPanel; // Le panneau ou le bouton pour ce niveau
        public Image star1;
        public Image star2;
        public Image star3;
        public Image lockImage;
    }

public class AccueilManager : MonoBehaviour
{
    public Sprite[] bodySprites, faceSprites, brasDroitSprites, brasGaucheSprites;
    public Image uiCorps, uiFace, uiBrasDroit, uiBrasGauche; // Références aux éléments UI 
    public LevelUI[] levelsUI; // Tableau de tous les UI de niveau
    public GameObject panelLevels;
    void Start()
    {
        panelLevels.SetActive(false);
        StartCoroutine(ChangeSpritesCoroutine());
        // Définir le premier niveau comme déverrouillé par défaut
        UnlockFirstLevel();
        UpdateLevelStars();
        UpdateLevelLocks();
    }

    IEnumerator ChangeSpritesCoroutine()
    {
        while (true)
        {
            uiCorps.sprite = bodySprites[Random.Range(0, bodySprites.Length)];
            uiFace.sprite = faceSprites[Random.Range(0, faceSprites.Length)];
            uiBrasDroit.sprite = brasDroitSprites[Random.Range(0, brasDroitSprites.Length)];
            uiBrasGauche.sprite = brasGaucheSprites[Random.Range(0, brasGaucheSprites.Length)];

            yield return new WaitForSeconds(1f); // Attendre 1 secondes
        }
    }

    void UnlockFirstLevel()
{
    bool isUnlocked = true;
    // // Vérifiez si "Level_001_Unlocked" a déjà été défini, sinon définissez-le par défaut à 1 (déverrouillé)
    // if (!PlayerPrefs.HasKey("Level_001_Stars"))
    // {
    //     PlayerPrefs.SetInt("Level_001_Stars", 1);
        
    //     levelsUI[0].lockImage.enabled = isUnlocked;
    //     levelsUI[0].levelPanel.GetComponent<Button>().interactable = isUnlocked;
    //     PlayerPrefs.Save();
    // }
       
        levelsUI[0].lockImage.enabled = !isUnlocked;
        //levelsUI[0].levelPanel.GetComponent<Button>().interactable = isUnlocked;
}

    void UpdateLevelStars()
    {
        for (int i = 0; i < levelsUI.Length; i++)
        {
            int starsEarned = PlayerPrefs.GetInt("Level_" + (i + 1) + "_Stars", 0);
            levelsUI[i].star1.enabled = starsEarned >= 1;
            levelsUI[i].star2.enabled = starsEarned >= 2;
            levelsUI[i].star3.enabled = starsEarned >= 3;
        }
    }


    void UpdateLevelLocks()
    {
        for (int i = 1; i < levelsUI.Length; i++)
        {
            bool isUnlocked = PlayerPrefs.GetInt("Level_" + (i + 1) + "_Unlocked", i == 0 ? 1 : 0) == 1;
            levelsUI[i].lockImage.enabled = !isUnlocked;
            levelsUI[i].levelPanel.GetComponent<Button>().interactable = isUnlocked;
        }
    }

    public void PlayGame(){
        panelLevels.SetActive(true);
    }

    public void SelectLevel() {
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
