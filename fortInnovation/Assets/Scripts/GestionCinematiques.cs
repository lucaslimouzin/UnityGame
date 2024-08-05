using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GestionCinematiques : MonoBehaviour
{
    public GameObject panelCinematiqueIntro;
    public GameObject imageCinematique;
    public TextMeshProUGUI textCinematique;
    private AsyncOperation asyncOperation;

    // Start is called before the first frame update
    void Start() {
        if(MainGameManager.Instance.niveauSelect =="Normal"){
                textCinematique.text = "Bienvenue dans Fort Innovation !\nDans ce jeu, vous allez découvrir 5 cellules représentant chacune un des grands principes de l'innovation participative.\nChaque principe est doté de bonne pratique et écueils à éviter, regroupés sous la forme de recommandations.\nPour obtenir ces recommandations, vous devrez relever les défis du Fort en affrontant les maîtres du jeu.\nÊtes-vous prêt à explorer, apprendre, et innover ?\nAlors entrez dès maintenant dans Fort Innovation, relevez les défis et devenez un expert\n de l'innovation participative!";         
        }
        else {
            textCinematique.text = "Bienvenue dans Fort Innovation !\nDans ce jeu, vous allez découvrir 5 cellules qui vous permettront d'en apprendre un peu plus sur l'innovation, en\ncommençant par les concepts de base de l'innovation, des exemples d'innovations historiques et contemporaines,\nla gestion de l'innovation et ces cycles de vie...\nÊtes-vous prêt à explorer, apprendre, et innover ?\nAlors entrez dès maintenant dans Fort Innovation, relevez les défis et devenez un expert de l'innovation !";        
        }
        // Recherchez le bouton par son nom
        GameObject buttonSuivant = GameObject.Find("ButtonSuivant");
        buttonSuivant.SetActive(false);
        // Commencez une coroutine pour attendre 3 secondes
        StartCoroutine(MakeButtonVisibleAfterDelay(buttonSuivant, 0.1f));
        panelCinematiqueIntro.SetActive(false);
        switch (MainGameManager.Instance.cinematiqueEnCours) {
            case "Introduction":
                panelCinematiqueIntro.SetActive(true);
                break;
        }

        // Précharger la scène suivante
        StartCoroutine(PreloadScene(MainGameManager.Instance.jeuEnCours));
    }

    private IEnumerator PreloadScene(string sceneName)
    {
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            // Optionally, you can display the loading progress here
            // Example: progressBar.fillAmount = asyncOperation.progress;
            yield return null;
        }
    }

    IEnumerator MakeButtonVisibleAfterDelay(GameObject button, float delay)
    {
        // Attendez le délai spécifié
        yield return new WaitForSeconds(delay);

        // Rendez le bouton visible
        button.SetActive(true);
    }

    public void PressSuivant() {
        // Redirige vers la map suivante
        if (asyncOperation != null && asyncOperation.isDone)
        {
            asyncOperation.allowSceneActivation = true;
        }
        else
        {
            SceneManager.LoadScene(MainGameManager.Instance.jeuEnCours);
        }
    }
}
