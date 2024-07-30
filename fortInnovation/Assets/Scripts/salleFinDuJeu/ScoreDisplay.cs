using UnityEngine;
using UnityEngine.Networking;
using TMPro; // Ajoutez cette ligne si vous utilisez TextMeshPro
using System.Collections.Generic;
using System;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using StarterAssets;
using UnityEngine.SceneManagement;


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
    public Image imageScore;

    public TextMeshProUGUI texteNiveau;
    public GameObject scorePrefab; // Référence au prefab Text ou TextMeshPro
    public Transform scoresParent; // Référence au parent sous le Canvas où les scores seront affichés
    public TMP_InputField playerNameInput; // Référence à l'InputField du nom du joueur
    public TextMeshProUGUI playerScoreText; 
    public TextMeshProUGUI playerScoreTextModeSimple; 
    public int score = 0; 
    public Button submitButton; // Ajoutez cette ligne en haut de votre script
    private List<GameObject> currentScoreObjects = new List<GameObject>();
    private StarterAssets.ThirdPersonController thirdPersonController;
    public Button openUrlButton; // Référence au bouton pour ouvrir l'URL
    public string url = "https://givrosgaming.fr/fortInnov/principes_participative.pdf"; // URL de destination
    // Start is called before the first frame update
    
    void Start()
    {
        //ajout v2
         if(MainGameManager.Instance.niveauSelect =="Normal"){
            imageScore.sprite= MainGameManager.Instance.imageScore[0];
        }else{
            imageScore.sprite= MainGameManager.Instance.imageScore[1];
        }
         // Trouver le script ThirdPersonController automatiquement au démarrage
        thirdPersonController = FindObjectOfType<StarterAssets.ThirdPersonController>();

        if (thirdPersonController == null)
        {
            Debug.LogError("ThirdPersonController not found in the scene.");
        }
        score = MainGameManager.Instance.scoreReco;
        //ajout v2
        if(MainGameManager.Instance.niveauSelect =="Normal"){
            playerScoreText.text = "Votre avez remporté " + score.ToString() + " / 17\nrecommandations.";
        }else{
            playerScoreTextModeSimple.text = "Votre avez remporté " + score.ToString() + " / 5\nduels.";
            
            switch (score){
                case <3:
                    texteNiveau.text = "<u>Niveau Débutant :</u> Tu as compris les concepts de base de l’innovation et tu es familiarisé avec quelques exemples d’innovations historiques et contemporaines. Recommence le jeu pour atteindre un niveau supérieur.";
                break;
                case <5:
                    texteNiveau.text = "<u>Niveau Intermédiaire :</u> Les concepts de base sont maitrisés et tu as une bonne connaissance des approches et outils en innovation. Tu as compris la gestion de l'innovation et ces cycles de vie. Tu as déjà atteint un très haut niveau, encore quelques efforts est tu seras un expert en innovation.";
                break;
                case >4:
                    texteNiveau.text = "<u>Niveau Expert :</u> Félicitation, te voilà expert en innovation. Maintenant que tu as une connaissance approfondie des bases de l’innovation, découvre une approche de l’innovation fondée sur le collectif : l’innovation participative.";
                break;
            }

        }
                
        // Ajoutez le gestionnaire de clics pour le texte avec lien
        playerScoreText.gameObject.AddComponent<LinkHandler>();
        //StartCoroutine(GetScores());
        // Configurez le bouton pour ouvrir l'URL
        if (openUrlButton != null)
        {
            openUrlButton.onClick.AddListener(OpenUrl);
        }
        else
        {
            Debug.LogError("OpenUrlButton is not assigned in the inspector.");
        }
    }
   public  void OpenUrl()
    {
        Application.OpenURL(url);
    }
    public void recommencerGame(){

    // variables pour jeu des paires
    MainGameManager.Instance.scoreRecoPaires = 0;
    MainGameManager.Instance.nbPartiePairesJoue = 0;
    MainGameManager.Instance.nbPartiePaires = 3;
    MainGameManager.Instance.gamePairesFait = false;
    // variables pour jeu des batons
    MainGameManager.Instance.scoreRecoBaton = 0;
    MainGameManager.Instance.nbPartieBatonJoue = 0;
    MainGameManager.Instance.nbPartieBaton = 4;
    MainGameManager.Instance.gameBatonFait = false;

    // variables pour jeu du clou
    MainGameManager.Instance.scoreRecoClou = 0;
    MainGameManager.Instance.nbPartieClouJoue = 0;
    MainGameManager.Instance.nbPartieClou = 3;
    MainGameManager.Instance.gameClouFait = false;

    // variables pour jeu du Bassin
    MainGameManager.Instance.scoreRecobassin = 0;
    MainGameManager.Instance.nbPartieBassinJoue = 0;
    MainGameManager.Instance.nbPartieBassin = 3;
    MainGameManager.Instance.gameBassinFait = false;

    // variables pour jeu des Enigmes
    MainGameManager.Instance.scoreRecoEnigmes = 0;
    MainGameManager.Instance.nbPartieEnigmesJoue = 0;
    MainGameManager.Instance.nbPartieEnigmes = 1;
    MainGameManager.Instance.gameEnigmesFait = false ;
    MainGameManager.Instance.scoreReco = 0;

    //retour vers accueil
    SceneManager.LoadScene("Accueil");

    }
    

    // IEnumerator GetScores()
    // {
    //     string apiUrl = "https://givrosgaming.fr/apinode/api/scores";
    //     UnityWebRequest request = UnityWebRequest.Get(apiUrl);
    //     yield return request.SendWebRequest();

    //     if (request.result != UnityWebRequest.Result.Success)
    //     {
    //         Debug.LogError(request.error);
    //     }
    //     else
    //     {
            
    //        ScoresList scoresList = JsonUtility.FromJson<ScoresList>("{\"scores\":" + request.downloadHandler.text + "}");
    //         // S'assurer qu'on ne dépasse pas le nombre de scores disponibles ou 10
    //         int scoresCount = Mathf.Min(scoresList.scores.Count, 10);

    //         // Mettre à jour ou créer des objets de score
    //         for (int i = 0; i < scoresCount; i++)
    //         {
    //             GameObject scoreObject;

    //             // Réutiliser l'objet s'il existe déjà
    //             if (i < currentScoreObjects.Count)
    //             {
    //                 scoreObject = currentScoreObjects[i];
    //             }
    //             else
    //             {
    //                 // Créer un nouvel objet s'il n'y en a pas assez
    //                 scoreObject = Instantiate(scorePrefab, scoresParent);
    //                 currentScoreObjects.Add(scoreObject);
    //             }

    //             // Mettre à jour le texte du score
    //             scoreObject.GetComponent<TextMeshProUGUI>().text = $"Prénom : {scoresList.scores[i].player_name}, Score: {scoresList.scores[i].score}";
    //             scoreObject.SetActive(true); // S'assurer que l'objet est actif
    //         }
    //         // Désactiver les objets de score excédentaires s'il y en a
    //         for (int i = scoresCount; i < currentScoreObjects.Count; i++)
    //         {
    //             currentScoreObjects[i].SetActive(false);
    //         }
    //     }
    // }

    // // Méthode à appeler lorsque le bouton est pressé
    // public void OnSubmitScore()
    // {
    //     string playerName = playerNameInput.text; // Obtient le nom du joueur depuis l'InputField
    //     StartCoroutine(PostScore(playerName, score));
    //     playerNameInput.interactable = false;
    //     submitButton.interactable = false; // Désactiver le bouton     

    // }

    // IEnumerator PostScore(string playerName, int score)
    // {
    //     string apiUrl = "https://givrosgaming.fr/apinode/api/scores";
    //     ScoreEntry newScore = new ScoreEntry { player_name = playerName, score = score };
    //     string jsonData = JsonUtility.ToJson(newScore);

    //     UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
    //     byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonData);
    //     request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
    //     request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    //     request.SetRequestHeader("Content-Type", "application/json");

    //     yield return request.SendWebRequest();

    //     if (request.result != UnityWebRequest.Result.Success)
    //     {
    //         Debug.LogError(request.error);
    //     }
    //     else
    //     {
    //         Debug.Log("Score soumis avec succès : " + request.downloadHandler.text);
    //         // Ici, vous pouvez ajouter une logique pour récupérer et afficher à nouveau tous les scores
    //         // si votre API retourne les données mises à jour après une soumission
    //     }

    //     //recharge les données 
    //     StartCoroutine(GetScores());
    // }

    // public void DisableGameplayInput()
    // {
    //     // Désactive les entrées de gameplay
    //     if (thirdPersonController != null)
    //     {
    //         thirdPersonController.enabled = false;
    //     }
    // }

    // public void EnableGameplayInput()
    // {
    //     // Réactive les entrées de gameplay
    //     if (thirdPersonController != null)
    //     {
    //         thirdPersonController.enabled = true;
    //     }
    // }

}

// // Classe helper pour gérer le parsing de listes avec JsonUtility
// public static class JsonHelper
// {
//     public static T[] FromJson<T>(string json)
//     {
//         string newJson = "{\"array\":" + json + "}";
//         Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
//         return wrapper.array;
//     }

//     [Serializable]
//     private class Wrapper<T>
//     {
//         public T[] array;
//     }
// }



   