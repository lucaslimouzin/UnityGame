using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManagerMarteau : MonoBehaviour
{
    public GameObject clou; 
    public GameObject marteauPlayer;
    public GameObject marteauMj;
    public GameObject panelInstruction;
    public GameObject panelInfoMJ;
    public GameObject panelJauge;
    public GameObject buttonTextMarteau;
    public TextMeshProUGUI MJText;
    public Button buttonJauge;
    public Transform pivotPointMarteauPlayer;
    public Transform pivotPointMarteauMj;
    float rotationSpeed = 60;
    Vector3 currentEulerAnglesPlayer;
    Vector3 currentEulerAnglesMj;
    float z;
    public float playerForce;
    public Slider forceMarteau;
    private bool up =false;
    private bool tourJoueur = true;
    private bool aRelacher = false;
    private float vieDuClou;
    private bool aJuste = false;
    private bool win = false;
    private bool firstTimeMj;
    private bool debutDuTour = false;
    private bool finDuJeu = false;
    public bool isMoving = false;


    //--------pour mettre à jour le score --------------------------------------
    private void OnEnable()
    {
        // S'abonner à l'événement OnScoreUpdated
       // MainGameManager.OnScoreUpdated += HandleScoreUpdated;
    }

    private void OnDisable()
    {
        // Se désabonner de l'événement OnScoreUpdated lors de la désactivation du script
       // MainGameManager.OnScoreUpdated -= HandleScoreUpdated;
    }

    // Méthode appelée lorsque le score est mis à jour
    private void HandleScoreUpdated(int newScore)
    {
        // Faire quelque chose avec le nouveau score
        Debug.Log("Nouveau score : " + newScore);
    }
    //-------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        firstTimeMj = false;
        forceMarteau.value = 0;
        playerForce = 0;
        vieDuClou = 800f;
        ResetGauge();
      //on affiche le panneau des régles
        PanneauRegle();
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKey(KeyCode.Space) && !aRelacher && tourJoueur && !finDuJeu){
        
        if (playerForce ==100) {
           up = false; 
        } 
        else if (playerForce == 0) {
            up = true;
        }
        if (up){
            playerForce +=5;
        }
        else {
            playerForce -=5;
        }
        Slider();
       } 
       if (Input.GetKeyUp(KeyCode.Space) && !aRelacher && tourJoueur && !finDuJeu){
        aRelacher = true;
        //relacheMarteau();
        StartCoroutine(Wait());
       }
       
    }

   
    public void Slider() {
        forceMarteau.value = playerForce;
    }

    public void ResetGauge(){
        playerForce = 0;
        forceMarteau.value = 0;
    }
    IEnumerator Wait(){
        yield return new WaitForSeconds(0f);
        aRelacher = true;
        TourDuJoueur();
        
    }


    //affichage du panneau des règles
    private void PanneauRegle (){
        panelInstruction.SetActive(true);
    }
    
    // retrait panneau des règles
    //affichage du panneau de la règle
    public void RetraitPanneauRegle (){
        panelInstruction.SetActive(false);
        TourDuJoueur();
       /* if (MainGameManager.Instance.quiCommence == "Player"){
            //tour du player
            TourDuJoueur();
            
        }
        else {
            panelInfoMJ.SetActive(true);
            firstTimeMj = true;
            MJText.text = "Maitre du jeu : Je commence à retirer des bâtons !";
            TourDuMj();
        } */
        
    }


    //fonction qui check le bouton enfoncé
    private void OnButtonClick(){
        // Supprimer tous les écouteurs d'événements du bouton
        buttonJauge.onClick.RemoveAllListeners();
    }


    private void TourDuJoueur(){
        if(tourJoueur) {
            buttonTextMarteau.SetActive(true);
            Debug.Log("Debut tour joueur");
            MJText.text = "Maître du jeu : A vous de jouer";
            if(aRelacher){
                Invoke("MoveMarteau",1f);
            }
        }  
    }

    private void TourDuMj(){
        if (!tourJoueur) {
            buttonTextMarteau.SetActive(false);
            Debug.Log("Debut tour Mj");
            MJText.text = "Maître du jeu : A mon tour de jouer";
            ResetGauge();
            Invoke("MoveMarteau",1f);
        }        
    }

    private void MoveMarteau(){
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(MoveMarteauCoroutine());
        }
    }

    private void MoveClou (){
        
    }

    private IEnumerator MoveMarteauCoroutine(){
       int mjForce = UnityEngine.Random.Range(1,101);
            //faire tourner le marteau du player
            if (tourJoueur){
                //on attribue la valeur de la Force à Z
                while (currentEulerAnglesPlayer.z < 70f){
                    z = 0.05f * playerForce;
                    currentEulerAnglesPlayer += new Vector3(0,0,z) * Time.deltaTime * rotationSpeed;
                    pivotPointMarteauPlayer.localEulerAngles = currentEulerAnglesPlayer;
                    yield return null; //Attendre la prochaine trame
                }
            }
            //faire tourner le marteau du Mj
            else {
                while (currentEulerAnglesMj.z > -70f ){
                    z = -0.05f * mjForce;
                    currentEulerAnglesMj += new Vector3(0,0,z) * Time.deltaTime * rotationSpeed;
                    pivotPointMarteauMj.localEulerAngles = currentEulerAnglesMj;
                    yield return null; //Attendre la prochaine trame
                }
            }
        //quand c fini replace les marteaux
        //faire remonter le marteau du player
            if (tourJoueur){
                while (currentEulerAnglesPlayer.z > 0f){
                    z = -0.05f * playerForce;
                    currentEulerAnglesPlayer += new Vector3(0,0,z) * Time.deltaTime * rotationSpeed;
                    pivotPointMarteauPlayer.localEulerAngles = currentEulerAnglesPlayer;
                    yield return null; //Attendre la prochaine trame
                }
            }
            //faire remonter le marteau du Mj
            else {
                while (currentEulerAnglesMj.z < -0f ){
                    z = 0.05f * mjForce;
                    currentEulerAnglesMj += new Vector3(0,0,z) * Time.deltaTime * rotationSpeed;
                    pivotPointMarteauMj.localEulerAngles = currentEulerAnglesMj;
                    yield return null ; //Attendre la prochaine trame
                }
            }
            //on attend 2 secondes la fin de la coroutine 
            yield return new WaitForSeconds(1f); 
            isMoving = false;
            Debug.Log(isMoving);
            Debug.Log("Tour joueur : " + tourJoueur);
            if (tourJoueur) {
                tourJoueur = false;
                vieDuClou -= playerForce; 
                Debug.Log(vieDuClou);
                if (vieDuClou <= 0){
                    FinDuJeu();
                } else {
                    TourDuMj()
;                }
                Debug.Log("Tour joueur : " + tourJoueur);
            }
            else {
                tourJoueur = true;
                aRelacher = false;
                vieDuClou -= mjForce; 
                Debug.Log(vieDuClou);
                if (vieDuClou <= 0){
                    FinDuJeu();
                }
                else {
                    TourDuJoueur();
                }
            }
            
    }
    
    //fin du jeu 
    private void FinDuJeu(){
        Debug.Log("GameOver");
        buttonTextMarteau.SetActive(false);
        finDuJeu = true;
        //si c'est tourJoueur = false alors le player a gagné
        if (!tourJoueur) {
            MJText.text = "Maître du jeu : Bravo vous avez remporté l'épreuve et une recommandation";
        }
        else {
            MJText.text = "Maître du jeu : Vous avez échoué, je détruis une recommandation";
        }
        /*
        if(MainGameManager.Instance.nbPartieClouJoue == 3 ){
            MainGameManager.Instance.gameClouFait = true;
            SceneManager.LoadScene("ClouEnfonce");
        }
        else {
            SceneManager.LoadScene("salleDes");
        }
        */
    }
}
