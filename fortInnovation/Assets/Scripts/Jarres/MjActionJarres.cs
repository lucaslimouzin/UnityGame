using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MjActionPaires : MonoBehaviour
{
    public GameObject panelMjInfo;
    public TextMeshProUGUI textMjInfo;
    public GameObject panelReco;
    public GameObject chest;
    // Start is called before the first frame update
    void Start()
    {   
        
        
        //Cursor.lockState = CursorLockMode.Locked;
        //panelRoom.SetActive(true);
        #if !UNITY_EDITOR && UNITY_WEBGL
            // disable WebGLInput.stickyCursorLock so if the browser unlocks the cursor (with the ESC key) the cursor will unlock in Unity
            WebGLInput.stickyCursorLock = true;
        #endif
        if (MainGameManager.Instance.gamePairesFait) {
                //active le coffre
                chest.SetActive(true);
                textMjInfo.text = "Maître du jeu : Approche toi du coffre pour débloquer les recommandations gagnées !";
        }
        else {
            //desactive le coffre
                chest.SetActive(false);
            //change le message du panel info
            if(MainGameManager.Instance.niveauSelect =="Normal"){
                textMjInfo.text = "Bienvenue dans la cellule des Paires ! \n\nVous allez affronter le Maître du jeu dans une épreuve de mémoire pour tenter de remporter les 5 recommandations du principe 1 de l'innovation participative : \"Donner du sens et la direction\". \nBonne chance !";
            }else{
                textMjInfo.text = "Bienvenue dans la cellule des Paires ! \n\nVous allez affronter le Maître du jeu dans une épreuve de mémoire.\nBonne chance !";
            }
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            if (!MainGameManager.Instance.gamePairesFait) {
                panelMjInfo.SetActive(true);   
            }
             
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            if (!MainGameManager.Instance.gamePairesFait) {
                if (panelMjInfo.activeSelf){
                    panelMjInfo.SetActive(false);
                }   
            }  
        }
    }

    public void ExitPanel(){
        if (panelMjInfo.activeSelf){
            panelReco.SetActive(false);  
        }   
    }

     public void PlayGame() {
        MainGameManager.Instance.jeuEnCours = "JeuPaires";
        SceneManager.LoadScene("SalleDes");
    }

    public void GoAccueil(){
        SceneManager.LoadScene("FortAccueil");
    }
}
