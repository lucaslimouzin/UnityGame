using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Accueil : MonoBehaviour
{
    public Image[] characters;
    public int selectedCharacter =0;

    void Start(){
        characters[0].enabled = true;
        characters[1].enabled = false;
        characters[2].enabled = false;
        
        
    }
    public void NextCharacter(){
        characters[selectedCharacter].enabled = false;
        selectedCharacter = (selectedCharacter +1 ) % characters.Length;
        characters[selectedCharacter].enabled = true;
    }

    public void PreviousCharacter(){
        characters[selectedCharacter].enabled = false;
        selectedCharacter--;
        if (selectedCharacter <0){
            selectedCharacter += characters.Length;
        }
        characters[selectedCharacter].enabled = true;
    }

    public void PlayInstructionModeNormal() {
        MainGameManager.Instance.selectedCharacter = selectedCharacter;
        MainGameManager.Instance.jeuEnCours = "Instruction";
        MainGameManager.Instance.cinematiqueEnCours = "Introduction";
        SceneManager.LoadScene("Cinematiques");
        //modification des parties en fonction de la difficulté
        MainGameManager.Instance.nbPartiePaires = 3;
        MainGameManager.Instance.nbPartieBaton = 4;
        MainGameManager.Instance.nbPartieClou = 3;
        MainGameManager.Instance.nbPartieBassin = 3;
    }
    public void PlayInstructionModeEasy() {
        MainGameManager.Instance.selectedCharacter = selectedCharacter;
        MainGameManager.Instance.jeuEnCours = "Instruction";
        MainGameManager.Instance.cinematiqueEnCours = "Introduction";
        SceneManager.LoadScene("Cinematiques");
        //modification des parties en fonction de la difficulté
        MainGameManager.Instance.nbPartiePaires = 1;
        MainGameManager.Instance.nbPartieBaton = 1;
        MainGameManager.Instance.nbPartieClou = 1;
        MainGameManager.Instance.nbPartieBassin = 1;
    }
}
