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
    public GameObject clou; 
    public GameObject marteauPlayer;
    public GameObject marteauMj;
    public GameObject panelInstruction;
    public GameObject panelInfoMJ;
    public GameObject panelJauge;
    public TextMeshProUGUI MJText;
    public Button buttonJauge;

    public float Force;
    public Slider forceMarteau;
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
        forceMarteau.value = 0;
        Force = 0;
      //on affiche le panneau des régles
        PanneauRegle();
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKey(KeyCode.Space)){
        Force++;
        Slider();
       } 
       if (Input.GetKeyUp(KeyCode.Space)){
        Force++;
        //relacheMarteau();
        StartCoroutine(Wait());
       }
    }

    public void Slider() {
        forceMarteau.value = Force;
    }

    public void ResetGauge(){
        Force = 0;
        forceMarteau.value = 0;
    }
    IEnumerator Wait(){
        yield return new WaitForSeconds(1.5f);
        ResetGauge();
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
