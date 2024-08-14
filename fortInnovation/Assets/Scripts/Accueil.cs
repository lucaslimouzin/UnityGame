using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Accueil : MonoBehaviour
{
    public Image[] characters;
    public int selectedCharacter = 0;

    void Start()
    {
        characters[0].enabled = true;
        characters[1].enabled = false;
        characters[2].enabled = false;
    }

    public void NextCharacter()
    {
        characters[selectedCharacter].enabled = false;
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        characters[selectedCharacter].enabled = true;
    }

    public void PreviousCharacter()
    {
        characters[selectedCharacter].enabled = false;
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        characters[selectedCharacter].enabled = true;
    }

    public void PlayGame()
    {
        MainGameManager.Instance.selectedCharacter = selectedCharacter;
        MainGameManager.Instance.jeuEnCours = "Instruction";
        MainGameManager.Instance.cinematiqueEnCours = "Introduction";
        SceneManager.LoadScene("AccueilPageLancement");
    
    }

}
