using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tableauScore : MonoBehaviour
{
    public GameObject panelTableauScores;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            panelTableauScores.SetActive(true);
            
            
            
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            if (panelTableauScores.activeSelf){
                panelTableauScores.SetActive(false);
                
                
            
            }   
        }        
    }
}
