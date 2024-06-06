using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestionCinematiques : MonoBehaviour
{
    public GameObject panelCinematiqueIntro;
    private AsyncOperation asyncOperation;

    // Start is called before the first frame update
    void Start() {
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
