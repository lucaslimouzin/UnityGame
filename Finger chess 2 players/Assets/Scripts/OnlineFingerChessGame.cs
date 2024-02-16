using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnlineFingerChessGame : MonoBehaviour
{
    public Button mainDroiteJoueur1Button;
    public Button mainGaucheJoueur1Button;
    public Button mainDroiteJoueur2Button;
    public Button mainGaucheJoueur2Button;
    public Button finDuTourJoueur1;
    public Button finDuTourJoueur2;
    public Button transfertJoueur1;
    public Button transfertJoueur2;
    public Button restartJoueur1;
    public Button restartJoueur2;
    public Image mainDroiteJoueur1Image;
    public Image mainGaucheJoueur1Image;
    public Image mainDroiteJoueur2Image;
    public Image mainGaucheJoueur2Image;

    public Sprite[] spritesDoigts;

    public TextMeshProUGUI TextJoueur1Commence;
    public TextMeshProUGUI TextJoueur2Commence;

    private bool joueur1Turn = true;
    private bool joueur2Turn = false;
    private Button boutonSelectionneJoueur1;
    private Button boutonSelectionneJoueur2;
    private bool boutonMainJoueur1Selected = false;
    private bool boutonMainJoueur2Selected = false;
    private bool aFaitUnTransfert = false;


    void Start()
    {
        InitButtonAndImage(mainDroiteJoueur1Button, mainDroiteJoueur1Image, 1);
        InitButtonAndImage(mainGaucheJoueur1Button, mainGaucheJoueur1Image, 1);
        InitButtonAndImage(mainDroiteJoueur2Button, mainDroiteJoueur2Image, 1);
        InitButtonAndImage(mainGaucheJoueur2Button, mainGaucheJoueur2Image, 1);
        finDuTourJoueur1.gameObject.SetActive(false);
        finDuTourJoueur2.gameObject.SetActive(false);
        transfertJoueur1.gameObject.SetActive(false);
        transfertJoueur2.gameObject.SetActive(false);
        restartJoueur1.gameObject.SetActive(false);
        restartJoueur2.gameObject.SetActive(false);
        aFaitUnTransfert = false;

        StartCoroutine(AfficherJoueurCommence());
    }

    IEnumerator AfficherJoueurCommence()
    {
        int joueurQuiCommence = Random.Range(1, 3);

        TextJoueur1Commence.text = "Joueur " + joueurQuiCommence + " commence";
        TextJoueur1Commence.enabled = true;

        TextJoueur2Commence.text = "Joueur " + joueurQuiCommence + " commence";
        TextJoueur2Commence.enabled = true;

        // D�sactiver l'interactivit� des boutons pendant la coroutine
        mainDroiteJoueur1Button.interactable = false;
        mainGaucheJoueur1Button.interactable = false;
        mainDroiteJoueur2Button.interactable = false;
        mainGaucheJoueur2Button.interactable = false;

        yield return new WaitForSeconds(0.5f);


        if (joueurQuiCommence == 1)
        {
            joueur1Turn = true;
            joueur2Turn = false;
            // Activer l'interactivit� des boutons pour le joueur 1
            mainDroiteJoueur1Button.interactable = true;
            mainGaucheJoueur1Button.interactable = true;
        }
        else
        {
            joueur1Turn = false;
            joueur2Turn = true;
            // Activer l'interactivit� des boutons pour le joueur 2
            mainDroiteJoueur2Button.interactable = true;
            mainGaucheJoueur2Button.interactable = true;
        }
    }

    void InitButtonAndImage(Button button, Image image, int nombreDoigts)
    {
        SetFingerCount(button, nombreDoigts);
        SetButtonOpacity(button, 0f);
    }

    void SetFingerCount(Button button, int nombreDoigts)
    {
        Image buttonImage = null;

        // Trouver l'image associ�e au bouton
        if (button == mainDroiteJoueur1Button)
        {
            buttonImage = mainDroiteJoueur1Image;
        }
        else if (button == mainGaucheJoueur1Button)
        {
            buttonImage = mainGaucheJoueur1Image;
        }
        else if (button == mainDroiteJoueur2Button)
        {
            buttonImage = mainDroiteJoueur2Image;
        }
        else if (button == mainGaucheJoueur2Button)
        {
            buttonImage = mainGaucheJoueur2Image;
        }

        // V�rifier si l'image est valide et le nouveau nombre de doigts est dans la plage des sprites
        if (buttonImage != null && nombreDoigts >= 0 && nombreDoigts <= 5)
        {
            // Mettre � jour le sprite de l'image associ�e au bouton
            buttonImage.sprite = spritesDoigts[nombreDoigts - 1];
        }
        else
        {
            // Mettre � jour le sprite de l'image associ�e au bouton
            buttonImage.sprite = spritesDoigts[5];
        }
    }


    void SetButtonOpacity(Button button, float opacity)
    {
        Color color = button.image.color;
        color.a = opacity;
        button.image.color = color;
    }

    //mecanique de changement de joueur
    public void OnButtonClick(Button button)
    {
        if (joueur1Turn)
        {
         tourJoueur1(button);   
        }

        //tour du joueur 2
        if (joueur2Turn)
        {
            tourJoueur2(button);
        }
    }

    public void desactiveToutesLesMains(){
        mainDroiteJoueur1Button.interactable = false;
        mainGaucheJoueur1Button.interactable = false;
        mainDroiteJoueur2Button.interactable = false;
        mainGaucheJoueur2Button.interactable = false;
    }
    public void reinitialisationOpacityButton(){
        SetButtonOpacity(mainDroiteJoueur1Button, 0f);
        SetButtonOpacity(mainDroiteJoueur2Button, 0f);
        SetButtonOpacity(mainGaucheJoueur1Button, 0f);
        SetButtonOpacity(mainGaucheJoueur2Button, 0f);
    }

    // Ajouter cette méthode pour gérer le clic sur le bouton finDuTourJoueur1
    public void OnEndTurnPlayer1ButtonClicked()
    {
        // Désactiver le bouton finDuTourJoueur1
        finDuTourJoueur1.gameObject.SetActive(false);
        transfertJoueur1.gameObject.SetActive(false);
        reinitialisationOpacityButton();

        int nombreDoigtsMainDroite = GetNumberOfFingers(mainDroiteJoueur2Button);
        int nombreDoigtsMainGauche = GetNumberOfFingers(mainGaucheJoueur2Button);
        if (nombreDoigtsMainDroite >= 5 ){
            mainDroiteJoueur2Button.interactable = false;
        } else {
            mainDroiteJoueur2Button.interactable = true;
        }
        if (nombreDoigtsMainGauche >= 5 ){
            mainGaucheJoueur2Button.interactable = false;
        } else{
            mainGaucheJoueur2Button.interactable = true;
        }
        
        joueur1Turn = false;
        joueur2Turn = true;
        boutonMainJoueur1Selected = false;
        TextJoueur1Commence.text = "Tour du joueur 2";
        TextJoueur1Commence.enabled = true;
        TextJoueur2Commence.text = "Tour du joueur 2";
        TextJoueur2Commence.enabled = true;
        aFaitUnTransfert = false;
    }
    
    // Ajouter cette méthode pour gérer le clic sur le bouton finDuTourJoueur2
    public void OnEndTurnPlayer2ButtonClicked()
    {
        // Désactiver le bouton finDuTourJoueur2
        finDuTourJoueur2.gameObject.SetActive(false);
        transfertJoueur2.gameObject.SetActive(false);
        reinitialisationOpacityButton();

        int nombreDoigtsMainDroite = GetNumberOfFingers(mainDroiteJoueur1Button);
        int nombreDoigtsMainGauche = GetNumberOfFingers(mainGaucheJoueur1Button);
        if (nombreDoigtsMainDroite >= 5 ){
            mainDroiteJoueur1Button.interactable = false;
        } else {
            mainDroiteJoueur1Button.interactable = true;
        }
        if (nombreDoigtsMainGauche >= 5 ){
            mainGaucheJoueur1Button.interactable = false;
        } else{
            mainGaucheJoueur1Button.interactable = true;
        }
        
        joueur2Turn = false;
        boutonMainJoueur2Selected = false;
        joueur1Turn = true;
        TextJoueur1Commence.text = "Tour du joueur 1";
        TextJoueur1Commence.enabled = true;
        TextJoueur2Commence.text = "Tour du joueur 1";
        TextJoueur2Commence.enabled = true;
        aFaitUnTransfert = false;
    }

    int GetNumberOfFingers(Button button)
    {
        if (button == mainDroiteJoueur1Button)
        {
            return GetFingerCount(mainDroiteJoueur1Image);
        }
        else if (button == mainGaucheJoueur1Button)
        {
            return GetFingerCount(mainGaucheJoueur1Image);
        }
        else if (button == mainDroiteJoueur2Button)
        {
            return GetFingerCount(mainDroiteJoueur2Image);
        }
        else if (button == mainGaucheJoueur2Button)
        {
            return GetFingerCount(mainGaucheJoueur2Image);
        }
        return 0;
    }

    int GetFingerCount(Image mainImage)
    {
        for (int i = 0; i < spritesDoigts.Length; i++)
        {
            if (mainImage.sprite == spritesDoigts[i])
            {
                return i + 1;
            }
        }
        return 0;
    }

    public void tourJoueur1(Button button) {
        if (boutonMainJoueur1Selected && !aFaitUnTransfert)
        {
            if (button == mainDroiteJoueur2Button || button == mainGaucheJoueur2Button)
            {
                int doigtsJoueur1 = GetNumberOfFingers(boutonSelectionneJoueur1);
                int doigtsJoueur2 = GetNumberOfFingers(button);
                transfertJoueur1.gameObject.SetActive(false);
                // ajoute le nombre de doigts au joueurs 2
                int nouveauNombreDoigts = doigtsJoueur2 + doigtsJoueur1;
                if(nouveauNombreDoigts >= 5){
                    SetFingerCount(button, 5);
                    button.interactable = false;
                } else {
                    SetFingerCount(button, nouveauNombreDoigts);
                }
                SetButtonOpacity(boutonSelectionneJoueur1, 0f);
                // Activer le bouton finDuTourJoueur1 lorsque le tour du joueur 1 est terminé
                finDuTourJoueur1.gameObject.SetActive(true);
                desactiveToutesLesMains();
                gameover();
            }
        }
        else
        {
            if (button == mainDroiteJoueur1Button || button == mainGaucheJoueur1Button)
            {
                SetButtonOpacity(button, 0.2f);
                boutonSelectionneJoueur1 = button;
                //affichage du bouton de transfert
                int nbDoigtsJoueurSelectionné = GetNumberOfFingers(button);
                if (nbDoigtsJoueurSelectionné >= 2){
                    transfertJoueur1.gameObject.SetActive(true);
                }
                boutonMainJoueur1Selected = true;

                int nombreDoigtsMainDroite = GetNumberOfFingers(mainDroiteJoueur2Button);
                int nombreDoigtsMainGauche = GetNumberOfFingers(mainGaucheJoueur2Button);
                if (nombreDoigtsMainDroite >= 5 ){
                    mainDroiteJoueur2Button.interactable = false;
                } else {
                    mainDroiteJoueur2Button.interactable = true;
                }
                if (nombreDoigtsMainGauche >= 5 ){
                    mainGaucheJoueur2Button.interactable = false;
                } else{
                    mainGaucheJoueur2Button.interactable = true;
                }
            }
        }
    }
    public void demandeTransfertJoueur1(){
        int nbDoigtsJoueurSelectionné = GetNumberOfFingers(boutonSelectionneJoueur1);
        if (boutonSelectionneJoueur1 == mainDroiteJoueur1Button &&  nbDoigtsJoueurSelectionné >= 2){
            
            int nbDoigtsReceveur = GetNumberOfFingers(mainGaucheJoueur1Button);
            if (nbDoigtsReceveur >= 5) {
                nbDoigtsReceveur = 0;
            }
            SetFingerCount(mainDroiteJoueur1Button, nbDoigtsJoueurSelectionné - 1);
            SetFingerCount(mainGaucheJoueur1Button, nbDoigtsReceveur + 1);
            mainGaucheJoueur1Button.interactable = true;
            aFaitUnTransfert = true;
            finDuTourJoueur1.gameObject.SetActive(true);
        }
        if (boutonSelectionneJoueur1 == mainGaucheJoueur1Button &&  nbDoigtsJoueurSelectionné >= 2){
            int nbDoigtsReceveur = GetNumberOfFingers(mainDroiteJoueur1Button);
            if (nbDoigtsReceveur >= 5) {
                nbDoigtsReceveur = 0;
            }
            SetFingerCount(mainGaucheJoueur1Button, nbDoigtsJoueurSelectionné - 1);
            SetFingerCount(mainDroiteJoueur1Button, nbDoigtsReceveur + 1);
            mainDroiteJoueur1Button.interactable = true;
            aFaitUnTransfert = true;
            finDuTourJoueur1.gameObject.SetActive(true);
        }
        
    }

    //tour du joueur 2
    public void tourJoueur2(Button button){
        if (boutonMainJoueur2Selected && !aFaitUnTransfert)
            {
                if (button == mainDroiteJoueur1Button || button == mainGaucheJoueur1Button)
                {
                    int doigtsJoueur2 = GetNumberOfFingers(boutonSelectionneJoueur2);
                    int doigtsJoueur1 = GetNumberOfFingers(button);
                    transfertJoueur2.gameObject.SetActive(false);
                    // ajoute le nombre de doigts au joueurs 2
                    int nouveauNombreDoigts = doigtsJoueur1 + doigtsJoueur2;
                    if(nouveauNombreDoigts >= 5){
                        SetFingerCount(button, 5);
                        button.interactable = false;
                    } else {
                        SetFingerCount(button, nouveauNombreDoigts);
                    }
                    SetButtonOpacity(boutonSelectionneJoueur2, 0f);
                    // Activer le bouton finDuTourJoueur2 lorsque le tour du joueur 1 est terminé
                    finDuTourJoueur2.gameObject.SetActive(true);
                    desactiveToutesLesMains();
                    gameover();
                    
                }
            }
            else
            {
                if (button == mainDroiteJoueur2Button || button == mainGaucheJoueur2Button)
                {
                    SetButtonOpacity(button, 0.2f);
                    boutonSelectionneJoueur2 = button;
                    int nbDoigtsJoueurSelectionné = GetNumberOfFingers(button);
                    if (nbDoigtsJoueurSelectionné >= 2){
                        transfertJoueur2.gameObject.SetActive(true);
                    }
                    boutonMainJoueur2Selected = true;
                    int nombreDoigtsMainDroite = GetNumberOfFingers(mainDroiteJoueur1Button);
                    int nombreDoigtsMainGauche = GetNumberOfFingers(mainGaucheJoueur1Button);
                    if (nombreDoigtsMainDroite >= 5 ){
                        mainDroiteJoueur1Button.interactable = false;
                    } else {
                        mainDroiteJoueur1Button.interactable = true;
                    }
                    if (nombreDoigtsMainGauche >= 5 ){
                        mainGaucheJoueur1Button.interactable = false;
                    } else{
                        mainGaucheJoueur1Button.interactable = true;
                    }
                }
            }
    }

    public void demandeTransfertJoueur2(){
        int nbDoigtsJoueurSelectionné = GetNumberOfFingers(boutonSelectionneJoueur2);
        if (boutonSelectionneJoueur2 == mainDroiteJoueur2Button &&  nbDoigtsJoueurSelectionné >= 2){
            int nbDoigtsReceveur = GetNumberOfFingers(mainGaucheJoueur2Button);
            if (nbDoigtsReceveur >= 5) {
                nbDoigtsReceveur = 0;
            }
            SetFingerCount(mainDroiteJoueur2Button, nbDoigtsJoueurSelectionné - 1);
            SetFingerCount(mainGaucheJoueur2Button, nbDoigtsReceveur + 1);
            mainGaucheJoueur2Button.interactable = true;
            aFaitUnTransfert = true;
            finDuTourJoueur2.gameObject.SetActive(true);
        }
        if (boutonSelectionneJoueur2 == mainGaucheJoueur2Button &&  nbDoigtsJoueurSelectionné >= 2){
            int nbDoigtsReceveur = GetNumberOfFingers(mainDroiteJoueur2Button);
            if (nbDoigtsReceveur >= 5) {
                nbDoigtsReceveur = 0;
            }
            SetFingerCount(mainGaucheJoueur2Button, nbDoigtsJoueurSelectionné - 1);
            SetFingerCount(mainDroiteJoueur2Button, nbDoigtsReceveur + 1);
            mainDroiteJoueur2Button.interactable = true;
            aFaitUnTransfert = true;
            finDuTourJoueur2.gameObject.SetActive(true);
        }
    }

    //gameover
    public void gameover(){
        int nombreDoigtsMainDroiteJoueur1 = GetNumberOfFingers(mainDroiteJoueur1Button);
        int nombreDoigtsMainGaucheJoueur1 = GetNumberOfFingers(mainGaucheJoueur1Button);
        int nombreDoigtsMainDroiteJoueur2 = GetNumberOfFingers(mainDroiteJoueur2Button);
        int nombreDoigtsMainGaucheJoueur2 = GetNumberOfFingers(mainGaucheJoueur2Button);

        if(nombreDoigtsMainDroiteJoueur1 >= 5 && nombreDoigtsMainGaucheJoueur1 >= 5) {
            TextJoueur1Commence.text = "Loose !!";
            TextJoueur1Commence.enabled = true;
            TextJoueur2Commence.text = "Win !!";
            TextJoueur2Commence.enabled = true;  
            finDuTourJoueur2.gameObject.SetActive(false);
            finDuTourJoueur1.gameObject.SetActive(false);  
            restartJoueur1.gameObject.SetActive(true);
            restartJoueur2.gameObject.SetActive(true);    
        }
        if(nombreDoigtsMainDroiteJoueur2 >= 5 && nombreDoigtsMainGaucheJoueur2 >= 5) {
            TextJoueur2Commence.text = "Loose !!";
            TextJoueur2Commence.enabled = true;
            TextJoueur1Commence.text = "Win !!";
            TextJoueur1Commence.enabled = true;     
            finDuTourJoueur2.gameObject.SetActive(false);
            finDuTourJoueur1.gameObject.SetActive(false);  
            restartJoueur1.gameObject.SetActive(true);
            restartJoueur2.gameObject.SetActive(true);   
        } 
    }

    public void restartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

//fin app
}

    
