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
    private bool finDuJeu = false;
    public bool isMoving = false;


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
       if (MainGameManager.Instance.quiCommence == "Player"){
            //tour du player
            TourDuJoueur();
            
        }
        else {
            panelInfoMJ.SetActive(true);
            tourJoueur = false;
            TourDuMj();
        } 
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

    private void MoveClou (float currentHP){
        // Assurez-vous que votre objet clou existe
    if (clou != null)
    {   float minY = 0.926f;
        float maxY = 1.498f;
        float maxHP = 1000;
        // Calculez la position Y en utilisant une règle de trois
        float normalizedY = Mathf.Lerp(minY, maxY, currentHP / maxHP);

        // Récupérez la position actuelle du clou
        Vector3 newPosition = clou.transform.position;

        //si le clou dépasse la zone cible alors on le met au minY
        if (currentHP <= 0){
            normalizedY = minY;
        }
        // Modifiez la position Y avec la valeur souhaitée
        newPosition.y = normalizedY;

        // Appliquez la nouvelle position au clou
        clou.transform.position = newPosition;
    }
    else
    {
        Debug.LogError("Clou n'est pas défini. Assurez-vous de le définir correctement.");
    }
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
                //on descend la vie du clou 
                vieDuClou -= playerForce; 
                MoveClou(vieDuClou);
            }
            //faire tourner le marteau du Mj
            else {
                while (currentEulerAnglesMj.z > -70f ){
                    z = -0.05f * mjForce;
                    currentEulerAnglesMj += new Vector3(0,0,z) * Time.deltaTime * rotationSpeed;
                    pivotPointMarteauMj.localEulerAngles = currentEulerAnglesMj;
                    yield return null; //Attendre la prochaine trame
                }
                //on descend la vie du clou
                vieDuClou -= mjForce; 
                MoveClou(vieDuClou);
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
            //envoi vers le Main Game Manager le scoreClou 
                MainGameManager.Instance.UpdateScore(MainGameManager.Instance.scoreRecoClou+= 1);
        }
        else {
            MJText.text = "Maître du jeu : Vous avez échoué, je détruis une recommandation";
        }
        MainGameManager.Instance.nbPartieClouJoue += 1;
        
        if(MainGameManager.Instance.nbPartieClouJoue == 3 ){
            MainGameManager.Instance.gameClouFait = true;
            StartCoroutine(LoadSceneAfterDelay("ClouEnfonce", 2f));
        }
        else
        {
            StartCoroutine(LoadSceneAfterDelay("salleDes", 2f));
        }

    }
    // Coroutine pour charger la scène après un délai
    private IEnumerator LoadSceneAfterDelay(string sceneName, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene(sceneName);
    }
}
