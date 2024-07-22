using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
//pour récupérer en réseau le fichier Json
using UnityEngine.Networking;
using System.Linq;

public class GameManagerEnigmes : MonoBehaviour
{
    public GameObject panelInstruction;
    public GameObject panelInfoMJ;
    public GameObject panelEnigmes;
    public GameObject buttonTextEnigmes;
    public TextMeshProUGUI MJText;
    public TextMeshProUGUI textEnigmeInfo;
    public TextMeshProUGUI textEnigme;
    private bool tourJoueur = true;
    private bool finDuJeu = false;
    
    

    //variables damier
    public List<LetterButton> allLetterButtons; // Liste de tous les boutons de lettres
    public string wordToFind = "ECOSYSTEME"; // Le mot à trouver
    private int compteurEssai;
  
    public TextMeshProUGUI gagnePerduText;
    
    

    

    //--------pour mettre à jour le score --------------------------------------
    private void OnEnable()
    {
        // S'abonner à l'événement OnScoreUpdated
        MainGameManager.OnScoreUpdated += HandleScoreUpdated;
    }

    private void OnDisable()
    {
        // Se désabonner de l'événement OnScoreUpdated lors de la désactivation du script
         MainGameManager.OnScoreUpdated -= HandleScoreUpdated;
    }

    // Méthode appelée lorsque le score est mis à jour
    private void HandleScoreUpdated(int scorePaires, int scoreBaton, int scoreClou, int scoreBassin, int scoreEnigmes)
    {
        // Faire quelque chose avec le nouveau score
        MainGameManager.Instance.scoreReco = scorePaires + scoreBaton + scoreClou + scoreBassin + scoreEnigmes; 
       
    }
    //-------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        if(MainGameManager.Instance.niveauSelect =="Normal"){
            wordToFind = "ECOSYSTEME";
        }else{
            wordToFind = "PARTICIPATIF";//Mode facile
            textEnigme.text = "Je suis souvent synonyme de collaboration, un processus où chacun peut apporter sa contribution. Je valorise l'implication collective, pour créer ensemble quelque chose de positif.";
        }
        panelEnigmes.SetActive(false);
        panelInfoMJ.SetActive(false);
        gagnePerduText.gameObject.SetActive(false); // Masque le texte
        compteurEssai = 0;
        
        //on affiche le panneau des régles
        PanneauRegle();
        //RetraitPanneauRegle();
    }

   
    private void Update()
    {
       
    }

    //affichage du panneau des règles
    private void PanneauRegle (){
        panelInstruction.SetActive(true);
    }
    
    // retrait panneau des règles
    //affichage du panneau de la règle
    public void RetraitPanneauRegle (){
        panelInstruction.SetActive(false);
        panelEnigmes.SetActive(true);
        panelInfoMJ.SetActive(true);

        TourDuJoueur();
    }

    private void TourDuJoueur(){
        if(tourJoueur) {
            buttonTextEnigmes.SetActive(true);
            ////debug.Log("Debut tour joueur");  
        }  
    }

   
     public void ValidateWord()
    {
        compteurEssai +=1;
        string selectedWord = "";
        foreach (LetterButton letterButton in allLetterButtons)
        {
            if (letterButton.isSelected)
            {
                selectedWord += letterButton.gameObject.name; // Utilisez le nom du GameObject ou une propriété personnalisée pour la lettre
                Debug.Log(selectedWord);
            }
        }

        if (IsWordCorrect(selectedWord, wordToFind))
        {
            Debug.Log("Mot correct !");
            // Actions pour un mot correct
            tourJoueur = false;
            FinDuJeu();
        }
        else //il n'a pas trouvé le mot
        {
            //verifie combien de partie il a fait
            if (compteurEssai > 2) {
                //il n'a plus d'essai et i
                tourJoueur = true;
                FinDuJeu();
            } else {
                Debug.Log(compteurEssai);
                //on appelle la fonction de reset et de l'indice
                ResetLettre();
                AfficheIndice();

            }
            
        }
    }

    //fonction qui reset les lettres
    public void ResetLettre()
    {
        foreach (LetterButton letterButton in allLetterButtons)
        {
            if (letterButton.isSelected)
            {
                letterButton.Deselect();
            }
        }
    }

    //fonction qui affiche un indice
    public void AfficheIndice(){
        switch(compteurEssai){
            case 1:
                //indice 1
                if(MainGameManager.Instance.niveauSelect =="Normal"){
                    MJText.text = "Vous n'avez pas donné le bon mot, recommencez.\nPour vous aidez, je vous offre un indice.\n<color=orange>Indice 1 : Le mot commence par un E</color>";
                }else{
                    MJText.text = "Vous n'avez pas donné le bon mot, recommencez.\nPour vous aidez, je vous offre un indice.\n<color=orange>Indice 1 : Le mot commence par un P</color>";
                }
                 
                break;
            case 2:
                //indice 2
                if(MainGameManager.Instance.niveauSelect =="Normal"){
                    MJText.text = "Vous n'avez pas donné le bon mot, recommencez.\nPour vous aidez, je vous offre un indice.\n<color=orange>Indice 2 : Le mot commence par un ECO</color>";
                }else{
                    MJText.text = "Vous n'avez pas donné le bon mot, recommencez.\nPour vous aidez, je vous offre un indice.\n<color=orange>Indice 2 : Le mot comporte 12 lettres</color>";
                }
                
                break;
        }
    }
    bool IsWordCorrect(string selectedWord, string wordToFind)
    {
        return selectedWord.OrderBy(c => c).SequenceEqual(wordToFind.OrderBy(c => c));
    }


    IEnumerator ShowAndHideGagneText()
    {
        gagnePerduText.gameObject.SetActive(true); // Affiche le texte
        gagnePerduText.text = "vous avez gagné !";
        // Change la couleur du texte en vert
        gagnePerduText.color = Color.green;
        yield return new WaitForSeconds(1f);  // Attend 1 seconde
    }
    IEnumerator ShowAndHidePerduText()
    {
        gagnePerduText.gameObject.SetActive(true); // Affiche le texte
        gagnePerduText.text = "vous avez perdu !";
        // Change la couleur du texte en vert
        gagnePerduText.color = Color.red;
        yield return new WaitForSeconds(1f);  // Attend 1 seconde
    }


    //fin du jeu 
    private void FinDuJeu(){
        ////debug.Log("GameOver");
        buttonTextEnigmes.SetActive(false);
        textEnigmeInfo.gameObject.SetActive(false);
        finDuJeu = true;
        panelInfoMJ.SetActive(true);
        //si c'est tourJoueur = false alors le player a gagné
        if (!tourJoueur) {
            MJText.text = "Bravo le mot était bien Ecosystème, vous avez remporté deux recommandations";
            //envoi vers le Main Game Manager le scoreEnigme
            MainGameManager.Instance.UpdateScore(MainGameManager.Instance.scoreRecoEnigmes+= 2);
            StartCoroutine(ShowAndHideGagneText());
        }
        else {
            MJText.text = "Maître du jeu : Vous avez échoué, je détruis les deux recommandations";
            StartCoroutine(ShowAndHidePerduText());
        }
        MainGameManager.Instance.nbPartieEnigmesJoue += 1;
        
        
            MainGameManager.Instance.gameEnigmesFait = true;
            StartCoroutine(LoadSceneAfterDelay("SalleEnigmes", 4f));

    }
    // Coroutine pour charger la scène après un délai
    private IEnumerator LoadSceneAfterDelay(string sceneName, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene(sceneName);
    }
}

