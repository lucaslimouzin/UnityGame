using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
public class GestionnaireUI : MonoBehaviour
{
    private static GestionnaireUI instance;
    private GameObject ui; // Variable pour stocker la r�f�rence de l'objet UI
    private string nomSceneAvecUI = "dungeons"; // Remplacez par le nom de votre sc�ne

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

    // M�thode pour activer ou d�sactiver l'objet UI
    public void ActiverUI(bool activer)
    {
        // Assurez-vous que la sc�ne avec l'objet UI est charg�e
        if (!SceneManager.GetSceneByName(nomSceneAvecUI).isLoaded)
        {
            SceneManager.LoadScene(nomSceneAvecUI, LoadSceneMode.Additive);
            return;
        }

        // Si la r�f�rence � l'objet UI n'a pas encore �t� d�finie, recherchez-la dans la sc�ne
        if (ui == null)
        {
            // Recherchez l'objet UI dans la sc�ne
            ui = GameObject.Find("UI_Canvas_StarterAssetsInputs_Joysticks");

            if (ui != null)
            {
                Debug.Log("R�f�rence � l'UI trouv�e dans la sc�ne " + nomSceneAvecUI);
            }
            else
            {
                Debug.LogError("UI non trouv� dans la sc�ne " + nomSceneAvecUI);
                return; // Sortez de la m�thode si l'objet n'est pas trouv�
            }
        }

        // Activez ou d�sactivez l'objet UI
        ui.SetActive(true);
    }
}