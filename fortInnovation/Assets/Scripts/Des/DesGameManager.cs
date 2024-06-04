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
    public GameObject panelConsignes;
    public TextMeshProUGUI textNbParties;
    public TextMeshProUGUI textConsignes;
    public TextMeshProUGUI textTitre;
   void Start() {
    panelQuiCommence.SetActive(false);
    panelTirageDesDes.SetActive(false);
    panelConsignes.SetActive(true);

    switch (MainGameManager.Instance.jeuEnCours){
        case "JeuPaires":
            textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartiePairesJoue.ToString() + "/" + MainGameManager.Instance.nbPartiePaires.ToString();
            textTitre.text= "Le jeu des paires";
            textConsignes.text = "Règles du jeu : \n Des cartes face cachée sont disposées sur le plateau, votre mission est de former des paires identiques avant le Maître du jeu. \n \n Pour retourner deux cartes, répondez correctement à une question sur l'innovation participative. En cas de mauvaise réponse, c'est au maitre du jeu de retourner deux cartes. \n \n Pour former une paire, vous devez obligatoirement retourner une carte de la ligne du haut et une carte de la ligne du bas. \n \n Chaque paire correcte vous rapproche de la victoire : trouver trois paires vous permet de gagner une recommandation précieuse.";
            break;
        case "JeuBatons":
            textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieBatonJoue.ToString() + "/" + MainGameManager.Instance.nbPartieBaton.ToString();
            textConsignes.text = "";
            break;
        case "JeuClous":
            textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieClouJoue.ToString() + "/" + MainGameManager.Instance.nbPartieClou.ToString();
            textConsignes.text = "";
            break;
        case "JeuBassins":
            textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieBassinJoue.ToString() + "/" + MainGameManager.Instance.nbPartieBassin.ToString();
            textConsignes.text = "";
            break;
        case "JeuEnigmes":
            textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieEnigmesJoue.ToString() + "/" + MainGameManager.Instance.nbPartieEnigmes.ToString();
            textConsignes.text = "";
            break;
    }
        
   }

   public void fermerPannelConsigne(){
    panelQuiCommence.SetActive(false);
    panelConsignes.SetActive(false);
    panelTirageDesDes.SetActive(true);

   }

   public void ChoixDuJeu(){

    SceneManager.LoadScene(MainGameManager.Instance.jeuEnCours);
   }
}
