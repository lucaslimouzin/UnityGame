using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManagerMarteau : MonoBehaviour
{
    public GameObject[] baton; 
    public GameObject panelInstruction;
    public GameObject panelInfoMJ;
    public GameObject panelJauge;
    public TextMeshProUGUI MJText;
    public Button buttonJauge;
    private bool aJuste = false;
    private bool win = false;
    private bool firstTimeMj;
    

    //--------pour mettre à jour le score --------------------------------------
    private void OnEnable()
    {
        // S'abonner à l'événement OnScoreUpdated
        MainGameManager.OnScoreUpdated += HandleScoreUpdated;
    }

    private void OnDisable()
    {
        // Se désabonner de l'événement OnScoreUpdated lors de la désactivation du script
        MainGameManager.OnScoreUpdated -= HandleScoreUpdated;
    }

    // Méthode appelée lorsque le score est mis à jour
    private void HandleScoreUpdated(int newScore)
    {
        // Faire quelque chose avec le nouveau score
        Debug.Log("Nouveau score : " + newScore);
    }
    //-------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        firstTimeMj = false;
      //on affiche le panneau des régles
        PanneauRegle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //affichage du panneau des règles
    private void PanneauRegle (){
        panelInstruction.SetActive(true);
    }
    
    // retrait panneau des règles
    //affichage du panneau de la règle
    public void RetraitPanneauRegle (){
        panelInstruction.SetActive(false);

        if (MainGameManager.Instance.quiCommence == "Player"){
            //tour du player
            TourDuJoueur();
            
        }
        else {
            panelInfoMJ.SetActive(true);
            firstTimeMj = true;
            MJText.text = "Maitre du jeu : Je commence à retirer des bâtons !";
            TourDuMj();
        }
        
    }


    //fonction qui check le bouton enfoncé
    private void OnButtonClick(){
        // Supprimer tous les écouteurs d'événements du bouton
        buttonJauge.onClick.RemoveAllListeners();
    }




    private void TourDuJoueur(){
        
    }

    private void TourDuMj(){
 
    }



    
    //fin du jeu 
    private void FinDuJeu(){
        if(MainGameManager.Instance.nbPartieClouJoue == 3 ){
            MainGameManager.Instance.gameClouFait = true;
            SceneManager.LoadScene("ClouEnfonce");
        }
        else {
            SceneManager.LoadScene("salleDes");
        }
    }
}
