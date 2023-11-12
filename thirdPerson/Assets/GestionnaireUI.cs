using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
public class GestionnaireUI : MonoBehaviour
{
    private static GestionnaireUI instance;
    private GameObject ui; // Variable pour stocker la référence de l'objet UI
    private string nomSceneAvecUI = "dungeons"; // Remplacez par le nom de votre scène

    private void Awake()
    {
        // Assurez-vous qu'il n'y a qu'une seule instance du GestionnaireUI
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Méthode pour activer ou désactiver l'objet UI
    public void ActiverUI(bool activer)
    {
        // Assurez-vous que la scène avec l'objet UI est chargée
        if (!SceneManager.GetSceneByName(nomSceneAvecUI).isLoaded)
        {
            SceneManager.LoadScene(nomSceneAvecUI, LoadSceneMode.Additive);
            return;
        }

        // Si la référence à l'objet UI n'a pas encore été définie, recherchez-la dans la scène
        if (ui == null)
        {
            // Recherchez l'objet UI dans la scène
            ui = GameObject.Find("UI_Canvas_StarterAssetsInputs_Joysticks");

            if (ui != null)
            {
                Debug.Log("Référence à l'UI trouvée dans la scène " + nomSceneAvecUI);
            }
            else
            {
                Debug.LogError("UI non trouvé dans la scène " + nomSceneAvecUI);
                return; // Sortez de la méthode si l'objet n'est pas trouvé
            }
        }

        // Activez ou désactivez l'objet UI
        ui.SetActive(true);
    }
}