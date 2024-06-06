using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Instruction : MonoBehaviour
{
    private AsyncOperation asyncOperation;

    void Start()
    {
        StartCoroutine(PreloadScene("SalleInstructions"));
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

    public void PlayGame()
    {
        if (asyncOperation != null && asyncOperation.isDone)
        {
            asyncOperation.allowSceneActivation = true;
        }
        else
        {
            SceneManager.LoadScene("SalleInstructions");
        }
    }
}
