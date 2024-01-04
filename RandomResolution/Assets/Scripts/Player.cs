using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Ajoutez cette directive pour TextMeshPro
using System.IO; // Nécessaire pour lire les fichiers
using UnityEngine.Networking;

[System.Serializable]
public class MessageData
{
    public string[] voeux_2024;
}

public class Player : MonoBehaviour
{
    public GameObject Face, Corps, MainGauche, MainDroite;
    public Sprite[] spriteFace, spriteCorps, spriteMainGauche, spriteMainDroite;
    public Button randomizeButton; // Référence au bouton

    private int currentSpriteIndexFace = 0;
    private int currentSpriteIndexCorps = 0;
    private int currentSpriteIndexMainGauche = 0;
    private int currentSpriteIndexMainDroite = 0;

    public GameObject panelCreate;
    public GameObject panelMessage;
    public TextMeshProUGUI messageText;
    public Toggle languageToggle; // Référence au Toggle


    // Start is called before the first frame update
    void Start()
    {
        // Attribuer un sprite aléatoire à chaque partie du corps au démarrage
        AssignRandomSprite(Face, spriteFace);
        AssignRandomSprite(Corps, spriteCorps);
        AssignRandomSprite(MainGauche, spriteMainGauche);
        AssignRandomSprite(MainDroite, spriteMainDroite);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Coroutine pour randomiser les sprites
    IEnumerator RandomizeCoroutine()
    {
        randomizeButton.interactable = false;

        float endTime = Time.time + 3f;
        while (Time.time < endTime)
        {
            AssignRandomSprite(Face, spriteFace);
            AssignRandomSprite(Corps, spriteCorps);
            AssignRandomSprite(MainGauche, spriteMainGauche);
            AssignRandomSprite(MainDroite, spriteMainDroite);

            yield return new WaitForSeconds(0.1f);
        }

        randomizeButton.interactable = true;
    }

    // Méthode publique pour démarrer la coroutine de randomisation
    public void RandomizeSprites()
    {
        StartCoroutine(RandomizeCoroutine());
    }

    // Attribuer un sprite aléatoire à l'objet
    void AssignRandomSprite(GameObject obj, Sprite[] sprites)
    {
        if (sprites.Length > 0)
        {
            int index = Random.Range(0, sprites.Length);
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = sprites[index];
            }
        }
    }

    // Méthodes pour changer le sprite du corps
    public void NextCorpsSprite()
    {
        ChangeSprite(ref currentSpriteIndexCorps, spriteCorps, Corps);
    }

    public void PreviousCorpsSprite()
    {
        ChangeSprite(ref currentSpriteIndexCorps, spriteCorps, Corps, false);
    }


    // Méthodes pour changer le sprite de la face
    public void NextFaceSprite()
    {
        ChangeSprite(ref currentSpriteIndexFace, spriteFace, Face);
    }

    public void PreviousFaceSprite()
    {
        ChangeSprite(ref currentSpriteIndexFace, spriteFace, Face, false);
    }

    // Méthodes pour changer le sprite de la main gauche
    public void NextMainGaucheSprite()
    {
        ChangeSprite(ref currentSpriteIndexMainGauche, spriteMainGauche, MainGauche);
    }

    public void PreviousMainGaucheSprite()
    {
        ChangeSprite(ref currentSpriteIndexMainGauche, spriteMainGauche, MainGauche, false);
    }

    // Méthodes pour changer le sprite de la main droite
    public void NextMainDroiteSprite()
    {
        ChangeSprite(ref currentSpriteIndexMainDroite, spriteMainDroite, MainDroite);
    }

    public void PreviousMainDroiteSprite()
    {
        ChangeSprite(ref currentSpriteIndexMainDroite, spriteMainDroite, MainDroite, false);
    }

    // Méthode générique pour changer le sprite
    void ChangeSprite(ref int currentIndex, Sprite[] sprites, GameObject obj, bool next = true)
    {
        if (next)
        {
            if (currentIndex < sprites.Length - 1)
                currentIndex++;
        }
        else
        {
            if (currentIndex > 0)
                currentIndex--;
        }

        UpdateSprite(obj, sprites[currentIndex]);
    }

    // Mettre à jour le sprite sur le GameObject
    void UpdateSprite(GameObject obj, Sprite newSprite)
    {
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        if (renderer != null)
            renderer.sprite = newSprite;
    }

    // Méthode pour activer le panel 2 et désactiver le panel 1
    public void ShowPanelMessage()
    {
        panelCreate.SetActive(false);
        panelMessage.SetActive(true);
        DisplayMessageFromJSON(languageToggle.isOn);
    }

    // Méthode pour activer le panel 1 et désactiver le panel 2
    public void ShowPanelCreate()
    {
        panelCreate.SetActive(true);
        panelMessage.SetActive(false);
    }

    // Coroutine pour charger le contenu JSON depuis une URL
    IEnumerator LoadJsonFromURL(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Envoyer la requête et attendre qu'elle soit terminée
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Erreur : " + webRequest.error);
            }
            else
            {
                // Traiter les données reçues
                string jsonContent = webRequest.downloadHandler.text;
                ProcessJsonData(jsonContent);
            }
        }
    }
    void DisplayMessageFromJSON(bool isFrench)
    {
        string url = isFrench ? "https://givrosgaming.fr/random-resolution2024/messageFr.json" : "https://givrosgaming.fr/random-resolution2024/messageEn.json";
        StartCoroutine(LoadJsonFromURL(url));
    }

    void ProcessJsonData(string jsonData)
    {
        MessageData loadedData = JsonUtility.FromJson<MessageData>(jsonData);
        string randomMessage = loadedData.voeux_2024[Random.Range(0, loadedData.voeux_2024.Length)];
        messageText.text = randomMessage;
    }
}
