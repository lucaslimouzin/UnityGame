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

    public RectTransform backgroundWhite;
    public RectTransform backgroundBlack;

    public TextMeshProUGUI textInfo; // Référence à l'objet TextMeshProUGUI

    void Start()
    {
        string playerColor = (string)PhotonNetwork.LocalPlayer.CustomProperties["playerColor"];
        InitializeBackgrounds(playerColor);
        
    }

    void InitializeBackgrounds(string playerColor)
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
            buttonLeftPlayerAdverse = buttonLeftBlack;

            UpdateTextInfo("À vous de commencer !");
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
            buttonLeftPlayerAdverse = buttonLeftWhite;
            UpdateTextInfo("À votre adversaire de commencer ! ");
        }
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



    //fin app
}

    
