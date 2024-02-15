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
        case "JeuJarres":
            textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieJarresJoue.ToString() + "/" + MainGameManager.Instance.nbPartieJarres.ToString();
            break;
        case "JeuBatons":
            textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieBatonJoue.ToString() + "/" + MainGameManager.Instance.nbPartieBaton.ToString();
            break;
        case "JeuClous":
            textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieClouJoue.ToString() + "/" + MainGameManager.Instance.nbPartieClou.ToString();
            break;
        case "JeuBassins":
            textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieBassinJoue.ToString() + "/" + MainGameManager.Instance.nbPartieBassin.ToString();
            break;
        case "JeuEnigmes":
            textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieEnigmesJoue.ToString() + "/" + MainGameManager.Instance.nbPartieEnigmes.ToString();
            break;
    }
        
   }

   public void ChoixDuJeu(){

    SceneManager.LoadScene(MainGameManager.Instance.jeuEnCours);
   }
}
