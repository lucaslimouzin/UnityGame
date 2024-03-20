using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptButtonHome : MonoBehaviour
{
    public GameObject panelUi_Move;



    public void activeUi(){
        MainGameManager.Instance.panelUiMobile = !MainGameManager.Instance.panelUiMobile;
        if (MainGameManager.Instance.panelUiMobile){
            panelUi_Move.SetActive(true);
        } else {
            panelUi_Move.SetActive(false);
        }
    }

    
    
}
