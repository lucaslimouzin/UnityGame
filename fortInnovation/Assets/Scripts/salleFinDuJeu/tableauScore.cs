using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tableauScore : MonoBehaviour
{
    public GameObject panelTableauScores;
    public GameObject panelModeSimple;
    
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
            if(MainGameManager.Instance.niveauSelect =="Normal"){
                panelTableauScores.SetActive(true);
            }else{
                panelModeSimple.SetActive(true);
            } 
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            if(MainGameManager.Instance.niveauSelect =="Normal"){
                panelTableauScores.SetActive(false);
            }else{
                panelModeSimple.SetActive(false);
            } 
        }        
    }
}
