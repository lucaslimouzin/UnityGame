using UnityEngine;
using UnityEngine.Networking;
using TMPro; // Ajoutez cette ligne si vous utilisez TextMeshPro
using System.Collections.Generic;
using System;
using System.Collections;


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
    // Start is called before the first frame update
    void Start()
    {
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
            foreach (ScoreEntry score in scoresList.scores)
            {
                GameObject scoreObject = Instantiate(scorePrefab, scoresParent);
                // Utilisez cette ligne si vous utilisez TextMeshPro
                scoreObject.GetComponent<TextMeshProUGUI>().text = $"Pseudo : {score.player_name}, Score: {score.score}";
                // Utilisez cette ligne si vous utilisez l'UI Text standard de Unity
                // scoreObject.GetComponent<Text>().text = $"Player: {score.player_name}, Score: {score.score}";
            }
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



   