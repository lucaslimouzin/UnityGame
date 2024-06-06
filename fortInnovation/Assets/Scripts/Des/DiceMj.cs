using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceMj : MonoBehaviour
{
    static Rigidbody rb;
    public GameObject panelInstructions;
    public GameObject panelTirageDesDes;
    public TextMeshProUGUI texteQuiCommence;
    public static Vector3 diceMjVelocity;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    void Update(){
        
    }
    
    public void LancerDesMj(){
        panelTirageDesDes.SetActive(false);
        MainGameManager.Instance.checkFaitDesPlayer = false;
        MainGameManager.Instance.checkFaitDesMj = false;
        MainGameManager.Instance.scoreDesMj = 0;
        MainGameManager.Instance.scoreDesPlayer = 0;
       
        diceMjVelocity = rb.velocity;
        float dirX = Random.Range(0, 200);
        float dirY = Random.Range(0, 200);
        float dirZ = Random.Range(0, 200);

        transform.position = new Vector3(-0.65f, 3.68f, -4.31f);
        transform.rotation = Quaternion.identity;

        rb.freezeRotation = false; // Allow rotation
        rb.angularDrag = 0.5f; // Adjust as needed

        rb.AddForce(transform.up * 200f);
        rb.AddForce(dirX, dirY, dirZ);
        rb.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
     Invoke("determQuiCommenceMj", 5f);
    
    }

    public void determQuiCommenceMj () {
        if (MainGameManager.Instance.scoreDesMj > MainGameManager.Instance.scoreDesPlayer) {
            texteQuiCommence.text = "Le Maître du jeu a réalisé le score le plus élevé, c'est à lui de commencer.";
            MainGameManager.Instance.quiCommence = "Mj";
            panelInstructions.SetActive(true);
        } 
        if(MainGameManager.Instance.scoreDesMj == MainGameManager.Instance.scoreDesPlayer){
            panelTirageDesDes.SetActive(true);
            }
        if (MainGameManager.Instance.scoreDesMj < MainGameManager.Instance.scoreDesPlayer) {
            texteQuiCommence.text = "Vous avez réalisé le score le plus élevé, c'est donc à vous de commencer.";
            MainGameManager.Instance.quiCommence = "Player";
            panelInstructions.SetActive(true);
        }
    }
}
