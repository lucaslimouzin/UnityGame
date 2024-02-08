using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DoorActions4 : MonoBehaviour
{
     public GameObject panelDoor;
     public GameObject objetADeplacer;
    public float distanceDuDeplacement = 5f; // Distance à déplacer sur l'axe Y
    public float dureeDuDeplacement = 3f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //Cursor.lockState = CursorLockMode.Locked;
        #if !UNITY_EDITOR && UNITY_WEBGL
            // disable WebGLInput.stickyCursorLock so if the browser unlocks the cursor (with the ESC key) the cursor will unlock in Unity
            WebGLInput.stickyCursorLock = true;
        #endif
    }

    private void OnTriggerEnter(Collider other) {
        if (MainGameManager.Instance.tutoCompteur == 3) {
            if (other.gameObject.CompareTag("Player")){
                panelDoor.SetActive(true);
                //Set Cursor to not be visible
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        } 
    }

    private void OnTriggerExit(Collider other) {
        if (MainGameManager.Instance.tutoCompteur == 3) {
            if (other.gameObject.CompareTag("Player")){
                if (panelDoor.activeSelf){
                    panelDoor.SetActive(false);
                    //Set Cursor to not be visible
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }
    }

     public void PlayGameBaton() {
        panelDoor.SetActive(false);
        //Set Cursor to not be visible
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //mettre un delai
        StartCoroutine(DeplacerGameObject(objetADeplacer));
        Invoke("ChangerDeScene", 2f);
        
    }

    private void ChangerDeScene(){
        SceneManager.LoadScene("SalleClous");
    }


    private IEnumerator DeplacerGameObject(GameObject objet)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(0f, distanceDuDeplacement, 0f);
        float elapsedTime = 0f;

        while (elapsedTime < dureeDuDeplacement)
        {
            // Interpoler la position entre le point de départ et la position cible
            objet.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / dureeDuDeplacement);

            // Mettre à jour le temps écoulé
            elapsedTime += Time.deltaTime;

            // Attendre la prochaine trame
            yield return null;
        }

        // Assurez-vous que l'objet soit bien positionné à la position cible à la fin
        objet.transform.position = targetPosition;

       
    }




}
