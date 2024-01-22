using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MjActionInstructions : MonoBehaviour
{
    public GameObject panelMjInfo;
    public GameObject panelReco;
    public GameObject panelRoom;
    public TextMeshProUGUI textMjRoom;
    public GameObject chest;
    public GameObject panelUi_Move;
    public GameObject panelUi_Jump;
    // Start is called before the first frame update
    void Start()
    {   
        //activation de l'ui mobile si vrai 
        if (MainGameManager.Instance.panelUiMobile){
            panelUi_Jump.SetActive(true);
            panelUi_Move.SetActive(true);
        }
        //Cursor.lockState = CursorLockMode.Locked;
        panelRoom.SetActive(true);
        #if !UNITY_EDITOR && UNITY_WEBGL
            // disable WebGLInput.stickyCursorLock so if the browser unlocks the cursor (with the ESC key) the cursor will unlock in Unity
            WebGLInput.stickyCursorLock = true;
        #endif
        //active le coffre
            chest.SetActive(true);
        //change le message du panel Room
        textMjRoom.text = "Maître du jeu : Bienvenue dans Fort Innovation. Pour te déplacer utilise ZQSD ou les flèches. Essayes d'atteindre le coffre derrière moi !";
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            panelRoom.SetActive(false);
            panelMjInfo.SetActive(true);
            //Set Cursor to not be visible
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            panelRoom.SetActive(false);
            if (panelMjInfo.activeSelf){
                panelMjInfo.SetActive(false);
                //Set Cursor to not be visible
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
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
        SceneManager.LoadScene("FortAccueil");
    }

    public void GoAccueil(){
        SceneManager.LoadScene("FortAccueil");
    }
}
