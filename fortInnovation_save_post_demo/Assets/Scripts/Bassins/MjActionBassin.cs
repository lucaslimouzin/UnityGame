using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MjActionBassin : MonoBehaviour
{
    public GameObject panelMjInfo;
    public GameObject panelReco;
    public GameObject panelRoom;
    public TextMeshProUGUI textMjRoom;
    public GameObject chest;
    // Start is called before the first frame update
    void Start()
    {   
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //Cursor.lockState = CursorLockMode.Locked;
        panelRoom.SetActive(true);
        #if !UNITY_EDITOR && UNITY_WEBGL
            // disable WebGLInput.stickyCursorLock so if the browser unlocks the cursor (with the ESC key) the cursor will unlock in Unity
            WebGLInput.stickyCursorLock = true;
        #endif
        if (MainGameManager.Instance.gameBassinFait) {
                //active le coffre
                chest.SetActive(true);
                textMjRoom.text = "Maître du jeu : Approche toi du coffre pour débloquer les recommandations gagnées !";
        }
        else {
            //desactive le coffre
                chest.SetActive(false);
            //change le message du panel Room
            textMjRoom.text = "Maître du jeu : Bienvenue dans la cellule des bassins coulants. Approche toi pour lancer le jeu";
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            panelRoom.SetActive(false);
            if (!MainGameManager.Instance.gameBassinFait) {
                panelMjInfo.SetActive(true);
                //Set Cursor to not be visible
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
             
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            panelRoom.SetActive(false);
            if (!MainGameManager.Instance.gameBassinFait) {
                if (panelMjInfo.activeSelf){
                    panelMjInfo.SetActive(false);
                    //Set Cursor to not be visible
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }   
            }
            
        }
    }

    public void ExitPanel(){
        if (panelMjInfo.activeSelf){
            panelReco.SetActive(false);
            //Set Cursor to not be visible
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
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
