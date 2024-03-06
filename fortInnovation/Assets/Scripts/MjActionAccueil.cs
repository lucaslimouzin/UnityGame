using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MjActionAccueil : MonoBehaviour
{
    public GameObject panelMjInfo;
    public GameObject panelRoom;
    public TextMeshProUGUI textMjRoom;
    public TextMeshProUGUI textMjInfo;
    private StarterAssets.ThirdPersonController thirdPersonController;

    // Start is called before the first frame update
    void Start()
    {   
         // Trouver le script ThirdPersonController automatiquement au démarrage
        thirdPersonController = FindObjectOfType<StarterAssets.ThirdPersonController>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //Cursor.lockState = CursorLockMode.Locked;
        panelRoom.SetActive(true);
        // desactive les entrées de gameplay
        DisableGameplayInput();
        #if !UNITY_EDITOR && UNITY_WEBGL
            // disable WebGLInput.stickyCursorLock so if the browser unlocks the cursor (with the ESC key) the cursor will unlock in Unity
            WebGLInput.stickyCursorLock = true;
        #endif
        //si on a fini l'ensemble du jeu on se tp vers la salle de fin
        if (MainGameManager.Instance.gameJarresFait && MainGameManager.Instance.gameBatonFait && MainGameManager.Instance.gameBassinFait && MainGameManager.Instance.gameClouFait && MainGameManager.Instance.gameEnigmesFait){
           textMjRoom.text = "Bravo vous avez fini toutes les épreuves!\n On se retrouve dans la salle des récompenses !"; 
           StartCoroutine(LoadSceneAfterDelay("SalleFinDuJeu", 4f));
        } else if (MainGameManager.Instance.tutoCompteur == 2) {
                textMjRoom.text = "Bienvenue dans Fort Innovation !!! \n Approche toi de moi pour avoir les explications sur ton aventure \n dans le fort ...";
        }
        else {
            //change le message du panel Room
            textMjRoom.text = "Continue d'explorer les autres salles \n Avance vers la voie de l'innovation...";
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (MainGameManager.Instance.tutoCompteur == 2){
            if (other.gameObject.CompareTag("Player")){
                panelRoom.SetActive(false);
                panelMjInfo.SetActive(true);
                textMjInfo.text = "\nHéros de l'innovation,\nbienvenue dans l'enceinte sacrée du Fort de l'Innovation.\n\nVotre quête ici est noble et essentielle. En parcourant les cinq cellules mystiques de ce fort, vous serez initiés aux principes sacrés de l'innovation participative.\n\nChaque cellule est un sanctuaire dédié à un principe spécifique, un lieu où se révèlent les leviers d'action - les bonnes pratiques - et les pièges - les écueils à éviter - qui façonnent le paysage de l'innovation.Votre périple à travers ces cellules a un double objectif. Premièrement, il vise à approfondir votre compréhension et votre mise en œuvre des bonnes pratiques et des écueils à éviter, afin de cultiver un terreau fertile pour l'innovation au sein de la Caisse.\n\nDeuxièmement, il est conçu pour vous équiper d'une sagesse pratique, vous permettant d'identifier et d'activer les leviers nécessaires pour animer l'innovation participative au sein de vos équipes.";
                //Set Cursor to not be visible
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                // Désactive les entrées de gameplay
                DisableGameplayInput();
            }
        } 
    }

    private void OnTriggerExit(Collider other) {
        if (MainGameManager.Instance.tutoCompteur == 2){
                if (other.gameObject.CompareTag("Player")){
                panelRoom.SetActive(true);
                textMjRoom.text = "Dirige toi vers une cellule \n Pour commencer ton aventure...";
                // desactive les entrées de gameplay
                DisableGameplayInput();
                if (panelMjInfo.activeSelf){
                    panelMjInfo.SetActive(false);
                    //Set Cursor to not be visible
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }  
            }
        }
    }

    public void textSuivant(){
        if (MainGameManager.Instance.tutoCompteur==2) {
            textMjInfo.text = "À chaque étape de ce voyage, vous serez confrontés à des défis qui testeront votre capacité à répondre à des questions opérationnelles.\n\nCes défis sont des portes vers la connaissance, ouvrant sur des recommandations précieuses que vous, et l'ensemble des managers, pourrez embrasser dans vos pratiques futures. Préparez-vous à plonger dans l'essence même de l'innovation participative, à découvrir ses principes fondateurs et à les incarner dans votre quête pour transformer et inspirer.\n\nQue votre voyage au sein du Fort de l'Innovation soit riche d'enseignements et de découvertes.\n\nQue la sagesse acquise ici guide vos pas vers une ère d'innovation sans précédent au sein de la Caisse.\n\n En avant, braves innovateurs, votre aventure commence maintenant!";
            MainGameManager.Instance.tutoCompteur = 3;
        }
        else{
            panelRoom.SetActive(true);
            textMjRoom.text = "Dirige toi vers une cellule \n Pour commencer ton aventure...";
            panelMjInfo.SetActive(false);
            //Set Cursor to not be visible
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            // desactive les entrées de gameplay
            DisableGameplayInput();
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

    public void EnableGameplayInput()
    {
        // Réactive les entrées de gameplay
        if (thirdPersonController != null)
        {
            thirdPersonController.enabled = true;
        }
    }
    private IEnumerator LoadSceneAfterDelay(string sceneName, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene(sceneName);
    }

}
