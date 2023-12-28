using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Accueil : MonoBehaviour
{
    public void PlayGame() {
        SceneManager.LoadScene("FortAccueil");
        Debug.Log("test");
    }
}
