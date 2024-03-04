using UnityEngine;
using UnityEngine.Networking;
using TMPro; // Ajoutez cette ligne si vous utilisez TextMeshPro
using System.Collections.Generic;
using System;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using StarterAssets;


// Classe pour correspondre à la structure des données reçues de l'API
[Serializable]
public class ScoreEntry
{
    public int id;
    public string player_name;
    public int score;
    public string date_recorded;
}

[Serializable]
public class ScoresList
{
    public List<ScoreEntry> scores;
}

public class ScoreDisplay : MonoBehaviour
{
    public GameObject scorePrefab; // Référence au prefab Text ou TextMeshPro
    public Transform scoresParent; // Référence au parent sous le Canvas où les scores seront affichés
    public TMP_InputField playerNameInput; // Référence à l'InputField du nom du joueur
    public TextMeshProUGUI playerScoreText; // Référence à l'InputField du nom du joueur
    public int score = 0; 
    public Button submitButton; // Ajoutez cette ligne en haut de votre script
    private List<GameObject> currentScoreObjects = new List<GameObject>();
    private StarterAssets.ThirdPersonController thirdPersonController;

    // Start is called before the first frame update
    
    void Start()
    {
         // Trouver le script ThirdPersonController automatiquement au démarrage
        thirdPersonController = FindObjectOfType<StarterAssets.ThirdPersonController>();

        if (thirdPersonController == null)
        {
            Debug.LogError("ThirdPersonController not found in the scene.");
        }
        score = MainGameManager.Instance.scoreReco;
        playerScoreText.text = "Votre score est de " + score.ToString() + " / 17 recommandations";
        StartCoroutine(GetScores());
    }

    IEnumerator GetScores()
    {
        string apiUrl = "https://givrosgaming.fr/apinode/api/scores";
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            
           ScoresList scoresList = JsonUtility.FromJson<ScoresList>("{\"scores\":" + request.downloadHandler.text + "}");
            // S'assurer qu'on ne dépasse pas le nombre de scores disponibles ou 10
            int scoresCount = Mathf.Min(scoresList.scores.Count, 10);

            // Mettre à jour ou créer des objets de score
            for (int i = 0; i < scoresCount; i++)
            {
                GameObject scoreObject;

                // Réutiliser l'objet s'il existe déjà
                if (i < currentScoreObjects.Count)
                {
                    scoreObject = currentScoreObjects[i];
                }
                else
                {
                    // Créer un nouvel objet s'il n'y en a pas assez
                    scoreObject = Instantiate(scorePrefab, scoresParent);
                    currentScoreObjects.Add(scoreObject);
                }

                // Mettre à jour le texte du score
                scoreObject.GetComponent<TextMeshProUGUI>().text = $"Prénom : {scoresList.scores[i].player_name}, Score: {scoresList.scores[i].score}";
                scoreObject.SetActive(true); // S'assurer que l'objet est actif
            }
            // Désactiver les objets de score excédentaires s'il y en a
            for (int i = scoresCount; i < currentScoreObjects.Count; i++)
            {
                currentScoreObjects[i].SetActive(false);
            }
        }
    }

    // Méthode à appeler lorsque le bouton est pressé
    public void OnSubmitScore()
    {
        string playerName = playerNameInput.text; // Obtient le nom du joueur depuis l'InputField
        StartCoroutine(PostScore(playerName, score));
        playerNameInput.interactable = false;
        submitButton.interactable = false; // Désactiver le bouton     

    }

    IEnumerator PostScore(string playerName, int score)
    {
        string apiUrl = "https://givrosgaming.fr/apinode/api/scores";
        ScoreEntry newScore = new ScoreEntry { player_name = playerName, score = score };
        string jsonData = JsonUtility.ToJson(newScore);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("Score soumis avec succès : " + request.downloadHandler.text);
            // Ici, vous pouvez ajouter une logique pour récupérer et afficher à nouveau tous les scores
            // si votre API retourne les données mises à jour après une soumission
        }

        //recharge les données 
        StartCoroutine(GetScores());
    }

    public void DisableGameplayInput()
    {
        // Désactive les entrées de gameplay
        if (thirdPersonController != null)
        {
            thirdPersonController.enabled = false;
        }
    }

    public void EnableGameplayInput()
    {
        // Réactive les entrées de gameplay
        if (thirdPersonController != null)
        {
            thirdPersonController.enabled = true;
        }
    }

}

// Classe helper pour gérer le parsing de listes avec JsonUtility
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{\"array\":" + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}



   