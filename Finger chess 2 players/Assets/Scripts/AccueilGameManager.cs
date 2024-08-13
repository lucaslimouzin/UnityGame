using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AccueilGameManager : MonoBehaviour
{
    //lancement du jeu
    public void PlayOfflineMode(){
        LoadSceneByName("HorsLigne_Gameplay");
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
