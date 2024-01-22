using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Instruction : MonoBehaviour
{
   

    void Start(){
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
   
    public void PlayGame() {
        SceneManager.LoadScene("SalleInstructions");
    }
}
