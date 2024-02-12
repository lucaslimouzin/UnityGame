using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FingerChessGame : MonoBehaviour
{
    public Button mainDroiteJoueur1Button;
    public Button mainGaucheJoueur1Button;
    public Button mainDroiteJoueur2Button;
    public Button mainGaucheJoueur2Button;

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


    void Start()
    {
        InitButtonAndImage(mainDroiteJoueur1Button, mainDroiteJoueur1Image, 1);
        InitButtonAndImage(mainGaucheJoueur1Button, mainGaucheJoueur1Image, 1);
        InitButtonAndImage(mainDroiteJoueur2Button, mainDroiteJoueur2Image, 1);
        InitButtonAndImage(mainGaucheJoueur2Button, mainGaucheJoueur2Image, 1);

        StartCoroutine(AfficherJoueurCommence());
    }

    IEnumerator AfficherJoueurCommence()
    {
        int joueurQuiCommence = Random.Range(1, 3);

        TextJoueur1Commence.text = "Joueur " + joueurQuiCommence + " commence";
        TextJoueur1Commence.enabled = true;

        TextJoueur2Commence.text = "Joueur " + joueurQuiCommence + " commence";
        TextJoueur2Commence.enabled = true;

        // Désactiver l'interactivité des boutons pendant la coroutine
        mainDroiteJoueur1Button.interactable = false;
        mainGaucheJoueur1Button.interactable = false;
        mainDroiteJoueur2Button.interactable = false;
        mainGaucheJoueur2Button.interactable = false;

        yield return new WaitForSeconds(0.5f);


        if (joueurQuiCommence == 1)
        {
            joueur1Turn = true;
            joueur2Turn = false;
            // Activer l'interactivité des boutons pour le joueur 1
            mainDroiteJoueur1Button.interactable = true;
            mainGaucheJoueur1Button.interactable = true;
        }
        else
        {
            joueur1Turn = false;
            joueur2Turn = true;
            // Activer l'interactivité des boutons pour le joueur 2
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

        // Trouver l'image associée au bouton
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

        // Vérifier si l'image est valide et le nouveau nombre de doigts est dans la plage des sprites
        if (buttonImage != null && nombreDoigts >= 0 && nombreDoigts <= 5)
        {
            // Mettre à jour le sprite de l'image associée au bouton
            buttonImage.sprite = spritesDoigts[nombreDoigts - 1];
        }
        else
        {
            // Mettre à jour le sprite de l'image associée au bouton
            buttonImage.sprite = spritesDoigts[5];
        }
    }


    void SetButtonOpacity(Button button, float opacity)
    {
        Color color = button.image.color;
        color.a = opacity;
        button.image.color = color;
    }

    public void OnButtonClick(Button button)
    {
        if (joueur1Turn)
        {

            if (boutonMainJoueur1Selected)
            {
                if (button == mainDroiteJoueur2Button || button == mainGaucheJoueur2Button)
                {
                    int doigtsJoueur1 = GetNumberOfFingers(boutonSelectionneJoueur1);
                    int doigtsJoueur2 = GetNumberOfFingers(button);

                    // ajoute le nombre de doigts au joueurs 2
                    int nouveauNombreDoigts = doigtsJoueur2 + doigtsJoueur1;
                    SetFingerCount(button, nouveauNombreDoigts);
                    

                    SetButtonOpacity(boutonSelectionneJoueur1, 0f);
                    mainDroiteJoueur2Button.interactable = true;
                    mainGaucheJoueur2Button.interactable = true;
                    Debug.Log("Bouton joueur 2 sélectionné: " + button.name);
                    button = null;
                    joueur1Turn = false;
                    joueur2Turn = true;
                    boutonMainJoueur1Selected = false;
                    TextJoueur1Commence.text = "Tour du joueur 2";
                    TextJoueur1Commence.enabled = true;
                    TextJoueur2Commence.text = "Tour du joueur 2";
                    TextJoueur2Commence.enabled = true;
                    

                }
            }
            else
            {
                if (button == mainDroiteJoueur1Button || button == mainGaucheJoueur1Button)
                {
                    //SetButtonOpacity(mainDroiteJoueur1Button, 0f);
                   // SetButtonOpacity(mainGaucheJoueur1Button, 0f);
                    SetButtonOpacity(button, 0.5f);
                    boutonSelectionneJoueur1 = button;
                    boutonMainJoueur1Selected = true;
                    mainDroiteJoueur2Button.interactable = true;
                    mainGaucheJoueur2Button.interactable = true;
                    Debug.Log("Bouton joueur 1 sélectionné: " + button.name);
                }
            }
        }

        //tour du joueur 2
        if (joueur2Turn)
        {
            if (boutonMainJoueur2Selected)
            {
                if (button == mainDroiteJoueur1Button || button == mainGaucheJoueur1Button)
                {
                    int doigtsJoueur2 = GetNumberOfFingers(boutonSelectionneJoueur2);
                    int doigtsJoueur1 = GetNumberOfFingers(button);

                    // ajoute le nombre de doigts au joueurs 2
                    int nouveauNombreDoigts = doigtsJoueur1 + doigtsJoueur2;
                    SetFingerCount(button, nouveauNombreDoigts);
                    

                    SetButtonOpacity(boutonSelectionneJoueur2, 0f);
                    mainDroiteJoueur1Button.interactable = true;
                    mainGaucheJoueur1Button.interactable = true;
                    Debug.Log("Bouton joueur 1 sélectionné: " + button.name);
                    button = null;
                    joueur2Turn = false;
                    boutonMainJoueur2Selected = false;
                    joueur1Turn = true;
                    TextJoueur1Commence.text = "Tour du joueur 1";
                    TextJoueur1Commence.enabled = true;
                    TextJoueur2Commence.text = "Tour du joueur 1";
                    TextJoueur2Commence.enabled = true;
                    
                }
            }
            else
            {
                if (button == mainDroiteJoueur2Button || button == mainGaucheJoueur2Button)
                {
                    //SetButtonOpacity(mainDroiteJoueur2Button, 0f);
                    //SetButtonOpacity(mainGaucheJoueur2Button, 0f);
                    SetButtonOpacity(button, 0.5f);
                    boutonSelectionneJoueur2 = button;
                    boutonMainJoueur2Selected = true;
                    mainDroiteJoueur1Button.interactable = true;
                    mainGaucheJoueur1Button.interactable = true;
                    Debug.Log("Bouton joueur 2 sélectionné: " + button.name);
                }
            }
        }
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
}
