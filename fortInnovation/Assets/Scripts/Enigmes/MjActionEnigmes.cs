using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MjActionEnigmes : MonoBehaviour
{
    public GameObject panelMjInfo;
    public GameObject panelReco;
    public GameObject panelRoom;
    public TextMeshProUGUI textMjRoom;
    public GameObject chest;
    // Start is called before the first frame update
    void Start()
    {   
        
        
        //Cursor.lockState = CursorLockMode.Locked;
        panelRoom.SetActive(true);
        #if !UNITY_EDITOR && UNITY_WEBGL
            // disable WebGLInput.stickyCursorLock so if the browser unlocks the cursor (with the ESC key) the cursor will unlock in Unity
            WebGLInput.stickyCursorLock = true;
        #endif
        if (MainGameManager.Instance.gameEnigmesFait) {
                //active le coffre
                chest.SetActive(true);
                textMjRoom.text = "Maître du jeu : Approche toi du coffre pour débloquer les recommandations gagnées !";
        }
        else {
            //desactive le coffre
                chest.SetActive(false);
            //change le message du panel Room
            textMjRoom.text = "Maître du jeu : Bienvenue dans la cellule aux Enigmes. Approche toi pour lancer le jeu";
        }
       
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
            if (!MainGameManager.Instance.gameEnigmesFait) {
                panelMjInfo.SetActive(true);
                
                
                
            }
             
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            panelRoom.SetActive(false);
            if (!MainGameManager.Instance.gameEnigmesFait) {
                if (panelMjInfo.activeSelf){
                    panelMjInfo.SetActive(false);
                    
                    
                    
                }   
            }
            
        }
    }

    public void ExitPanel(){
        if (panelMjInfo.activeSelf){
            panelReco.SetActive(false);
            
            
            
        }   
    }

     public void PlayGame() {
        MainGameManager.Instance.jeuEnCours = "JeuEnigmes";
        SceneManager.LoadScene("SalleDes");
    }

    public void GoAccueil(){
        SceneManager.LoadScene("FortAccueil");
    }
}
