using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class CollisionDetection : MonoBehaviour
{
    public GameObject consigneUI;
    public GameObject consigneDoor;
    private string input;
    public TMP_InputField champSaisie;

    private void Start() {
        
        
    }

    private void OnTriggerEnter(Collider collision) {
        
        if (collision.gameObject.CompareTag("Book")) {
            // Activez l'interface UI consigne
            if (consigneUI != null)
            {
                consigneUI.SetActive(true);
            }
        }
        if (collision.gameObject.CompareTag("Door")) {
            // Activez l'interface UI consigne
            if (consigneDoor != null)
            {
                consigneDoor.SetActive(true);
                // Met le focus sur le champ de saisie
                champSaisie.Select();

                // Active le curseur dans le champ de saisie
                champSaisie.ActivateInputField();
                
            }
        }



    }
    private void OnTriggerExit(Collider collision) {
        
        if (collision.gameObject.CompareTag("Book")) {
            // Desactivez l'interface UI consigne
                consigneUI.SetActive(false);
        }
        if (collision.gameObject.CompareTag("Door")) {
            // Desactivez l'interface UI consigne
                consigneDoor.SetActive(false);
                
        }
    }

    public void ReadStringInput(string s){
        input = s.ToLower();  // Convertit la saisie en minuscules
        if (input == "humain") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
