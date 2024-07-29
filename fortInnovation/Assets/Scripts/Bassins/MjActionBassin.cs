using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MjActionBassin : MonoBehaviour
{
    public GameObject panelMjInfo;
    public TextMeshProUGUI textMjInfo;
    public GameObject chest;
    public Image imageScore;
    // Start is called before the first frame update
    void Start()
    {   
        
        //ajout v2
         if(MainGameManager.Instance.niveauSelect =="Normal"){
            imageScore.sprite= MainGameManager.Instance.imageScore[0];
        }else{
            imageScore.sprite= MainGameManager.Instance.imageScore[1];
        }
        //Cursor.lockState = CursorLockMode.Locked;
       // panelRoom.SetActive(true);
        #if !UNITY_EDITOR && UNITY_WEBGL
            // disable WebGLInput.stickyCursorLock so if the browser unlocks the cursor (with the ESC key) the cursor will unlock in Unity
            WebGLInput.stickyCursorLock = true;
        #endif
        if (MainGameManager.Instance.gameBassinFait) {
                //active le coffre
                chest.SetActive(true);
                textMjInfo.text = "Maître du jeu : Approche toi du coffre pour débloquer les recommandations gagnées !";
        }
        else {
            //desactive le coffre
                chest.SetActive(false);
            //change le message du panel Room
            textMjInfo.text = "Bienvenue dans la cellule des Bassins !\n\nVous allez affronter le Maître du jeu dans une épreuve d'adresse pour tenter de remporter les 3 recommandations du principe 3 de l'innovation participative : \"Accompagner l'expérimentation et le déploiement des innovations\".\nBonne chance !";
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
            if (!MainGameManager.Instance.gameBassinFait) {
                panelMjInfo.SetActive(true);
                
                
                
            }
             
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            if (!MainGameManager.Instance.gameBassinFait) {
                if (panelMjInfo.activeSelf){
                    panelMjInfo.SetActive(false); 
                }   
            }
        }
    }

    public void ExitPanel(){
        if (panelMjInfo.activeSelf){
        }   
    }

     public void PlayGame() {
        MainGameManager.Instance.jeuEnCours = "JeuBassins";
        SceneManager.LoadScene("SalleDes");
    }

    public void GoAccueil(){
        SceneManager.LoadScene("FortAccueil");
    }
}
