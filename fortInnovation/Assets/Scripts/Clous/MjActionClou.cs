using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MjActionClou : MonoBehaviour
{
    public GameObject panelMjInfo;
    public GameObject panelReco;
    public GameObject panelRoom;
    public TextMeshProUGUI textMjRoom;
    public GameObject chest;
    // Start is called before the first frame update
    void Start()
    {   
        Cursor.lockState = CursorLockMode.Locked;
        panelRoom.SetActive(true);
        #if !UNITY_EDITOR && UNITY_WEBGL
            // disable WebGLInput.stickyCursorLock so if the browser unlocks the cursor (with the ESC key) the cursor will unlock in Unity
            WebGLInput.stickyCursorLock = true;
        #endif
        if (MainGameManager.Instance.gameClouFait) {
                //active le coffre
                chest.SetActive(true);
                textMjRoom.text = "Maître du jeu : Approche toi du coffre pour débloquer les recommandations gagnées !";
        }
        else {
            //desactive le coffre
                chest.SetActive(false);
            //change le message du panel Room
            textMjRoom.text = "Maître du jeu : Bienvenue dans la cellule aux clous enfoncés. Approche toi pour lancer le jeu";
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            panelRoom.SetActive(false);
            if (!MainGameManager.Instance.gameClouFait) {
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
            if (!MainGameManager.Instance.gameClouFait) {
                if (panelMjInfo.activeSelf){
                    panelMjInfo.SetActive(false);
                    //Set Cursor to not be visible
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }   
            }
            
        }
    }

    public void ExitPanel(){
        if (panelMjInfo.activeSelf){
            panelReco.SetActive(false);
            //Set Cursor to not be visible
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }   
    }

     public void PlayGame() {
        MainGameManager.Instance.jeuEnCours = "JeuClous";
        SceneManager.LoadScene("SalleDes");
    }

    public void GoAccueil(){
        SceneManager.LoadScene("FortAccueil");
    }
}
