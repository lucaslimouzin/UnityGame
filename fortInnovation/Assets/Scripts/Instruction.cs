using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Instruction : MonoBehaviour
{
    private AsyncOperation asyncOperation;
    public GameObject continueButton; // Assurez-vous de lier ce bouton dans l'inspecteur

    void Start()
    {
        continueButton.SetActive(false); // Masquer le bouton au démarrage
        StartCoroutine(PreloadScene("SalleInstructions"));
    }

    private IEnumerator PreloadScene(string sceneName)
    {
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            // Afficher le bouton lorsque la scène est complètement chargée (progress atteint 0.9f)
            if (asyncOperation.progress >= 0.9f)
            {
                continueButton.SetActive(true);
            }

            yield return null;
        }
    }

    public void PlayGame()
    {
        Debug.Log("j'ai appuyé");
        // Réduire l'opacité du bouton
        Image buttonImage = continueButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            Color color = buttonImage.color;
            color.a = 0.5f; // Réduire l'opacité à 50%
            buttonImage.color = color;
        }

        // Vérifier si la scène est prête à être activée (progress atteint 0.9f)
        if (asyncOperation != null && asyncOperation.progress >= 0.9f)
        {
            asyncOperation.allowSceneActivation = true;
            SceneManager.LoadScene("SalleInstructions");
            Debug.Log("j'ai appuyé là");
        }
        else
        {
            SceneManager.LoadScene("SalleInstructions");
            Debug.Log("j'ai appuyé ici");
        }
    }
}
