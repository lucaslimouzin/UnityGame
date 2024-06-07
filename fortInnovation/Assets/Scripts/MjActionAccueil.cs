using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MjActionAccueil : MonoBehaviour
{
    public GameObject panelMjInfo;
    public GameObject panelRoom;
    public GameObject pointExclamation;
    public TextMeshProUGUI textMjRoom;
    public TextMeshProUGUI textMjInfo;
    private StarterAssets.ThirdPersonController thirdPersonController;
    public GameObject panelQuest;
    public TMP_Text checklistText;
    public GameObject Bulle;
    public GameObject[] doorGameObjects;
    public GameObject buttonFermer;
    public GameObject buttonTeleportation;

    // Start is called before the first frame update
    void Start()
    {   
         // Trouver le script ThirdPersonController automatiquement au démarrage
        thirdPersonController = FindObjectOfType<StarterAssets.ThirdPersonController>();
        buttonFermer.SetActive(true);
        buttonTeleportation.SetActive(false);
        //Cursor.lockState = CursorLockMode.Locked;
        panelRoom.SetActive(true);
        // desactive les entrées de gameplay
        DisableGameplayInput();
        #if !UNITY_EDITOR && UNITY_WEBGL
            // disable WebGLInput.stickyCursorLock so if the browser unlocks the cursor (with the ESC key) the cursor will unlock in Unity
            WebGLInput.stickyCursorLock = true;
        #endif
        if (MainGameManager.Instance.gamePairesFait && MainGameManager.Instance.gameBatonFait && MainGameManager.Instance.gameBassinFait && MainGameManager.Instance.gameClouFait && MainGameManager.Instance.gameEnigmesFait){
           panelRoom.SetActive(false);
           buttonFermer.SetActive(false);
           buttonTeleportation.SetActive(true);
           EnableGameplayInput();
           
        } else if (MainGameManager.Instance.tutoCompteur == 2) {
                panelQuest.SetActive(false);
                textMjRoom.text = "Bienvenue dans Fort Innovation. \nApproche-toi et viens en apprendre davantage sur ton aventure !";
        }
        else {
            //change le message du panel Room
            Bulle.SetActive(false);
            pointExclamation.SetActive(false);
            panelQuest.SetActive(true);
            textMjRoom.text = "Continue d'explorer les autres salles \nAvance vers la voie de l'innovation...";
        }
        MettreAJourChecklist();
       
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnTriggerEnter(Collider other) {
         if (MainGameManager.Instance.gamePairesFait && MainGameManager.Instance.gameBatonFait && MainGameManager.Instance.gameBassinFait && MainGameManager.Instance.gameClouFait && MainGameManager.Instance.gameEnigmesFait){
           panelRoom.SetActive(true);
           textMjRoom.text = "Bravo vous avez fini toutes les épreuves !\nOn se retrouve dans la dernière cellule !"; 
           buttonFermer.SetActive(false);
           buttonTeleportation.SetActive(true);
        } else if (MainGameManager.Instance.tutoCompteur == 2){
            Bulle.SetActive(false);
            if (other.gameObject.CompareTag("Player")){
                panelRoom.SetActive(false);
                panelMjInfo.SetActive(true);
                pointExclamation.SetActive(false);
                textMjInfo.text = "Pour terminer ta quête, tu devras visiter les 5 cellules du Fort qui se trouvent derière moi et affronter le Maître du jeu. \n\nAu cours de ton aventure, un panneau t'indique les cellules qu'il te reste à visiter. \n\nBonne chance à toi aventurier de l'innovation !";
                
                
                
                // Désactive les entrées de gameplay
                DisableGameplayInput();
            }
        }
        else {
            panelRoom.SetActive(true);
            pointExclamation.SetActive(false);
            textMjRoom.text = "Continue d'explorer les autres salles.\nAvance vers la voie de l'innovation...";
            panelMjInfo.SetActive(false);
        }
        
    }

    private void OnTriggerExit(Collider other) {
        if (MainGameManager.Instance.tutoCompteur == 2){
            // if (other.gameObject.CompareTag("Player")){
            //     panelRoom.SetActive(true);
            //     textMjRoom.text = "Dirige toi vers une cellule \nPour commencer ton aventure...";
            //     // desactive les entrées de gameplay
            //     DisableGameplayInput();
            //     if (panelMjInfo.activeSelf){
            //         panelMjInfo.SetActive(false);
            //         
            //         
            //         
            //     }  
            // }
        }
        panelRoom.SetActive(false);
    }

    public void textSuivant(){
        panelRoom.SetActive(true);
        pointExclamation.SetActive(false);
        panelQuest.SetActive(true);
        textMjRoom.text = "Dirige toi vers une cellule \nPour commencer ton aventure...";
        MainGameManager.Instance.tutoCompteur = 3;
        panelMjInfo.SetActive(false);
        
        
        
        // desactive les entrées de gameplay
        DisableGameplayInput();
        
    }

    public void goFinDuJeu(){
       SceneManager.LoadScene("SalleFinDuJeu");
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

    // Méthode pour mettre à jour la checklist
    void MettreAJourChecklist()
    {
        // Initialiser le texte avec les noms de jeux
        string[] nomsJeux = { "Cellule des paires", "Cellule des bâtonnets", "Cellule des bassins", "Cellule des clous", "Cellule des énigmes"};
        bool[] statutJeux = { MainGameManager.Instance.gamePairesFait, MainGameManager.Instance.gameBatonFait, MainGameManager.Instance.gameBassinFait, MainGameManager.Instance.gameClouFait, MainGameManager.Instance.gameEnigmesFait};

        string checklist = "Les cellules restantes à visiter :\n\n";
        for (int i = 0; i < nomsJeux.Length; i++)
        {
            // Si le jeu est fait, le barrer et réduire l'opacité, sinon l'afficher normalement
            checklist += statutJeux[i] ? $"<color=#80808080><s>. {nomsJeux[i]}</s></color>\n\n" : $". {nomsJeux[i]}\n\n";
        }


        // Mettre à jour le texte du TextMeshPro
        checklistText.text = checklist;

        //modifie l'opacité des pancartes si les salles sont faites
        // Modifier l'opacité des pancartes si les salles sont faites
        UpdateDoorOpacity(MainGameManager.Instance.gamePairesFait, doorGameObjects[0]);
        UpdateDoorOpacity(MainGameManager.Instance.gameBatonFait, doorGameObjects[1]);
        UpdateDoorOpacity(MainGameManager.Instance.gameBassinFait, doorGameObjects[2]);
        UpdateDoorOpacity(MainGameManager.Instance.gameClouFait, doorGameObjects[3]);
        UpdateDoorOpacity(MainGameManager.Instance.gameEnigmesFait, doorGameObjects[4]);
    }

    // Méthode publique pour mettre à jour la checklist après modification des booléens
    // public void UpdateJeuxChecklist(bool nouveauJeu1, bool nouveauJeu2, bool nouveauJeu3)
    // {
    //     jeu1 = nouveauJeu1;
    //     jeu2 = nouveauJeu2;
    //     jeu3 = nouveauJeu3;

    //     MettreAJourChecklist();
    // }
    void UpdateDoorOpacity(bool gameStatus, GameObject doorGameObject)
    {
        if (gameStatus)
        {
            // Récupérer le composant TextMeshPro du GameObject
            TextMeshPro textDoor = doorGameObject.GetComponent<TextMeshPro>();

            if (textDoor != null)
            {
                // Récupérer la couleur actuelle
                Color currentColor = textDoor.color;

                // Définir l'alpha à 40% (0.4)
                currentColor.a = 0.4f;

                // Appliquer la nouvelle couleur
                textDoor.color = currentColor;
            }
            else
            {
                Debug.LogError($"TextMeshPro component not found on the GameObject: {doorGameObject.name}");
            }
        }
    }

}
