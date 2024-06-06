using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chestbassin : MonoBehaviour
{
    public GameObject panelReco;
    public GameObject panelReco1;
    public GameObject panelReco2;
    public GameObject panelReco3;
    public GameObject panelReco4;
    public GameObject panelReco5;
    public GameObject[] buttonCadenas;
    public Sprite unlockSprite;
    public Sprite whiteSprite;
    public GameObject buttonClose;
    private int nbReco;
    private bool openReco1;
    private bool openReco2;
    private bool openReco3;
    private bool openReco4;
    private bool openReco5;
    // Start is called before the first frame update
    void Start()
    {
        ActivateButton(MainGameManager.Instance.scoreRecobassin);
        buttonClose.SetActive(false);
        nbReco = 0;
        openReco1 = openReco2 = openReco3 = openReco4 = openReco5 = false;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (nbReco == MainGameManager.Instance.scoreRecobassin){
            buttonClose.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
                panelReco.SetActive(true);
                
                
                 
        }
    }

    private void OnTriggerExit(Collider other) {
        if (panelReco.activeSelf){
            panelReco.SetActive(false);
            
            
            
        }
    }

    private void ActivateButton(int score){
        for (int i = 0; i < score; i++){
            // Accéder au composant Button du GameObject
            Button button = buttonCadenas[i].GetComponent<Button>();
            // Accéder au composant Image du GameObject
            Image image = buttonCadenas[i].GetComponent<Image>();
            button.interactable = true;
            image.sprite = unlockSprite;
        }
    }

  //ouverture des reco
    public void OpenReco1() {
        panelReco1.SetActive(true);
        Image image = buttonCadenas[0].GetComponent<Image>();
        image.sprite = whiteSprite;
        if (!openReco1) {
            nbReco +=1;
            openReco1 = true;
        }       
    }

    public void OpenReco2() {
        panelReco2.SetActive(true);
        Image image = buttonCadenas[1].GetComponent<Image>();
        image.sprite = whiteSprite;
        if (!openReco2) {
            nbReco +=1;
            openReco2 = true;
        }
    }

    public void OpenReco3() {
        panelReco3.SetActive(true);
        Image image = buttonCadenas[2].GetComponent<Image>();
        image.sprite = whiteSprite;
        if (!openReco3) {
            nbReco +=1;
            openReco3 = true;
        }
    }

    public void OpenReco4() {
        panelReco4.SetActive(true);
        Image image = buttonCadenas[3].GetComponent<Image>();
        image.sprite = whiteSprite;
        if (!openReco4) {
            nbReco +=1;
            openReco4 = true;
        }
    }

    public void OpenReco5() {
        panelReco5.SetActive(true);
        Image image = buttonCadenas[4].GetComponent<Image>();
        image.sprite = whiteSprite;
        if (!openReco5) {
            nbReco +=1;
            openReco5 = true;
        }
    }
}
