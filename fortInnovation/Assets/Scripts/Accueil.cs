using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Accueil : MonoBehaviour
{
    public GameObject[] characters;
    public int selectedCharacter =0;
    public void PlayGame() {
        MainGameManager.Instance.selectedCharacter = selectedCharacter;
        SceneManager.LoadScene("Third_FortAccueil");
        Debug.Log("test");
    }
}
