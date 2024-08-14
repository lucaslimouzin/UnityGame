using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DesGameManager : MonoBehaviour
{
    public Sprite[] headCharacter;
    public Image headAffiche;
    public GameObject panelQuiCommence;
    public GameObject panelTirageDesDes;
    public GameObject panelConsignes;
    public TextMeshProUGUI textNbParties;
    public TextMeshProUGUI textConsignes;
    public TextMeshProUGUI textTitre;
    public Image imageScore;
    public Transform boutonCommencer;

    void Start() {
        
        //ajout v2
         if(MainGameManager.Instance.niveauSelect =="Normal"){
            imageScore.sprite= MainGameManager.Instance.imageScore[0];
        }else{
            imageScore.sprite= MainGameManager.Instance.imageScore[1];
        }
        
        panelQuiCommence.SetActive(false);
        panelTirageDesDes.SetActive(false);
        panelConsignes.SetActive(true);

        switch (MainGameManager.Instance.selectedCharacter) {
            case 0:
                headAffiche.sprite = headCharacter[0];
                break;
            case 1:
                headAffiche.sprite = headCharacter[1];
                break;
            case 2:
                headAffiche.sprite = headCharacter[2];
                break;
        }

        // Configuration initiale des textes et des consignes
        UpdateTexts(MainGameManager.Instance.jeuEnCours);
    }

    void UpdateTexts(string jeuEnCours)
    {
        switch (jeuEnCours) {
            case "JeuPaires":
                textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartiePairesJoue.ToString() + "/" + MainGameManager.Instance.nbPartiePaires.ToString();
                textTitre.text = "Le jeu des paires";
                //ajout v2
                if(MainGameManager.Instance.niveauSelect =="Normal"){
                    textConsignes.text = "Règles du jeu :\n\nDes cartes face cachée sont disposées sur le plateau, votre mission est de former des paires identiques avant le Maître du jeu. \n\nPour retourner deux cartes, répondez correctement à une question sur l'innovation participative. En cas de mauvaise réponse, c'est au Maître du jeu de retourner deux cartes. \n\nPour former une paire, vous devez obligatoirement retourner une carte de la ligne du haut et une carte de la ligne du bas. \n\nChaque paire correcte vous rapproche de la victoire : trouver trois paires vous permet de gagner une recommandation précieuse.";
                }else{
                    textConsignes.text = "Règles du jeu :\n\nDes cartes face cachée sont disposées sur le plateau, votre mission est de former des paires identiques avant le Maître du jeu. \n\nPour retourner deux cartes, répondez correctement à une question sur l'innovation participative. En cas de mauvaise réponse, c'est au Maître du jeu de retourner deux cartes. \n\nPour former une paire, vous devez obligatoirement retourner une carte de la ligne du haut et une carte de la ligne du bas. \n\nChaque paire correcte vous rapproche de la victoire : trouver trois paires vous permet de gagner le duel.";
                    textNbParties.gameObject.SetActive(false);
                    RectTransform rectTransform = boutonCommencer.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(0, -330);
                }
                
                if (MainGameManager.Instance.nbPartiePairesJoue >= 2)
                {
                    textConsignes.gameObject.SetActive(false);
                    RectTransform rectTransform = textNbParties.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(0, -466); // La position indiquée dans votre image
                }
                break;
            case "JeuBatons":
                textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieBatonJoue.ToString() + "/" + MainGameManager.Instance.nbPartieBaton.ToString();
                textTitre.text = "Le jeu des bâtonnets";
                //ajout v2
                if(MainGameManager.Instance.niveauSelect =="Normal"){
                    textConsignes.text = "Règles du jeu :\n\nDes bâtonnets sont disposés au centre du plateau, votre mission est de ne pas retirer le dernier bâtonnet. \n\nPour retirer 1, 2 ou 3 bâtonnets, répondez correctement à une question sur l'innovation participative. En cas de mauvaise réponse, c'est au Maître du jeu de choisir le nombre de bâtonnets qu'il souhaite retirer.\n\nSi le Maître du jeu retire le dernier bâtonnet, vous remportez la manche et gagnez une recommandation.";
                }else{
                    textConsignes.text = "Règles du jeu :\n\nDes bâtonnets sont disposés au centre du plateau, votre mission est de ne pas retirer le dernier bâtonnet. \n\nPour retirer 1, 2 ou 3 bâtonnets, répondez correctement à une question sur l'innovation. En cas de mauvaise réponse, c'est au Maître du jeu de choisir le nombre de bâtonnets qu'il souhaite retirer.\n\nSi le Maître du jeu retire le dernier bâtonnet, vous remportez le duel.";
                    textNbParties.gameObject.SetActive(false);
                    RectTransform rectTransform = boutonCommencer.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(0, -330);
                }
                
                if (MainGameManager.Instance.nbPartieBatonJoue >= 2)
                {
                    textConsignes.gameObject.SetActive(false);
                    RectTransform rectTransform = textNbParties.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(0, -466);
                }
                break;
            case "JeuClous":
                textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieClouJoue.ToString() + "/" + MainGameManager.Instance.nbPartieClou.ToString();
                textTitre.text = "Le jeu des clous";
                //ajout v2
                if(MainGameManager.Instance.niveauSelect =="Normal"){
                    textConsignes.text = "Règles du jeu :\n\nAu centre du plateau se trouve deux clous, l'un est à vous, l'autre au Maître du jeu.\n\nVotre mission est d'enfoncer votre clou avant celui du Maître du jeu.\n\nPour frapper votre clou, répondez correctement à une question sur l'innovation participative. En cas de bonne reponse, munissez-vous de votre marteau, déterminez votre force à l'aide de la jauge avant de frapper votre clou. En cas de mauvaise réponse, c'est au Maître du jeu de frapper son clou.\n\nSi vous enfoncer votre clou, intégralement dans le bois, en premier, vous remportez la manche et gagnez une recommandation.";
                }else{
                    textConsignes.text = "Règles du jeu :\n\nAu centre du plateau se trouve deux clous, l'un est à vous, l'autre au Maître du jeu.\n\nVotre mission est d'enfoncer votre clou avant celui du Maître du jeu.\n\nPour frapper votre clou, répondez correctement à une question sur l'innovation. En cas de bonne reponse, munissez-vous de votre marteau, déterminez votre force à l'aide de la jauge avant de frapper votre clou. En cas de mauvaise réponse, c'est au Maître du jeu de frapper son clou.\n\nSi vous enfoncer votre clou, intégralement dans le bois, en premier, vous remportez le duel.";
                    textNbParties.gameObject.SetActive(false);
                    RectTransform rectTransform = boutonCommencer.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(0, -330);
                }
                
                if (MainGameManager.Instance.nbPartieClouJoue >= 2)
                {
                    textConsignes.gameObject.SetActive(false);
                    RectTransform rectTransform = textNbParties.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(0, -466);
                }
                break;
            case "JeuBassins":
                textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieBassinJoue.ToString() + "/" + MainGameManager.Instance.nbPartieBassin.ToString();
                textTitre.text = "Le jeu des bassins";
                //ajout v2
                if(MainGameManager.Instance.niveauSelect =="Normal"){
                    textConsignes.text = "Règles du jeu :\n\nAu centre du plateau se trouve une bassine dans laquelle flotte 2 verres, l'un est à vous, l'autre au Maître du jeu.\n\nVotre mission est de faire couler le verre du Maître du jeu, en en y ajoutant des billes, avant qu'il ne coule le votre.\n\nPour ajouter une bille, répondez correctement à une question sur l'innovation participative. En cas de mauvaise réponse, c'est au Maître du jeu d'ajouter une bille dans votre verre.\n\nSi vous coulez le verre du Maître du jeu, vous remportez la manche et gagnez une recommandation.";
                }else{
                    textConsignes.text = "Règles du jeu :\n\nAu centre du plateau se trouve une bassine dans laquelle flotte 2 verres, l'un est à vous, l'autre au Maître du jeu.\n\nVotre mission est de faire couler le verre du Maître du jeu, en en y ajoutant des billes, avant qu'il ne coule le votre.\n\nPour ajouter une bille, répondez correctement à une question sur l'innovation. En cas de mauvaise réponse, c'est au Maître du jeu d'ajouter une bille dans votre verre.\n\nSi vous coulez le verre du Maître du jeu, vous remportez le duel.";
                    textNbParties.gameObject.SetActive(false);
                    RectTransform rectTransform = boutonCommencer.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(0, -330);
                }
                
                if (MainGameManager.Instance.nbPartieBassinJoue >= 2)
                {
                    textConsignes.gameObject.SetActive(false);
                    RectTransform rectTransform = textNbParties.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(0, -466);
                }
                break;
            case "JeuEnigmes":
                textNbParties.text = "Nombre de parties : " + MainGameManager.Instance.nbPartieEnigmesJoue.ToString() + "/" + MainGameManager.Instance.nbPartieEnigmes.ToString();
                textTitre.text = "L'Enigme";
                //ajout v2
                if(MainGameManager.Instance.niveauSelect =="Normal"){
                    textConsignes.text = "Règles du jeu :\n\nTentez de résoudre une charade pour découvrir le mot mystère.\n\nLorsque vous pensez avoir découvert le mot mystère, inscrivez le sur le damier à droit de l'écran en cliquant sur les lettres.";
                }else{
                    textConsignes.text = "Règles du jeu :\n\nTentez de résoudre une énigme pour découvrir le mot mystère.\n\nLorsque vous pensez avoir découvert le mot mystère, inscrivez le sur le damier à droit de l'écran en cliquant sur les lettres.";
                    textNbParties.gameObject.SetActive(false);
                    RectTransform rectTransform = boutonCommencer.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(0, -330);
                }
                
                if (MainGameManager.Instance.nbPartieEnigmesJoue >= 2)
                {
                    textConsignes.gameObject.SetActive(false);
                    RectTransform rectTransform = textNbParties.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(0, -466);
                }
                break;
        }
    }

    public void fermerPannelConsigne()
    {
        panelQuiCommence.SetActive(false);
        panelConsignes.SetActive(false);
        panelTirageDesDes.SetActive(true);
    }

    public void ChoixDuJeu()
    {
        SceneManager.LoadScene(MainGameManager.Instance.jeuEnCours);
    }
}
