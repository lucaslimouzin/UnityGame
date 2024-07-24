using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MjActionInstructions : MonoBehaviour
{
    public GameObject panelMjInfo;
    //public TextMeshProUGUI textMjInfo;
    public GameObject panelReco;
    public GameObject panelRoom;
    public TextMeshProUGUI textMjRoom;
    public GameObject chest;
    public GameObject panelUi_Move;
    public GameObject panelUi_Jump;
    public TextMeshProUGUI texte1Coffre;
    public TextMeshProUGUI texte2Coffre;
    public TextMeshProUGUI texte1MjInfo;
    private StarterAssets.ThirdPersonController thirdPersonController;
    public Image imageScore;



    // Start is called before the first frame update
    void Start()
    {   
        // Trouver le script ThirdPersonController automatiquement au démarrage
        thirdPersonController = FindObjectOfType<StarterAssets.ThirdPersonController>();
        imageScore.sprite= MainGameManager.Instance.imageScore[1];
        MainGameManager.Instance.tutoCompteur = 0;
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
        textMjRoom.text = "Pour te déplacer,\nutilise les flèches de ton clavier.\nPour t'entrainer, essaie d'atteindre le coffre.";
        //textMjInfo.text = "Bien tu es prêt(e) à commencer l'aventure !\n Clique sur le bouton SORTIR et retrouve moi dans la salle suivante.\n Bonne chance !";
        //initialisation du texte du panel coffre
        texte1Coffre.text = "Durant la partie tu vas retrouver des panneaux d'instructions comme celui-çi.";
        texte2Coffre.text = "Ils te donneront des consignes pour les jeux et te permettront également de lire les recommandations gagnées.";
        texte1MjInfo.text = "Te voilà fin prêt(e) à débuter ton aventure !\n\nClique sur le bouton “entrer” et retrouve-moi dans la salle suivante.";
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnTriggerEnter(Collider other) {
        if (MainGameManager.Instance.tutoCompteur == 1) {
            if (other.gameObject.CompareTag("Player")){
                panelRoom.SetActive(false);
                panelMjInfo.SetActive(true);
 
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (MainGameManager.Instance.tutoCompteur == 1) {
            if (other.gameObject.CompareTag("Player")){
            panelRoom.SetActive(false);
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
        textMjRoom.text = "Dirige toi à présent vers la porte pour débuter l'aventure !";
        MainGameManager.Instance.tutoCompteur = 1;
        
    }
}
