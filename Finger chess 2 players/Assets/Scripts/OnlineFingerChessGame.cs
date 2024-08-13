using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class OnlineFingerChessGame : MonoBehaviour
{
    public Button buttonLeftWhite;
    public Button buttonRightWhite;
    public Button buttonLeftBlack;
    public Button buttonRightBlack;
    private Button buttonLeftPlayerLocal;
    private Button buttonRightPlayerLocal;
    private Button buttonLeftPlayerAdverse;
    private Button buttonRightPlayerAdverse;
    public Image mainDroiteWhiteImage;
    public Image mainGaucheWhiteImage;
    public Image mainDroiteBlackImage;
    public Image mainGaucheBlackImage;
    private Image mainDroitePlayerLocalImage;
    private Image mainGauchePlayerLocalImage;
    private Image mainDroitePlayerAdverseImage;
    private Image mainGauchePlayerAdverseImage;

    public RectTransform backgroundWhite;
    public RectTransform backgroundBlack;

    public TextMeshProUGUI textInfo; // Référence à l'objet TextMeshProUGUI

    private bool isPlayerTurn;
    private bool isTransferDone = false;
    public Sprite[] spritesDoigts;

    void Start()
    {
        string playerColor = (string)PhotonNetwork.LocalPlayer.CustomProperties["playerColor"];
        InitializeBackgroundsAndImages(playerColor);
        
    }

    // ------ Partie Initialisation du jeu -----------
    void InitializeBackgroundsAndImages(string playerColor)
    {
  
        if (playerColor == "blanc")
        {
            // Configure les backgrounds pour le joueur blanc
            SetRectTransform(backgroundWhite, 500, 0, 0);
            SetRectTransform(backgroundBlack, 0, 500, 0);
            //faire en sorte que les boutons blancs soient aux joueur local et noirs à l'adversaire
            buttonLeftPlayerLocal = buttonLeftWhite;
            buttonRightPlayerLocal = buttonRightWhite;
            buttonLeftPlayerAdverse = buttonLeftBlack;
            buttonRightPlayerAdverse = buttonRightBlack;
            //de même pour les images des boutons
            mainDroitePlayerLocalImage = mainDroiteWhiteImage;
            mainGauchePlayerLocalImage = mainGaucheWhiteImage;
            mainDroitePlayerAdverseImage = mainDroiteBlackImage;
            mainGauchePlayerAdverseImage = mainGaucheBlackImage;

            UpdateTextInfo("À vous de commencer !");
            isPlayerTurn = playerColor == "blanc";
        }
        else if (playerColor == "noir")
        {
            // Configure les backgrounds pour le joueur noir
            SetRectTransform(backgroundWhite, 0, 500, 180);
            SetRectTransform(backgroundBlack, 500, 0, 180);
            //faire en sorte que les boutons noirs soient aux joueur local et blancs à l'adversaire
            buttonLeftPlayerLocal = buttonLeftBlack;
            buttonRightPlayerLocal = buttonRightBlack;
            buttonLeftPlayerAdverse = buttonLeftWhite;
            buttonRightPlayerAdverse = buttonRightWhite;
            //de même pour les images des boutons
            mainDroitePlayerLocalImage = mainDroiteBlackImage;
            mainGauchePlayerLocalImage = mainGaucheBlackImage;
            mainDroitePlayerAdverseImage = mainDroiteWhiteImage;
            mainGauchePlayerAdverseImage = mainGaucheWhiteImage;

            UpdateTextInfo("À votre adversaire de commencer ! ");
            isPlayerTurn = playerColor == "noir";
        }

        InitButtonAndImage(buttonRightPlayerLocal, mainDroitePlayerLocalImage, 1);
        InitButtonAndImage(buttonLeftPlayerLocal, mainGauchePlayerLocalImage, 1);
        InitButtonAndImage(buttonRightPlayerAdverse, mainDroitePlayerAdverseImage, 1);
        InitButtonAndImage(buttonLeftPlayerAdverse, mainGauchePlayerAdverseImage, 1);

    }

    void InitButtonAndImage(Button button, Image image, int nombreDoigts)
    {
        SetFingerCount(button, nombreDoigts);
        //on appelle la méthode qui gère l'opacité
        SetButtonOpacity(button, 0f);
    }

    void SetFingerCount(Button button, int nombreDoigts)
    {
        Image buttonImage = null;

        // Trouver l'image associ�e au bouton
        if (button == buttonRightPlayerLocal)
        {
            buttonImage = mainDroitePlayerLocalImage;
        }
        else if (button == buttonLeftPlayerLocal)
        {
            buttonImage = mainGauchePlayerLocalImage;
        }
        else if (button == buttonRightPlayerAdverse)
        {
            buttonImage = mainDroitePlayerAdverseImage;
        }
        else if (button == buttonLeftPlayerAdverse)
        {
            buttonImage = mainGauchePlayerAdverseImage;
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

    void SetRectTransform(RectTransform rectTransform, float top, float bottom, float zRotation)
    {
        // Configure l'ancrage et l'offset
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, bottom); // left, bottom
        rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -top); // -right, -top

        // Configure la rotation
        rectTransform.localEulerAngles = new Vector3(rectTransform.localEulerAngles.x, rectTransform.localEulerAngles.y, zRotation);
    }


    void UpdateTextInfo(string message)
    {
        if (textInfo != null)
        {
            textInfo.text = message; // Met à jour le texte de l'objet TextMeshProUGUI
        }
    }

    // ---------------- Partie Gameplay --------------


    //fin app
}

    
