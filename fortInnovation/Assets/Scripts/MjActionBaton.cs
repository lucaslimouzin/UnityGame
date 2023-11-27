using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MjActionBaton : MonoBehaviour
{
    public GameObject panelMjInfo;
    public GameObject chest;
    // Start is called before the first frame update
    void Start()
    {   
        Cursor.lockState = CursorLockMode.Locked;
        #if !UNITY_EDITOR && UNITY_WEBGL
            // disable WebGLInput.stickyCursorLock so if the browser unlocks the cursor (with the ESC key) the cursor will unlock in Unity
            WebGLInput.stickyCursorLock = true;
        #endif
        if (MainGameManager.Instance.gameBatonFait) {
                //active le coffre
                chest.SetActive(true);
        }
        else {
            //desactive le coffre
                chest.SetActive(false);
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            if (!MainGameManager.Instance.gameBatonFait) {
                panelMjInfo.SetActive(true);
                //Set Cursor to not be visible
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
             
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            if (!MainGameManager.Instance.gameBatonFait) {
                if (panelMjInfo.activeSelf){
                    panelMjInfo.SetActive(false);
                    //Set Cursor to not be visible
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }   
            }
            
        }
    }

     public void PlayGameBaton() {
        SceneManager.LoadScene("salleDes");
    }

    public void GoAccueil(){
        SceneManager.LoadScene("FortAccueil");
    }
}
