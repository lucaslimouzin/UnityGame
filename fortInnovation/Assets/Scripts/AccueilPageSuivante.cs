using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccueilPageSuivante : MonoBehaviour
{
    

    public void PlayInstructionModeNormal()
    {
        
        MainGameManager.Instance.niveauSelect = "Normal";
        StartCoroutine(LoadSettingsAndStartGame("reglagesModeNormal.json"));
    }

    public void PlayInstructionModeEasy()
    {
        MainGameManager.Instance.niveauSelect = "Facile";
        StartCoroutine(LoadSettingsAndStartGame("reglagesModeEasy.json"));
    }

    private IEnumerator LoadSettingsAndStartGame(string settingsFileName)
{
    yield return MainGameManager.Instance.LoadSettings(settingsFileName);

    

    SceneManager.LoadScene("Cinematiques");
}

}
