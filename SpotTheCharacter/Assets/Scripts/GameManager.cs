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
    public GameObject panelWin;
    public GameObject panelSearch;
    public GameObject panelGameOver;

    private GameObject selectedSpoonyForUI;
    public Image star1;
    public Image star2;
    public Image star3;
    public bool identique = true;

    public Image star1Win;
    public Image star2Win;
    public Image star3Win;
    private int earnedStars;
    public TMP_Text wrongText; // Référence au TextMeshPro

    private int errorsCount = 0;
    public GameObject[] spawnPoints;
    private List<int> usedSpawnIndices = new List<int>();

    private int currentLevel;
    private PartToRandomize currentPartToRandomize;
    private PartToRandomize currentPart2ToRandomize;
    private PartToRandomize currentPart3ToRandomize;
    private PartToRandomize currentPart4ToRandomize;

    enum PartToRandomize
    {
        None,
        Face,
        Body,
        RightArm,
        LeftArm
    }
    Interstitial interstitial;

    void Start()
    {
        interstitial = GameObject.FindGameObjectWithTag("TagAds").GetComponent<Interstitial>();
        panelWin.SetActive(false);
        panelGameOver.SetActive(false);
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

        // Déterminer le niveau actuel
        currentLevel = GetCurrentLevelNumber();
        
        //détermine le nombre de partie du corps qui sera randomisé
        DetermineNumberOfPartieDuCorps();

        List<GameObject> spawnedSpoonies = new List<GameObject>();
        
        //création du spoony à trouver
        spawnedSpoonies.Add(SpawnSelectedSpoony());
        selectedSpoonyForUI = spawnedSpoonies[0];
        UpdateUI(selectedSpoonyForUI);


        //autres spoony
        int numberOfSpoonies = DetermineNumberOfSpoonies();       
        for (int i = 1; i < numberOfSpoonies; i++)
        {
            spawnedSpoonies.Add(SpawnSpoony(currentPartToRandomize, currentPart2ToRandomize, currentPart3ToRandomize, currentPart4ToRandomize));
        }
       
    }

       

    int DetermineNumberOfSpoonies()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Level_"))
        {
            if (int.TryParse(sceneName.Substring("Level_".Length), out int levelNumber))
            {
                // Calculez le nombre de Spoonies en fonction du niveau
                int numberOfSpoonies = Mathf.Clamp(3 + levelNumber / 5, 3, 21);
                return numberOfSpoonies;
            }
        }
        return 2; // Valeur par défaut si le nom de la scène ne correspond pas
    }

    void DetermineNumberOfPartieDuCorps(){
        // Déterminer la partie à randomiser en fonction du niveau
        if (currentLevel < 6)
        {
            currentPartToRandomize = PartToRandomize.Face;
            currentPart2ToRandomize = PartToRandomize.Body;
            currentPart3ToRandomize = PartToRandomize.LeftArm;
            currentPart4ToRandomize = PartToRandomize.RightArm;
        }
        
        else if (currentLevel < 12)
        {
            int randomPartie = Random.Range(1, 5); //entre 1 et 4
            switch (randomPartie)
            {
                case 1:
                    currentPartToRandomize = PartToRandomize.Face;
                    currentPart2ToRandomize = PartToRandomize.Body;
                    currentPart3ToRandomize = PartToRandomize.RightArm;
                    break;
                case 2:
                    currentPartToRandomize = PartToRandomize.Body;
                    currentPart2ToRandomize = PartToRandomize.Face;
                    currentPart3ToRandomize = PartToRandomize.LeftArm;
                    break;
                case 3:
                    currentPartToRandomize = PartToRandomize.RightArm;
                    currentPart2ToRandomize = PartToRandomize.LeftArm;
                    currentPart3ToRandomize = PartToRandomize.Face;
                    break;
                case 4:
                    currentPartToRandomize = PartToRandomize.LeftArm;
                    currentPart2ToRandomize = PartToRandomize.RightArm;
                    currentPart3ToRandomize = PartToRandomize.Body;
                    break;
                default:
                    // Gestion de cas par défaut si nécessaire
                    break;
            }
        }
        else if (currentLevel < 18)
        {
            int randomPartie = Random.Range(1, 7); //entre 1 et 6
            switch (randomPartie)
            {
                case 1:
                    currentPartToRandomize = PartToRandomize.Face;
                    currentPart2ToRandomize = PartToRandomize.Body;
                    break;
                case 2:
                    currentPartToRandomize = PartToRandomize.Face;
                    currentPart2ToRandomize = PartToRandomize.RightArm;
                    break;
                case 3:
                    currentPartToRandomize = PartToRandomize.Face;
                    currentPart2ToRandomize = PartToRandomize.LeftArm;
                    break;
                case 4:
                    currentPartToRandomize = PartToRandomize.Body;
                    currentPart2ToRandomize = PartToRandomize.RightArm;
                    break;
                case 5:
                    currentPartToRandomize = PartToRandomize.Body;
                    currentPart2ToRandomize = PartToRandomize.LeftArm;
                    break;
                case 6:
                    currentPartToRandomize = PartToRandomize.RightArm;
                    currentPart2ToRandomize = PartToRandomize.LeftArm;
                    break; 
                default:
                    // Gestion de cas par défaut si nécessaire
                    break;
            }
        }
        else if (currentLevel < 24)
        {
            int randomPartie = Random.Range(1, 5); //entre 1 et 4
            switch (randomPartie)
            {
                case 1:
                    currentPartToRandomize = PartToRandomize.Face;
                    break;
                case 2:
                    currentPartToRandomize = PartToRandomize.Body;
                    break;
                case 3:
                    currentPartToRandomize = PartToRandomize.RightArm;
                    break;
                case 4:
                    currentPartToRandomize = PartToRandomize.LeftArm;
                    break;
                default:
                    // Gestion de cas par défaut si nécessaire
                    break;
            }
        }
        else if (currentLevel < 30)
        {
            currentPartToRandomize = PartToRandomize.Face;
            currentPart2ToRandomize = PartToRandomize.Body;
            currentPart3ToRandomize = PartToRandomize.LeftArm;
            currentPart4ToRandomize = PartToRandomize.RightArm;
        }
        
        else if (currentLevel < 36)
        {
            int randomPartie = Random.Range(1, 5); //entre 1 et 4
            switch (randomPartie)
            {
                case 1:
                    currentPartToRandomize = PartToRandomize.Face;
                    currentPart2ToRandomize = PartToRandomize.Body;
                    currentPart3ToRandomize = PartToRandomize.RightArm;
                    break;
                case 2:
                    currentPartToRandomize = PartToRandomize.Body;
                    currentPart2ToRandomize = PartToRandomize.Face;
                    currentPart3ToRandomize = PartToRandomize.LeftArm;
                    break;
                case 3:
                    currentPartToRandomize = PartToRandomize.RightArm;
                    currentPart2ToRandomize = PartToRandomize.LeftArm;
                    currentPart3ToRandomize = PartToRandomize.Face;
                    break;
                case 4:
                    currentPartToRandomize = PartToRandomize.LeftArm;
                    currentPart2ToRandomize = PartToRandomize.RightArm;
                    currentPart3ToRandomize = PartToRandomize.Body;
                    break;
                default:
                    // Gestion de cas par défaut si nécessaire
                    break;
            }
        }
        else if (currentLevel < 42)
        {
            int randomPartie = Random.Range(1, 7); //entre 1 et 6
            switch (randomPartie)
            {
                case 1:
                    currentPartToRandomize = PartToRandomize.Face;
                    currentPart2ToRandomize = PartToRandomize.Body;
                    break;
                case 2:
                    currentPartToRandomize = PartToRandomize.Face;
                    currentPart2ToRandomize = PartToRandomize.RightArm;
                    break;
                case 3:
                    currentPartToRandomize = PartToRandomize.Face;
                    currentPart2ToRandomize = PartToRandomize.LeftArm;
                    break;
                case 4:
                    currentPartToRandomize = PartToRandomize.Body;
                    currentPart2ToRandomize = PartToRandomize.RightArm;
                    break;
                case 5:
                    currentPartToRandomize = PartToRandomize.Body;
                    currentPart2ToRandomize = PartToRandomize.LeftArm;
                    break;
                case 6:
                    currentPartToRandomize = PartToRandomize.RightArm;
                    currentPart2ToRandomize = PartToRandomize.LeftArm;
                    break; 
                default:
                    // Gestion de cas par défaut si nécessaire
                    break;
            }
        }
        
        else if (currentLevel < 48)
        {
            int randomPartie = Random.Range(1, 5); //entre 1 et 4
            switch (randomPartie)
            {
                case 1:
                    currentPartToRandomize = PartToRandomize.Face;
                    break;
                case 2:
                    currentPartToRandomize = PartToRandomize.Body;
                    break;
                case 3:
                    currentPartToRandomize = PartToRandomize.RightArm;
                    break;
                case 4:
                    currentPartToRandomize = PartToRandomize.LeftArm;
                    break;
                default:
                    // Gestion de cas par défaut si nécessaire
                    break;
            }
        }
        else if (currentLevel < 54)
        {
            currentPartToRandomize = PartToRandomize.Face;
            currentPart2ToRandomize = PartToRandomize.Body;
            currentPart3ToRandomize = PartToRandomize.LeftArm;
            currentPart4ToRandomize = PartToRandomize.RightArm;
        }
        
        else if (currentLevel < 60)
        {
            int randomPartie = Random.Range(1, 5); //entre 1 et 4
            switch (randomPartie)
            {
                case 1:
                    currentPartToRandomize = PartToRandomize.Face;
                    currentPart2ToRandomize = PartToRandomize.Body;
                    currentPart3ToRandomize = PartToRandomize.RightArm;
                    break;
                case 2:
                    currentPartToRandomize = PartToRandomize.Body;
                    currentPart2ToRandomize = PartToRandomize.Face;
                    currentPart3ToRandomize = PartToRandomize.LeftArm;
                    break;
                case 3:
                    currentPartToRandomize = PartToRandomize.RightArm;
                    currentPart2ToRandomize = PartToRandomize.LeftArm;
                    currentPart3ToRandomize = PartToRandomize.Face;
                    break;
                case 4:
                    currentPartToRandomize = PartToRandomize.LeftArm;
                    currentPart2ToRandomize = PartToRandomize.RightArm;
                    currentPart3ToRandomize = PartToRandomize.Body;
                    break;
                default:
                    // Gestion de cas par défaut si nécessaire
                    break;
            }
        }
        else if (currentLevel < 66)
        {
            int randomPartie = Random.Range(1, 7); //entre 1 et 6
            switch (randomPartie)
            {
                case 1:
                    currentPartToRandomize = PartToRandomize.Face;
                    currentPart2ToRandomize = PartToRandomize.Body;
                    break;
                case 2:
                    currentPartToRandomize = PartToRandomize.Face;
                    currentPart2ToRandomize = PartToRandomize.RightArm;
                    break;
                case 3:
                    currentPartToRandomize = PartToRandomize.Face;
                    currentPart2ToRandomize = PartToRandomize.LeftArm;
                    break;
                case 4:
                    currentPartToRandomize = PartToRandomize.Body;
                    currentPart2ToRandomize = PartToRandomize.RightArm;
                    break;
                case 5:
                    currentPartToRandomize = PartToRandomize.Body;
                    currentPart2ToRandomize = PartToRandomize.LeftArm;
                    break;
                case 6:
                    currentPartToRandomize = PartToRandomize.RightArm;
                    currentPart2ToRandomize = PartToRandomize.LeftArm;
                    break; 
                default:
                    // Gestion de cas par défaut si nécessaire
                    break;
            }
        }
        else if (currentLevel < 72)
        {
            int randomPartie = Random.Range(1, 5); //entre 1 et 4
            switch (randomPartie)
            {
                case 1:
                    currentPartToRandomize = PartToRandomize.Face;
                    break;
                case 2:
                    currentPartToRandomize = PartToRandomize.Body;
                    break;
                case 3:
                    currentPartToRandomize = PartToRandomize.RightArm;
                    break;
                case 4:
                    currentPartToRandomize = PartToRandomize.LeftArm;
                    break;
                default:
                    // Gestion de cas par défaut si nécessaire
                    break;
            }
        }
        else if (currentLevel < 78)
        {
            currentPartToRandomize = PartToRandomize.Face;
            currentPart2ToRandomize = PartToRandomize.Body;
            currentPart3ToRandomize = PartToRandomize.LeftArm;
            currentPart4ToRandomize = PartToRandomize.RightArm;
        }
        
        else if (currentLevel < 84)
        {
            int randomPartie = Random.Range(1, 5); //entre 1 et 4
            switch (randomPartie)
            {
                case 1:
                    currentPartToRandomize = PartToRandomize.Face;
                    currentPart2ToRandomize = PartToRandomize.Body;
                    currentPart3ToRandomize = PartToRandomize.RightArm;
                    break;
                case 2:
                    currentPartToRandomize = PartToRandomize.Body;
                    currentPart2ToRandomize = PartToRandomize.Face;
                    currentPart3ToRandomize = PartToRandomize.LeftArm;
                    break;
                case 3:
                    currentPartToRandomize = PartToRandomize.RightArm;
                    currentPart2ToRandomize = PartToRandomize.LeftArm;
                    currentPart3ToRandomize = PartToRandomize.Face;
                    break;
                case 4:
                    currentPartToRandomize = PartToRandomize.LeftArm;
                    currentPart2ToRandomize = PartToRandomize.RightArm;
                    currentPart3ToRandomize = PartToRandomize.Body;
                    break;
                default:
                    // Gestion de cas par défaut si nécessaire
                    break;
            }
        }
        else if (currentLevel < 90)
        {
            int randomPartie = Random.Range(1, 7); //entre 1 et 6
            switch (randomPartie)
            {
                case 1:
                    currentPartToRandomize = PartToRandomize.Face;
                    currentPart2ToRandomize = PartToRandomize.Body;
                    break;
                case 2:
                    currentPartToRandomize = PartToRandomize.Face;
                    currentPart2ToRandomize = PartToRandomize.RightArm;
                    break;
                case 3:
                    currentPartToRandomize = PartToRandomize.Face;
                    currentPart2ToRandomize = PartToRandomize.LeftArm;
                    break;
                case 4:
                    currentPartToRandomize = PartToRandomize.Body;
                    currentPart2ToRandomize = PartToRandomize.RightArm;
                    break;
                case 5:
                    currentPartToRandomize = PartToRandomize.Body;
                    currentPart2ToRandomize = PartToRandomize.LeftArm;
                    break;
                case 6:
                    currentPartToRandomize = PartToRandomize.RightArm;
                    currentPart2ToRandomize = PartToRandomize.LeftArm;
                    break; 
                default:
                    // Gestion de cas par défaut si nécessaire
                    break;
            }
        }
        else
        {
            int randomPartie = Random.Range(1, 5); //entre 1 et 4
            switch (randomPartie)
            {
                case 1:
                    currentPartToRandomize = PartToRandomize.Face;
                    break;
                case 2:
                    currentPartToRandomize = PartToRandomize.Body;
                    break;
                case 3:
                    currentPartToRandomize = PartToRandomize.RightArm;
                    break;
                case 4:
                    currentPartToRandomize = PartToRandomize.LeftArm;
                    break;
                default:
                    // Gestion de cas par défaut si nécessaire
                    break;
            }
        }

    }
    GameObject SpawnSpoony(PartToRandomize partToRandomize,PartToRandomize part2ToRandomize, PartToRandomize part3ToRandomize, PartToRandomize part4ToRandomize)
    {
        // Vérifier combien de points de spawn sont disponibles
        if (usedSpawnIndices.Count == spawnPoints.Length)
        {
            // Tous les points de spawn ont été utilisés, vous pouvez choisir de gérer cela comme vous le souhaitez
            // Par exemple, réinitialiser la liste des indices utilisés
            usedSpawnIndices.Clear();
        }

        // Sélectionnez un spawnPoint aléatoire qui n'a pas été utilisé précédemment
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, spawnPoints.Length);
        }
        while (usedSpawnIndices.Contains(randomIndex));

        // Ajoutez l'index utilisé à la liste des indices utilisés
        usedSpawnIndices.Add(randomIndex);

        // Obtenez le GameObject du spawnPoint sélectionné
        GameObject spawnPoint = spawnPoints[randomIndex];

        // Obtenez la position du spawnPoint sélectionné
        Vector3 spawnPosition = spawnPoint.transform.position;

        // Instancier le prefab Spoony à la position du spawnPoint
        GameObject newSpoony = Instantiate(selectedSpoonyForUI, spawnPosition, Quaternion.identity);

        identique = true;

        while (identique) 
        {
            if (currentLevel < 6)
            {
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (partToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 2 à randomiser
                switch (part2ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 3 à randomiser
                switch (part3ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 4 à randomiser
                switch (part4ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
            }
            
            else if (currentLevel < 12) 
            {
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (partToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 2 à randomiser
                switch (part2ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 3 à randomiser
                switch (part3ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
            }
            else if (currentLevel < 18)
            {
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (partToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (part2ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
            } 
            else if(currentLevel <24)
            {
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (partToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
            }
            else if (currentLevel < 30)
            {
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (partToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 2 à randomiser
                switch (part2ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 3 à randomiser
                switch (part3ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 4 à randomiser
                switch (part4ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
            }
            
            else if (currentLevel < 36) 
            {
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (partToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 2 à randomiser
                switch (part2ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 3 à randomiser
                switch (part3ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
            }
            else if (currentLevel < 42)
            {
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (partToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (part2ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
            } 
            else if(currentLevel <48)
            {
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (partToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
            }
            else if (currentLevel < 54)
            {
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (partToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 2 à randomiser
                switch (part2ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 3 à randomiser
                switch (part3ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 4 à randomiser
                switch (part4ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
            }
            else if (currentLevel < 60) 
            {
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (partToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 2 à randomiser
                switch (part2ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 3 à randomiser
                switch (part3ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
            }
            else if (currentLevel < 66)
            {
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (partToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (part2ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
            } 
            else if(currentLevel <72)
            {
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (partToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
            }
            else if (currentLevel < 78)
            {
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (partToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 2 à randomiser
                switch (part2ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 3 à randomiser
                switch (part3ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 4 à randomiser
                switch (part4ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
            }
            
            else if (currentLevel < 84) 
            {
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (partToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 2 à randomiser
                switch (part2ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie 3 à randomiser
                switch (part3ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
            }
            else if (currentLevel < 90)
            {
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (partToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (part2ToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
            } 
            else
            {
                // Attribuer des sprites aléatoires, en fonction de la partie à randomiser
                switch (partToRandomize)
                {
                    case PartToRandomize.Face:
                        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
                        break;
                    case PartToRandomize.Body:
                        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
                        break;
                    case PartToRandomize.RightArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
                        break;
                    case PartToRandomize.LeftArm:
                        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);
                        break;
                    default:
                        break;
                }
            } 

            bool areEqual = AreSpritesEqual(selectedSpoonyForUI, newSpoony);

            if (areEqual)
            {
                // Les sprites des parties sont identiques
                identique = true;
                Debug.Log("il est identique");
            }
            else
            {
                // Les sprites des parties ne sont pas identiques
                identique = false;
                Debug.Log("il est différent");
                break;
            }
        }

        return newSpoony;
    }

    GameObject SpawnSelectedSpoony()
    {
        // Vérifier combien de points de spawn sont disponibles
        if (usedSpawnIndices.Count == spawnPoints.Length)
        {
            // Tous les points de spawn ont été utilisés, vous pouvez choisir de gérer cela comme vous le souhaitez
            // Par exemple, réinitialiser la liste des indices utilisés
            usedSpawnIndices.Clear();
        }

        // Sélectionnez un spawnPoint aléatoire qui n'a pas été utilisé précédemment
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, spawnPoints.Length);
        }
        while (usedSpawnIndices.Contains(randomIndex));

        // Ajoutez l'index utilisé à la liste des indices utilisés
        usedSpawnIndices.Add(randomIndex);

        // Obtenez le GameObject du spawnPoint sélectionné
        GameObject spawnPoint = spawnPoints[randomIndex];

        // Obtenez la position du spawnPoint sélectionné
        Vector3 spawnPosition = spawnPoint.transform.position;

        // Instancier le prefab Spoony à la position du spawnPoint
        GameObject newSpoony = Instantiate(spoonyPrefab, spawnPosition, Quaternion.identity);

        // Attribuer des sprites aléatoires
        AssignRandomSpriteForSelectedSpoony(newSpoony.transform.Find("Body").gameObject, bodySprites);
        AssignRandomSpriteForSelectedSpoony(newSpoony.transform.Find("Face").gameObject, faceSprites);
        AssignRandomSpriteForSelectedSpoony(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
        AssignRandomSpriteForSelectedSpoony(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);

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
    void AssignRandomSpriteForSelectedSpoony(GameObject obj, Sprite[] sprites)
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

    bool AreSpritesEqual(GameObject obj1, GameObject obj2)
    {
        
            // Obtenez les sprites des différentes parties
            Sprite bodySprite1 = obj1.transform.Find("Body").GetComponent<SpriteRenderer>().sprite;
            Sprite faceSprite1 = obj1.transform.Find("Face").GetComponent<SpriteRenderer>().sprite;
            Sprite rightArmSprite1 = obj1.transform.Find("BrasDroit").GetComponent<SpriteRenderer>().sprite;
            Sprite leftArmSprite1 = obj1.transform.Find("BrasGauche").GetComponent<SpriteRenderer>().sprite;

            Sprite bodySprite2 = obj2.transform.Find("Body").GetComponent<SpriteRenderer>().sprite;
            Sprite faceSprite2 = obj2.transform.Find("Face").GetComponent<SpriteRenderer>().sprite;
            Sprite rightArmSprite2 = obj2.transform.Find("BrasDroit").GetComponent<SpriteRenderer>().sprite;
            Sprite leftArmSprite2 = obj2.transform.Find("BrasGauche").GetComponent<SpriteRenderer>().sprite;

            // Comparez les sprites de chaque partie
            bool bodyEqual = bodySprite1 == bodySprite2;
            bool faceEqual = faceSprite1 == faceSprite2;
            bool rightArmEqual = rightArmSprite1 == rightArmSprite2;
            bool leftArmEqual = leftArmSprite1 == leftArmSprite2;

            // Si toutes les parties sont identiques, retournez true
            return bodyEqual && faceEqual && rightArmEqual && leftArmEqual;
        
    }



    public void CheckSpoonySelection(GameObject clickedSpoony)
    {
        if (clickedSpoony == selectedSpoonyForUI)
        {
            Win();
        }
        else
        {
            Loose();
        }
    }

    void Win()
    {
        if (errorsCount <1)
            {
                star1.enabled = star2.enabled = star3.enabled = star1Win.enabled = star2Win.enabled = star3Win.enabled = true;
                earnedStars = 3;
            }

        int levelNumber = GetCurrentLevelNumber();
        SaveStars(levelNumber, earnedStars);
        // Déverrouillez le niveau suivant
        UnlockNextLevel();
        // Activer le panneau de victoire
        panelWin.SetActive(true);
    }

    public void Loose()
    {
        errorsCount++;
        StartCoroutine(ShowAndHideWrongText());
        // Mettre à jour l'affichage des étoiles
        if (errorsCount == 1) // Plus de 1 min 30 sec
        {
            star1.enabled = star2.enabled = star1Win.enabled = star2Win.enabled = true;
            star3.enabled= star3Win.enabled = false;
            earnedStars = 2;
            
        }
        else if (errorsCount == 2) // Plus de 1 min
        {
            star1.enabled = star1Win.enabled = true;
            star2.enabled = star3.enabled = star2Win.enabled = star3Win.enabled = false;
            earnedStars = 1;
        }
        else if (errorsCount == 3) // Plus de 30 sec
        {
            star1.enabled = star2.enabled = star3.enabled = star1Win.enabled = star2Win.enabled = star3Win.enabled = false;
            earnedStars = 0;
        }
        if (errorsCount > 3)
        {
            panelGameOver.SetActive(true);
        }
    }

    IEnumerator ShowAndHideWrongText()
    {
        wrongText.gameObject.SetActive(true); // Affiche le texte
        yield return new WaitForSeconds(0.5f);  // Attend 1 seconde
        wrongText.gameObject.SetActive(false); // Masque le texte
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

    public void UnlockNextLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("Level_"))
        {
            if (int.TryParse(sceneName.Substring("Level_".Length), out int currentLevel))
            {
                PlayerPrefs.SetInt("Level_" + (currentLevel + 1) + "_Unlocked", 1);
                PlayerPrefs.Save();
            }
        }
    }

    public void PushStart (){
        panelSearch.SetActive(false);

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
        // Vérifiez si l'action doit être exécutée avec une chance sur 4
        if (ShouldShowInterstitialAd())
        {
            interstitial.ShowAd();
        }

        // Vérifiez si la scène suivante existe dans les paramètres de build
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Chargez la scène suivante
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene("Home");
            // Gérez le cas où il n'y a pas de niveau suivant, par exemple, affichez un écran de fin de jeu
        }
    }

    public void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private bool ShouldShowInterstitialAd()
    {
        // Génère un nombre aléatoire entre 1 et 4 (inclus)
        int randomValue = Random.Range(1, 5);
        Debug.Log(randomValue);
        // Si le nombre aléatoire est égal à 2, retourne true (1 chance sur 4)
        return randomValue == 2;
    }
}
