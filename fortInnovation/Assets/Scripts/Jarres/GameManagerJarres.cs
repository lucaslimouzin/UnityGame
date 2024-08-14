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
public class GameManagerPaires : MonoBehaviour
{
 

    // ajout v2
    public Image imageScore;
    public GameObject panelComplémentReponse;
    public TextMeshProUGUI questionTextReponse;
    public TextMeshProUGUI textComplementBonneReponse;
    public TextMeshProUGUI textComplementReponse;
    
    public TextMeshProUGUI boutonTextReponse;
    private string nomDoc;
    //fin ajout v2

    public GameObject panelJarreHead;
    public Sprite[] headCharacter;
    public Image headAffiche;
    public GameObject panelInfoMJ;
    public TextMeshProUGUI MJText;
  
    public bool tourJoueur = false;
    public bool deuxCartesRetournees = false;
    private bool phaseQuestion = false;
    private bool finDuJeu = false;
    public bool isMoving = false;

   //variables pour les questions//////////////////////
   // URL du fichier JSON sur le réseau
    private string jsonURL = "https://givrosgaming.fr/fortInnov/BatonQuestions.json";
    public GameObject panelQuestions;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI propositionAtext;
    public TextMeshProUGUI propositionBtext;
    public TextMeshProUGUI propositionCtext;
    public Button buttonA;
    public Button buttonB;
    public Button buttonC;
    public Sprite[] imagesButton;
    public GameObject buttonFermer;
    public TextMeshProUGUI gagnePerduText;
    private bool aJuste = false;
    private int numQuestions = 0;

    public List<Button> ligne1Cartes; // Liste pour stocker les cartes de la ligne 1
    public List<Button> ligne2Cartes; // Liste pour stocker les cartes de la ligne 2
    public Sprite cardDos; // Image pour le dos de la carte
    public Sprite cardChat, cardEtoile, cardNuage, cardPlanete, cardSoleil; // Images pour les faces des cartes
    
    private string[] ligne1Refs;
    private string[] ligne2Refs;
    private string[] typesDeCartes = { "chat", "etoile", "nuage", "planete", "soleil" }; // Les types de cartes
    private Button carteSelectionneeLigne1;
    private Button carteSelectionneeLigne2;
    private Dictionary<int, string> memoireIA = new Dictionary<int, string>();
    private HashSet<int> cartesVuesNonAppariees = new HashSet<int>();
    private int premiereCarteIndexIA = -1; // Ajoutez cette variable pour stocker l'index de la première carte choisie par l'IA
    private int toursIA = 0;
    public TextMeshProUGUI scorePlayerText;
    public TextMeshProUGUI scoreMjText;
    public TextMeshProUGUI trueWrongText;
    public int scoreMj = 0;
    public int scorePlayer = 0;
    

    [System.Serializable]
    public class QuestionData
    {
        public int idQuestion;
        public string question;
        public string[] propositions;
        public string reponseCorrecte;
        public string complementReponse;
    }

    [System.Serializable]
    public class Questions
    {
        public QuestionData[] questions;
    }

    private Questions listQuestions; 

    //fin variables pour les questions ////////////////////

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
        //ajout v2
         if(MainGameManager.Instance.niveauSelect =="Normal"){
            imageScore.sprite= MainGameManager.Instance.imageScore[0];
        }else{
            imageScore.sprite= MainGameManager.Instance.imageScore[1];
        }
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
        gagnePerduText.gameObject.SetActive(false); // Masque le texte
        toursIA = 0;
        scoreMj = 0;
        scorePlayer = 0;
        trueWrongText.gameObject.SetActive(false); // Masque le texte

        //charge la coroutine qui va récupérer le fichier Json 
        //StartCoroutine(LoadJsonFromNetwork()); //a activer lors du déploiment
        //charge la coroutine qui va récupérer le fichier Json 
        StartCoroutine(LoadJsonFromLocal());
        phaseQuestion = true;

        //on mélange les cartes
        MelangerListe(ligne1Cartes);
        MelangerListe(ligne2Cartes);
        InitialiserCartesFaceCachee();
        AssigneClicAuxBoutons();
        // Initialiser et mélanger les tableaux de références
        InitialiserEtMelangerRefs();
        //on affiche le panneau des régles
        //PanneauRegle();
        RetraitPanneauRegle();
    }

    //fonction qui charge les questions depuis local
    IEnumerator LoadJsonFromLocal(){
        //ajout v2
        if(MainGameManager.Instance.niveauSelect =="Normal"){
            nomDoc = "PaireQuestions";
        }else{
            nomDoc = "PaireQuestionsFacile";//Mode facile
        }
        // Charger le fichier JSON Normal
        TextAsset jsonFile = Resources.Load<TextAsset>(nomDoc);
        // Désérialiser les données JSON
        listQuestions = JsonUtility.FromJson<Questions>(jsonFile.ToString());
        yield return null;
    }
    //fonction qui charge les questions depuis le réseau
    IEnumerator LoadJsonFromNetwork()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(jsonURL))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Erreur lors du chargement du fichier JSON depuis le réseau: " + www.error);
            }
            else
            {
                // Les données JSON ont été téléchargées avec succès
                string jsonText = www.downloadHandler.text;

                // Désérialiser les données JSON
                listQuestions = JsonUtility.FromJson<Questions>(jsonText);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    
    //affichage du panneau des règles
    // private void PanneauRegle (){
    //     panelInstruction.SetActive(true);
    // }
    
    // retrait panneau des règles
    //affichage du panneau de la règle
    public void RetraitPanneauRegle (){
       // panelInstruction.SetActive(false);
       if (MainGameManager.Instance.quiCommence == "Player"){
            //On affiche la question
            Invoke("AfficheLaQuestion",0f);
            //tour du player     
        }
        else {
            panelInfoMJ.SetActive(true);
           // tourJoueur = false;
           tourJoueur = true;
            MJText.text = "Le Maître du jeu commence à jouer !";
            TourDuMj();
        } 
    }

    

    //affichage de la question   
    private void AfficheLaQuestion(){
        phaseQuestion = true;
        deuxCartesRetournees = false;
        //Debug.Log("PhaseQ 2 = " + phaseQuestion);
        tourJoueur = false;
        //choisi les questions de 1 à taille Json
        //comme on vise un tableau on est obligé de commencer à 0
        //et pour range on va jusqu'à taille Json
        do
        {
            numQuestions = UnityEngine.Random.Range(0, listQuestions.questions.Length);
        } while (MainGameManager.Instance.questionsPairesPosees.Contains(numQuestions));

        MainGameManager.Instance.questionsPairesPosees.Add(numQuestions);
        // Restreindre le nombre total de questions posées
        if (MainGameManager.Instance.questionsPairesPosees.Count >= listQuestions.questions.Length)
        {
            // Si toutes les questions ont été posées, réinitalisation de la liste
            MainGameManager.Instance.questionsPairesPosees.Clear();
        }

        //gestion des panneaux
        // if (panelInstruction.activeSelf){
        //     panelInstruction.SetActive(false);
        // }
        if (panelInfoMJ.activeSelf){
            panelInfoMJ.SetActive(false);
        }
        panelQuestions.SetActive(true);
        panelJarreHead.SetActive(false);

        QuestionData question = listQuestions.questions[numQuestions];
        //affichage des données
        questionText.text = question.question;
        propositionAtext.text = question.propositions[0];
        propositionBtext.text = question.propositions[1];
        propositionCtext.text = question.propositions[2];

        //ajout v2
        if (MainGameManager.Instance.niveauSelect == "Facile"){
            questionTextReponse.text = question.question;
             //on recolor juste la réponse juste
            switch (question.reponseCorrecte)
            {
                case "A":
                    textComplementBonneReponse.text = question.propositions[0];
                    break;
                case "B":
                    textComplementBonneReponse.text = question.propositions[1];
                    break;
                case "C":
                    textComplementBonneReponse.text = question.propositions[2];
                    break;
            }
            
            textComplementReponse.text = question.complementReponse;
        }
        
        //met le fond des boutons en noir
        buttonA.image.sprite = imagesButton[0]; // "black"
        buttonB.image.sprite = imagesButton[0]; // "black"
        buttonC.image.sprite = imagesButton[0]; // "black"
        //desactive le boutonFermer
        buttonFermer.SetActive(false);
                    
                
        if (Input.GetKey(KeyCode.Space)) {

        } else {
            buttonA.onClick.AddListener(() => OnButtonClick("A", question.reponseCorrecte));
            buttonB.onClick.AddListener(() => OnButtonClick("B", question.reponseCorrecte));
            buttonC.onClick.AddListener(() => OnButtonClick("C", question.reponseCorrecte));
        }
        
        
    }

    //fonction qui check le bouton enfoncé
    private void OnButtonClick(String choix, string reponseCorrecte){
        // Supprimer tous les écouteurs d'événements du bouton
        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.RemoveAllListeners();
        buttonC.onClick.RemoveAllListeners();

        if (choix == reponseCorrecte){
            aJuste = true;
            //on met tout en rouge 
            buttonA.image.sprite = imagesButton[1]; // "red"
            buttonB.image.sprite = imagesButton[1]; // "red"
            buttonC.image.sprite = imagesButton[1]; // "red"
            // Mettre en vert le texte qu'il a cliqué
            switch (choix)
            {
                case "A":
                    buttonA.image.sprite = imagesButton[2]; // "green"
                    break;
                case "B":
                    buttonB.image.sprite = imagesButton[2]; // "green"
                    break;
                case "C":
                    buttonC.image.sprite = imagesButton[2]; // "green"
                    break;
            }
        }
        else {
            aJuste = false;
            // Mettre en rouge le texte qu'il a cliqué et en vert le texte de la bonne réponse
            //on met tout en rouge 
            buttonA.image.sprite = imagesButton[1]; // "red"
            buttonB.image.sprite = imagesButton[1]; // "red"
            buttonC.image.sprite = imagesButton[1]; // "red"
            
            //on recolor juste la réponse juste
            switch (reponseCorrecte)
            {
                case "A":
                    buttonA.image.sprite = imagesButton[2]; // "green"
                    break;
                case "B":
                    buttonB.image.sprite = imagesButton[2]; // "green"
                    break;
                case "C":
                    buttonC.image.sprite = imagesButton[2]; // "green"
                    break;
            }
            
        }
        // affiche le bouton fermer & v2
        if (MainGameManager.Instance.niveauSelect == "Facile"){
            boutonTextReponse.text = "Suivant";
        }
        buttonFermer.SetActive(true);
        
        
    }
    //action du bouton fermer v2
    public void clicBoutonPanelReponse(){
        panelComplémentReponse.SetActive(false);
        panelJarreHead.SetActive(true);
        RetraitPanneauQuestions(aJuste);
        
    }

    //action du bouton fermer
    public void clicBoutonPanelQuestion(){
        //si on a choisi le mode Facile v2
        if (MainGameManager.Instance.niveauSelect == "Facile"){
            panelComplémentReponse.SetActive(true);
        }else {
            panelJarreHead.SetActive(true);
            RetraitPanneauQuestions(aJuste);
        }
        
    }
   
    //retrait panneau Question
    private void RetraitPanneauQuestions(bool reponseJuste){

        panelQuestions.SetActive(false);
        panelInfoMJ.SetActive(true);
        if(reponseJuste){
            tourJoueur= true;
            phaseQuestion = false;
            MJText.text = "Vous avez bien répondu ! \nVous pouvez retourner deux cartes.";
            TourDuJoueur();
        } 
        else {
            tourJoueur = false;
            MJText.text = "Vous n'avez pas donné la bonne réponse...\nLe Maître du jeu va retourner deux cartes.";
            TourDuMj();
        }
        
    }

    private void InitialiserCartesFaceCachee()
    {
        foreach (var carte in ligne1Cartes)
        {
            SetCardFace(carte, cardDos);
        }
        foreach (var carte in ligne2Cartes)
        {
            SetCardFace(carte, cardDos);
        }
    }
    // Méthode pour changer l'image d'une carte
    private void SetCardFace(Button carteButton, Sprite face)
    {
        carteButton.GetComponent<Image>().sprite = face;
    }

    // Méthode pour mélanger une liste de cartes
    private void MelangerListe<T>(List<T> liste)
    {
        System.Random rnd = new System.Random();
        int n = liste.Count;
        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            T valeur = liste[k];
            liste[k] = liste[n];
            liste[n] = valeur;
        }
    }

    private void InitialiserEtMelangerRefs()
    {
        // Créer et remplir les tableaux de références
        ligne1Refs = new string[typesDeCartes.Length];
        ligne2Refs = new string[typesDeCartes.Length];
        for (int i = 0; i < typesDeCartes.Length; i++)
        {
            ligne1Refs[i] = typesDeCartes[i];
            ligne2Refs[i] = typesDeCartes[i];
        }

        // Mélanger les tableaux
        MelangerTableau(ligne1Refs);
        MelangerTableau(ligne2Refs);
    }

    // Méthode pour mélanger un tableau
    private void MelangerTableau<T>(T[] tableau)
    {
        System.Random rnd = new System.Random();
        int n = tableau.Length;
        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1);
            T valeur = tableau[k];
            tableau[k] = tableau[n];
            tableau[n] = valeur;
        }
    }

    private void AssigneClicAuxBoutons(){
        // Assigner les méthodes de clic aux boutons
        for (int i = 0; i < ligne1Cartes.Count; i++)
        {
            int index = i; // Important pour capturer la valeur actuelle de i dans la closure
            ligne1Cartes[i].onClick.AddListener(() => OnCardClicked(index, true));
        }
        for (int i = 0; i < ligne2Cartes.Count; i++)
        {
            int index = i;
            ligne2Cartes[i].onClick.AddListener(() => OnCardClicked(index, false));
        }
    }
    private void OnCardClicked(int index, bool isLigne1)
    {
        // Si c'est le tour du joueur et si une carte n'a pas déjà été sélectionnée dans la ligne
        if (tourJoueur && !deuxCartesRetournees && ((isLigne1 && carteSelectionneeLigne1 == null) || (!isLigne1 && carteSelectionneeLigne2 == null)))
        {
            Button carteCliquee = isLigne1 ? ligne1Cartes[index] : ligne2Cartes[index];
            string refCarte = isLigne1 ? ligne1Refs[index] : ligne2Refs[index];
            Sprite nouvelleImage = TrouverSprite(refCarte);

            carteCliquee.GetComponent<Image>().sprite = nouvelleImage;

            if (isLigne1)
            {
                carteSelectionneeLigne1 = carteCliquee;
                carteSelectionneeLigne1.interactable = false;
                
            }
            else
            {
                carteSelectionneeLigne2 = carteCliquee;
                carteSelectionneeLigne2.interactable = false;
            }

            VerifierSiDeuxCartesSelectionnees();
        }
        else {
            Debug.Log("erreur");
            Debug.Log(tourJoueur);
            Debug.Log(isLigne1);
        }
    }
    private void VerifierSiDeuxCartesSelectionnees()
    {

        if (carteSelectionneeLigne1 != null && carteSelectionneeLigne2 != null)
        {
            deuxCartesRetournees = true;
            int indexCarte1 = ligne1Cartes.IndexOf(carteSelectionneeLigne1);
            int indexCarte2 = ligne2Cartes.IndexOf(carteSelectionneeLigne2);
            if (ligne1Refs[indexCarte1] == ligne2Refs[indexCarte2])
            {
                // Les cartes forment une paire, les laisser retournées et désactiver le clic
                carteSelectionneeLigne1.interactable = false;
                carteSelectionneeLigne2.interactable = false;

                // Réinitialiser les cartes sélectionnées pour le prochain tour
                carteSelectionneeLigne1 = null;
                carteSelectionneeLigne2 = null;
                //attribution des points de score 
                StartCoroutine(ShowAndHideTrueText());
                if(tourJoueur){
                    scorePlayer++;
                    scorePlayerText.text = "Nombre de paires : \n" + scorePlayer.ToString()+ "/3" ;
                }
                else {
                    scoreMj++;
                    scoreMjText.text = "Nombre de paires : \n" + scoreMj.ToString() + "/3";
                }
                if (scorePlayer == 3 || scoreMj ==3) {
                    //fin du jeu
                    FinDuJeu();
                }else {
                    Invoke("AfficheLaQuestion",1.5f);
                }
                
            }
            else
            {
                // Les cartes ne forment pas une paire
                // Ajouter les index des cartes aux cartes vues non appariées
                StartCoroutine(ShowAndHideWrongText());
                cartesVuesNonAppariees.Add(indexCarte1);
                cartesVuesNonAppariees.Add(indexCarte2);
                // Les cartes ne forment pas une paire, planifier pour les retourner face cachée
                StartCoroutine(RetournerCartes());
            }
        }
    }

    IEnumerator ShowAndHideTrueText()
    {
        trueWrongText.gameObject.SetActive(true); // Affiche le texte
        trueWrongText.text = "paire trouvée";
        // Change la couleur du texte en vert
        trueWrongText.color = Color.green;
        yield return new WaitForSeconds(1f);  // Attend 1 seconde
        trueWrongText.gameObject.SetActive(false); // Masque le texte
    }
    IEnumerator ShowAndHideWrongText()
    {
        trueWrongText.gameObject.SetActive(true); // Affiche le texte
        trueWrongText.text = "paire non trouvée";
        // Change la couleur du texte en vert
        trueWrongText.color = Color.red;
        yield return new WaitForSeconds(1f);  // Attend 1 seconde
        trueWrongText.gameObject.SetActive(false); // Masque le texte
    }


    private IEnumerator RetournerCartes()
    {
        // Attendre un court délai avant de retourner les cartes
        yield return new WaitForSeconds(2);

        // Retourner les cartes face cachée 
        carteSelectionneeLigne1.GetComponent<Image>().sprite = cardDos;
        carteSelectionneeLigne2.GetComponent<Image>().sprite = cardDos;

        //rend les cartes non interactable
        carteSelectionneeLigne1.interactable = true;
        carteSelectionneeLigne2.interactable = true;
        // Réinitialiser les cartes sélectionnées
        carteSelectionneeLigne1 = null;
        carteSelectionneeLigne2 = null;

        //affiche la prochaine question
        Invoke("AfficheLaQuestion",1f);
    }

    private Sprite TrouverSprite(string refCarte)
    {
        switch (refCarte)
        {
            case "chat": return cardChat;
            case "etoile": return cardEtoile;
            case "nuage": return cardNuage;
            case "planete": return cardPlanete;
            case "soleil": return cardSoleil;
            default: return cardDos; // Ou gérer une erreur
        }
    }





    private void TourDuJoueur(){
        
        tourJoueur = true;
        if(tourJoueur) {

        }  
    }

    private void TourDuMj()
    {
        tourJoueur = false;
        
        StartCoroutine(RetournerCartesIACoroutine());
    }

    private int ChoisirCarteIA(List<Button> cartes)
    {
        int index;

        // Choisissez une carte aléatoirement parmi les cartes interactables
        List<Button> cartesInteractables = cartes.FindAll(carte => carte.interactable);

        if (cartesInteractables.Count > 0)
        {
            index = UnityEngine.Random.Range(0, cartesInteractables.Count);
            return cartes.IndexOf(cartesInteractables[index]);
        }
        else
        {
            // Si aucune carte n'est interactable, retournez -1 pour indiquer qu'aucune carte valide n'a été trouvée
            return -1;
        }
    }







    private IEnumerator RetournerCartesIACoroutine()
    {
        int carteLigne1Index = ChoisirCarteIA(ligne1Cartes);
        RetournerCarteIA(carteLigne1Index, true);
        yield return new WaitForSeconds(1);

        // Initialiser carteSelectionneeLigne1 avec la carte choisie par l'IA
        carteSelectionneeLigne1 = ligne1Cartes[carteLigne1Index];

        // Trouvez la référence de la deuxième carte directement dans ligne2Refs
        string refDeuxiemeCarte = ligne1Refs[carteLigne1Index];
        int carteLigne2Index = -1;

        for (int i = 0; i < ligne2Refs.Length; i++)
        {
            if (ligne2Refs[i] == refDeuxiemeCarte && ligne2Cartes[i].interactable)
            {
                carteLigne2Index = i;
                break;
            }
        }

        if (carteLigne2Index != -1)
        {
            RetournerCarteIA(carteLigne2Index, false);
            // Initialiser carteSelectionneeLigne2 avec la deuxième carte choisie par l'IA
            carteSelectionneeLigne2 = ligne2Cartes[carteLigne2Index];
        }

        yield return new WaitForSeconds(1);

        VerifierSiDeuxCartesSelectionnees();
    }



    private void RetournerCarteIA(int index, bool isLigne1)
    {
        Button carte = isLigne1 ? ligne1Cartes[index] : ligne2Cartes[index];
        string refCarte = isLigne1 ? ligne1Refs[index] : ligne2Refs[index];
        Sprite nouvelleImage = TrouverSprite(refCarte);

        carte.GetComponent<Image>().sprite = nouvelleImage;
        carte.interactable = false;
    }

    IEnumerator ShowAndHideGagneText()
    {
        yield return new WaitForSeconds(2f);  // Attend 1 seconde
        gagnePerduText.gameObject.SetActive(true); // Affiche le texte
        gagnePerduText.text = "vous avez gagné !";
        // Change la couleur du texte en vert
        gagnePerduText.color = Color.green;
        yield return new WaitForSeconds(1f);  // Attend 1 seconde
    }
    IEnumerator ShowAndHidePerduText()
    {
        yield return new WaitForSeconds(2f);  // Attend 1 seconde
        gagnePerduText.gameObject.SetActive(true); // Affiche le texte
        gagnePerduText.text = "vous avez perdu !";
        // Change la couleur du texte en vert
        gagnePerduText.color = Color.red;
        yield return new WaitForSeconds(1f);  // Attend 1 seconde
    }

    //fin du jeu 
    private void FinDuJeu(){
        ////debug.Log("GameOver");
        finDuJeu = true;
        //si c'est tourJoueur = false alors le player a gagné
        if (tourJoueur) {
            //ajout v2
            if(MainGameManager.Instance.niveauSelect =="Normal"){
                MJText.text = "Bravo vous avez remporté une recommandation";
            }else{
                MJText.text = "Bravo vous avez remporté le duel";
            }
            
            //envoi vers le Main Game Manager le scorePaires

            //ajout v2 choix mode de jeu
            if(MainGameManager.Instance.niveauSelect =="Normal"){
                MainGameManager.Instance.UpdateScore(MainGameManager.Instance.scoreRecoPaires+= 2);
            }else{
                MainGameManager.Instance.UpdateScore(MainGameManager.Instance.scoreRecoPaires+= 1);
            } 
            
            if (MainGameManager.Instance.scoreRecoPaires == 6) {
                MainGameManager.Instance.UpdateScore(MainGameManager.Instance.scoreRecoPaires-= 1);
            }
            StartCoroutine(ShowAndHideGagneText());
        }
        else {
            if(MainGameManager.Instance.niveauSelect =="Normal"){
                MJText.text = "Vous avez échoué, je détruis une recommandation";
            }else{
                MJText.text = "Vous avez échoué ce duel";
            }
            StartCoroutine(ShowAndHidePerduText());
        }
        MainGameManager.Instance.nbPartiePairesJoue += 1;
        
        if(MainGameManager.Instance.nbPartiePairesJoue > MainGameManager.Instance.nbPartiePaires ){
            MainGameManager.Instance.gamePairesFait = true;
            StartCoroutine(LoadSceneAfterDelay("SallePaires", 4f));
        }
        else
        {
            StartCoroutine(LoadSceneAfterDelay("SalleDes", 4f));
        }

    }
    // Coroutine pour charger la scène après un délai
    private IEnumerator LoadSceneAfterDelay(string sceneName, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene(sceneName);
    }
}
