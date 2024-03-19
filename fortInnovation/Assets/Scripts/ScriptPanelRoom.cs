using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPanelRoom : MonoBehaviour
{   
    private StarterAssets.ThirdPersonController thirdPersonController;

    void Start(){
        // Trouver le script ThirdPersonController automatiquement au démarrage
        thirdPersonController = FindObjectOfType<StarterAssets.ThirdPersonController>();
        DisableGameplayInput();
    }
    public void DisableGameplayInput()
    {
        // Désactive les entrées de gameplay
        if (thirdPersonController != null)
        {
            thirdPersonController.enabled = false;
        }
    }

    public void EnableGameplayInput()
    {
        // Réactive les entrées de gameplay
        if (thirdPersonController != null)
        {
            thirdPersonController.enabled = true;
            Debug.Log("oui");
        }
    }
}
