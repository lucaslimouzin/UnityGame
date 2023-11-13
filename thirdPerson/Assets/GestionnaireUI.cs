using UnityEngine;
using UnityEngine.UI;
public class GestionnaireUI : MonoBehaviour
{
    
    public Toggle checkboxUI;
    public GameObject ui1;
    public GameObject ui2;
    public GameObject ui3;
    public GameObject ui4;

    public void ActiverDesactiverUI() {
        ActiverUI(checkboxUI.isOn);
    }

    // M�thode pour activer ou d�sactiver l'objet UI
    public void ActiverUI(bool activer)
    {
         // Activez ou d�sactivez l'objet UI1
        ui1.SetActive(activer);
         // Activez ou d�sactivez l'objet UI1
        ui2.SetActive(activer);
         // Activez ou d�sactivez l'objet UI1
        ui3.SetActive(activer);
         // Activez ou d�sactivez l'objet UI1
        ui4.SetActive(activer);
    }
}