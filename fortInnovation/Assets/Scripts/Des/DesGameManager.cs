using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DesGameManager : MonoBehaviour
{
    public GameObject panelInstructions;
    public GameObject panelTirageDesDes;
   void Start() {
    panelInstructions.SetActive(false);
    panelTirageDesDes.SetActive(true);

   }

   public void ChoixDuJeu(){

    SceneManager.LoadScene(MainGameManager.Instance.jeuEnCours);
   }
}
