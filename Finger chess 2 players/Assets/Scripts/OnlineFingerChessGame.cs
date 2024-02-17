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

    public RectTransform backgroundWhite;
    public RectTransform backgroundBlack;

    void Start()
    {
        string playerColor = (string)PhotonNetwork.LocalPlayer.CustomProperties["playerColor"];
        InitializePlayerColor(playerColor);
    }


    void InitializePlayerColor(string playerColor)
    {
        if (playerColor == "blanc")
        {
            Debug.Log("j'ai bien les blancs");
            // Configuration pour le joueur blanc
        }
        else if (playerColor == "noir")
        {
            // Configuration pour le joueur noir
            Debug.Log("j'ai bien les noirs");
        }
    }






    //fin app
}


