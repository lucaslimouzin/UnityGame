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

    // Start is called before the first frame update
    void Start()
    {   
         // Trouver le script ThirdPersonController automatiquement au démarrage
        thirdPersonController = FindObjectOfType<StarterAssets.ThirdPersonController>();
        
        
        //Cursor.lockState = CursorLockMode.Locked;
        panelRoom.SetActive(true);
        // desactive les entrées de gameplay
        DisableGameplayInput();
        #if !UNITY_EDITOR && UNITY_WEBGL
            // disable WebGLInput.stickyCursorLock so if the browser unlocks the cursor (with the ESC key) the cursor will unlock in Unity
            WebGLInput.stickyCursorLock = true;
        #endif
        //si on a fini l'ensemble du jeu on se tp vers la salle de fin
        if (MainGameManager.Instance.gamePairesFait && MainGameManager.Instance.gameBatonFait && MainGameManager.Instance.gameBassinFait && MainGameManager.Instance.gameClouFait && MainGameManager.Instance.gameEnigmesFait){
           textMjRoom.text = "Bravo vous avez fini toutes les épreuves!\n On se retrouve dans la salle des récompenses !"; 
           StartCoroutine(LoadSceneAfterDelay("SalleFinDuJeu", 4f));
        } else if (MainGameManager.Instance.tutoCompteur == 2) {
                panelQuest.SetActive(false);
                textMjRoom.text = "Bienvenue dans Fort Innovation. \nApproche-toi et viens en apprendre davantage sur ton aventure !";
        }
        else {
            //change le message du panel Room
            pointExclamation.SetActive(false);
            panelQuest.SetActive(true);
            textMjRoom.text = "Continue d'explorer les autres salles \n Avance vers la voie de l'innovation...";
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
        if (MainGameManager.Instance.tutoCompteur == 2){
            if (other.gameObject.CompareTag("Player")){
                panelRoom.SetActive(false);
                panelMjInfo.SetActive(true);
                pointExclamation.SetActive(false);
                textMjInfo.text = "Pour terminer ta quête, tu devras visiter les 5 cellules du Fort qui se trouvent derière moi et affronter le maître du jeu. \n \n Au cours de ton aventure, un parchemin t'indique les cellules qu'il te reste à visiter. \n \n Bonne chance à toi aventurier de l'innovation !";
                
                
                
                // Désactive les entrées de gameplay
                DisableGameplayInput();
            }
        } 
    }

    private void OnTriggerExit(Collider other) {
        if (MainGameManager.Instance.tutoCompteur == 2){
            // if (other.gameObject.CompareTag("Player")){
            //     panelRoom.SetActive(true);
            //     textMjRoom.text = "Dirige toi vers une cellule \n Pour commencer ton aventure...";
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
    }

    public void textSuivant(){
        panelRoom.SetActive(true);
        pointExclamation.SetActive(false);
        panelQuest.SetActive(true);
        textMjRoom.text = "Dirige toi vers une cellule \n Pour commencer ton aventure...";
        MainGameManager.Instance.tutoCompteur = 3;
        panelMjInfo.SetActive(false);
        
        
        
        // desactive les entrées de gameplay
        DisableGameplayInput();
        
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
        string[] nomsJeux = { "Jeu des paires", "Jeu des bâtons", "Jeu des bassins", "Jeu des clous", "Jeu des énigmes"};
        bool[] statutJeux = { MainGameManager.Instance.gamePairesFait, MainGameManager.Instance.gameBatonFait, MainGameManager.Instance.gameBassinFait, MainGameManager.Instance.gameClouFait, MainGameManager.Instance.gameEnigmesFait};

        string checklist = "Jeux à faire :\n";
        for (int i = 0; i < nomsJeux.Length; i++)
        {
            // Si le jeu est fait, le barrer avec `<s>`, sinon l'afficher normalement
            checklist += statutJeux[i] ? $"<s>- {nomsJeux[i]}</s>\n" : $"- {nomsJeux[i]}\n";
        }

        // Mettre à jour le texte du TextMeshPro
        checklistText.text = checklist;
    }

    // Méthode publique pour mettre à jour la checklist après modification des booléens
    // public void UpdateJeuxChecklist(bool nouveauJeu1, bool nouveauJeu2, bool nouveauJeu3)
    // {
    //     jeu1 = nouveauJeu1;
    //     jeu2 = nouveauJeu2;
    //     jeu3 = nouveauJeu3;

    //     MettreAJourChecklist();
    // }

}
