using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class resetGAme : MonoBehaviour
{


    public void recommencerGame(){
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

    //retour vers accueil
    SceneManager.LoadScene("Accueil");

    }

    
    
}