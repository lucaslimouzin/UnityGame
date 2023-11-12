using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenu : MonoBehaviour
{
    public Toggle checkboxUI;
    private GestionnaireUI gestionnaireUI;

    private void Start()
    {
        // Trouver le gestionnaire UI existant ou créer un nouveau s'il n'existe pas
        gestionnaireUI = FindObjectOfType<GestionnaireUI>();
        if (gestionnaireUI == null)
        {
            GameObject gestionnaireUIGameObject = new GameObject("GestionnaireUI");
            gestionnaireUI = gestionnaireUIGameObject.AddComponent<GestionnaireUI>();
        }
    }
    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }
    // Appelez cette méthode depuis l'événement OnValueChanged de la checkbox
    public void ActiverDesactiverUI()
    {
        if (gestionnaireUI != null)
        {
            gestionnaireUI.ActiverUI(checkboxUI.isOn);
        }
        else
        {
            Debug.LogError("GestionnaireUI non trouvé.");
        }
    }
}
