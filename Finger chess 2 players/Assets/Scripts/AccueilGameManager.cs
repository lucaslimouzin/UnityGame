using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AccueilGameManager : MonoBehaviour
{
    //lancement du jeu
    public void PlayOfflineMode(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PlayOnlineMode()
    {
        LoadSceneByName("PhotonMap");
    }

   
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //fin script
 }
