using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DesGameManager : MonoBehaviour
{
    public GameObject panelQuiCommence;
    public GameObject panelTirageDesDes;
    public TextMeshProUGUI textNbParties;
   void Start() {
    panelQuiCommence.SetActive(false);
    panelTirageDesDes.SetActive(true);
    switch (MainGameManager.Instance.jeuEnCours){
        case "jeuBatonQuestions":
            textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieBatonJoue.ToString() + "/" + MainGameManager.Instance.nbPartieBaton.ToString();
            break;
        case "jeuClou":
            textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieClouJoue.ToString() + "/" + MainGameManager.Instance.nbPartieClou.ToString();
            break;
    }
        
   }

   public void ChoixDuJeu(){

    SceneManager.LoadScene(MainGameManager.Instance.jeuEnCours);
   }
}
