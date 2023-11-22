using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MjAction : MonoBehaviour
{
    public GameObject panelMjInfo;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            panelMjInfo.SetActive(true);
            //Set Cursor to not be visible
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; 
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            if (panelMjInfo.activeSelf){
                panelMjInfo.SetActive(false);
                //Set Cursor to not be visible
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

     public void PlayGameBaton() {
        SceneManager.LoadScene("jeuBatonQuestions");
    }
}
