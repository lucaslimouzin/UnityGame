using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class chestInstructions : MonoBehaviour
{
    public GameObject panelRoom;
    public TextMeshProUGUI textMjRoom;
    public GameObject panelReco;
    public GameObject exclamation;
    
    private StarterAssets.ThirdPersonController thirdPersonController;
    
    // Start is called before the first frame update
    void Start()
    {
        //ActivateButton(MainGameManager.Instance.scoreRecoEnigmes);
        
        
        // Trouver le script ThirdPersonController automatiquement au démarrage
        thirdPersonController = FindObjectOfType<StarterAssets.ThirdPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
                panelRoom.SetActive(false);
                panelReco.SetActive(true);
                DisableGameplayInput();
                
                
                
        }
    }

    private void OnTriggerExit(Collider other) {
        if (panelReco.activeSelf){
            panelReco.SetActive(false);
            panelRoom.SetActive(true);
            //desactive le deplacement
            DisableGameplayInput();
            
            exclamation.SetActive(false);
            
            
            
            MainGameManager.Instance.tutoCompteur = 1;
        }
    }

    public void DisableGameplayInput()
    {
        // Désactive les entrées de gameplay
        if (thirdPersonController != null)
        {
            thirdPersonController.enabled = false;
        }
    }

    
}
