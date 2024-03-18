using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chestClou : MonoBehaviour
{
    public GameObject panelReco;
    public GameObject panelReco1;
    public GameObject panelReco2;
    public GameObject panelReco3;
    public GameObject panelReco4;
    public GameObject panelReco5;
    public GameObject[] buttonCadenas;
    public Sprite unlockSprite;
    // Start is called before the first frame update
    void Start()
    {
        ActivateButton(MainGameManager.Instance.scoreRecoClou);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
    }

    public void OpenReco2() {
        panelReco2.SetActive(true);
    }

    public void OpenReco3() {
        panelReco3.SetActive(true);
    }

    public void OpenReco4() {
        panelReco4.SetActive(true);
    }

    public void OpenReco5() {
        panelReco5.SetActive(true);
    }
}
