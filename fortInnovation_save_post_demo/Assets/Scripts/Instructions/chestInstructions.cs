using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class chestInstructions : MonoBehaviour
{
    public GameObject panelMj;
    public TextMeshProUGUI textMjRoom;
    public GameObject panelReco;
    
    // Start is called before the first frame update
    void Start()
    {
        //ActivateButton(MainGameManager.Instance.scoreRecoEnigmes);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
                panelMj.SetActive(false);
                panelReco.SetActive(true);
                //Set Cursor to not be visible
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (panelReco.activeSelf){
            panelReco.SetActive(false);
            panelMj.SetActive(true);
            textMjRoom.text = "Maître du jeu : Maintenant, va vers la table pour débuter l'aventure !";
            //Set Cursor to not be visible
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            MainGameManager.Instance.tutoCompteur = 1;
        }
    }

    
}
