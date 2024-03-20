using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptButtonHome : MonoBehaviour
{
    public GameObject panelUi_Move;
    public GameObject panelUi_Jump;


    public void activeUi(){
        MainGameManager.Instance.panelUiMobile = !MainGameManager.Instance.panelUiMobile;
        if (MainGameManager.Instance.panelUiMobile){
            
            panelUi_Jump.SetActive(true);
            panelUi_Move.SetActive(true);
        } else {
            panelUi_Jump.SetActive(false);
            panelUi_Move.SetActive(false);
        }
    }

    //pour mettre en plein écran
    public void ToggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    
}
