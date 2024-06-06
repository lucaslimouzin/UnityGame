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
            textConsignes.text = "Règles du jeu : \nDes cartes face cachée sont disposées sur le plateau, votre mission est de former des paires identiques avant le Maître du jeu. \n\nPour retourner deux cartes, répondez correctement à une question sur l'innovation participative. En cas de mauvaise réponse, c'est au maitre du jeu de retourner deux cartes. \n\nPour former une paire, vous devez obligatoirement retourner une carte de la ligne du haut et une carte de la ligne du bas. \n\nChaque paire correcte vous rapproche de la victoire : trouver trois paires vous permet de gagner une recommandation précieuse.";
            break;
        case "JeuBatons":
            textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieBatonJoue.ToString() + "/" + MainGameManager.Instance.nbPartieBaton.ToString();
            textTitre.text= "Le jeu des bâtonnets";
            textConsignes.text = "Règles du jeu : \nDes batonnets sont disposés au centre du plateau, votre mission est de ne pas retirer le dernier batonnet. \nPour retirer 1, 2 ou 3 batonnets, répondez correctement à une question sur l'innovation participative. En cas de mauvaise réponse, c'est au maitre du jeu de choisir le nombre de batonnets qu'il souhaite retirer.\nSi le maitre du jeu retire le dernier batonnet, vous remportez la manche et gagnez une recommandation.";
            break;
        case "JeuClous":
            textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieClouJoue.ToString() + "/" + MainGameManager.Instance.nbPartieClou.ToString();
            textTitre.text= "Le jeu des clous";
            textConsignes.text = "Règles du jeu : \nAu centre du plateau se trouve deux clous, l'un est à vous, l'autre au Maître du jeu.\nVotre mission est d'enfoncer votre clou avant celui du Maître du jeu.\nPour frapper votre clou, répondez correctement à une question sur l'innovation participative. En cas de bonne reponse, munissez-vous de votre marteau, déterminez votre force à l'aide de la jauge avant de frapper votre clou. En cas de mauvaise réponse, c'est au maitre du jeu de frapper son clou.\nSi vous enfoncer votre clou, intégralement dans le bois, en premier, vous remportez la manche et gagnez une recommandation.";
            break;
        case "JeuBassins":
            textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieBassinJoue.ToString() + "/" + MainGameManager.Instance.nbPartieBassin.ToString();
            textTitre.text= "Le jeu des bassins";
            textConsignes.text = "Règles du jeu : \nAu centre du plateau se trouve une bassine dans laquelle flotte 2 verres, l'un est à vous, l'autre au Maître du jeu.\nVotre mission est de faire couler le verre du Maître du jeu, en en y ajoutant des billes, avant qu'il ne coule le votre.\nPour ajouter une bille, répondez correctement à une question sur l'innovation participative. En cas de mauvaise réponse, c'est au maitre du jeu d'ajouter une bille dans votre verre.\nSi vous coulez le verre du Maître du jeu, vous remportez la manche et gagnez une recommandation.";
            break;
        case "JeuEnigmes":
            textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieEnigmesJoue.ToString() + "/" + MainGameManager.Instance.nbPartieEnigmes.ToString();
            textTitre.text= "L'Enigme'";
            textConsignes.text = "Règles du jeu : \nTentez de résoudre une charade pour découvrir le mot mystère.\nLorsque vous pensez avoir découvert le mot mystère, inscrivez le sur le damier à droit de l'écran en cliquant sur les lettres.";
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
