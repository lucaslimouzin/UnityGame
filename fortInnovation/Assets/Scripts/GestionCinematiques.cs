using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestionCinematiques : MonoBehaviour
{
    public GameObject panelCinematiqueIntro;
   
    // Start is called before the first frame update
    void Start() {
        // Recherchez le bouton par son nom
        GameObject buttonSuivant = GameObject.Find("ButtonSuivant");
        buttonSuivant.SetActive(false);
        // Commencez une coroutine pour attendre 3 secondes
        StartCoroutine(MakeButtonVisibleAfterDelay(buttonSuivant, 6.0f));
        panelCinematiqueIntro.SetActive(false);
        switch (MainGameManager.Instance.cinematiqueEnCours){
            case "Introduction":
                panelCinematiqueIntro.SetActive(true);
                break;
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
        //redirige vers la map suivante
        SceneManager.LoadScene(MainGameManager.Instance.jeuEnCours);
    }

//fin script
}
