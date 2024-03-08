using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MjActionInstructions : MonoBehaviour
{
    public GameObject panelMjInfo;
    public TextMeshProUGUI textMjInfo;
    public GameObject panelReco;
    public GameObject panelRoom;
    public TextMeshProUGUI textMjRoom;
    public GameObject chest;
    public GameObject panelUi_Move;
    public GameObject panelUi_Jump;
    private StarterAssets.ThirdPersonController thirdPersonController;
    // Start is called before the first frame update
    void Start()
    {   
        // Trouver le script ThirdPersonController automatiquement au démarrage
        thirdPersonController = FindObjectOfType<StarterAssets.ThirdPersonController>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        MainGameManager.Instance.tutoCompteur = 0;
        //activation de l'ui mobile si vrai 
        if (MainGameManager.Instance.panelUiMobile){
            Cursor.visible = true;
            panelUi_Jump.SetActive(false);
            panelUi_Move.SetActive(true);
        }
        //Cursor.lockState = CursorLockMode.Locked;
        panelRoom.SetActive(true);
        //desactive le deplacement
        DisableGameplayInput();
        #if !UNITY_EDITOR && UNITY_WEBGL
            // disable WebGLInput.stickyCursorLock so if the browser unlocks the cursor (with the ESC key) the cursor will unlock in Unity
            WebGLInput.stickyCursorLock = true;
        #endif
        //active le coffre
            chest.SetActive(true);
        //change le message du panel Room
        textMjRoom.text = "Pour te déplacer utilise ZQSD ou les flèches. \n Essayes d'atteindre le coffre !";
        textMjInfo.text = "Bien tu es prêt(e) à commençez l'aventure !\n Clique sur le bouton SORTIR et retrouve moi dans la salle suivante.\n Bonne chance !";
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (MainGameManager.Instance.tutoCompteur == 1) {
            if (other.gameObject.CompareTag("Player")){
                panelRoom.SetActive(false);
                panelMjInfo.SetActive(true);
                //Set Cursor to not be visible
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (MainGameManager.Instance.tutoCompteur == 1) {
            if (other.gameObject.CompareTag("Player")){
            panelRoom.SetActive(false);
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
        MainGameManager.Instance.tutoCompteur = 2;
        SceneManager.LoadScene("FortAccueil");
    }

    public void GoAccueil(){
        SceneManager.LoadScene("FortAccueil");
    }

    public void DisableGameplayInput()
    {
        // Désactive les entrées de gameplay
        if (thirdPersonController != null)
        {
            thirdPersonController.enabled = false;
        }
    }

    public void EnableGameplayInput()
    {
        // Réactive les entrées de gameplay
        if (thirdPersonController != null)
        {
            thirdPersonController.enabled = true;
        }
        panelRoom.SetActive(true);
        //desactive le deplacement
        DisableGameplayInput();
        textMjRoom.text = "Maintenant, va vers la porte pour débuter l'aventure !";
        //Set Cursor to not be visible
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        MainGameManager.Instance.tutoCompteur = 1;
        
    }
}
