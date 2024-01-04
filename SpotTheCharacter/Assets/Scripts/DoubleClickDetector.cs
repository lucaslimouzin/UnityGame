using UnityEngine;

public class DoubleClickDetector : MonoBehaviour
{
    private float lastClickTime;
    private const float doubleClickThreshold = 0.4f; // Temps en secondes pour considérer qu'il s'agit d'un double clic

    void OnMouseDown()
    {
        // Vérifiez si le temps écoulé depuis le dernier clic est inférieur au seuil
        if (Time.time - lastClickTime < doubleClickThreshold)
        {
            // Double-clic détecté
            Debug.Log("Sélectionné");
        }
        lastClickTime = Time.time; // Mettre à jour le temps du dernier clic
    }
}
