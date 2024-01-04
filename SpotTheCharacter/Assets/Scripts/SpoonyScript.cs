using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpoonyScript : MonoBehaviour
{
     private Vector3 offset;
    private Camera mainCamera;
    private float lastClickTime;
    private const float doubleClickThreshold = 0.2f; // Temps en secondes


    void Start()
    {
        mainCamera = Camera.main; // Obtenir une référence à la caméra principale
    }

    
    void OnMouseDown()
    {
        // Convertir la position de l'objet de l'espace monde en espace écran
        Vector3 objectPointInScreen = mainCamera.WorldToScreenPoint(transform.position);

        // Calculer l'offset entre la position de l'objet et la position de la souris/du doigt
        offset = transform.position - mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, objectPointInScreen.z));
    }

    void OnMouseDrag()
    {
         // Suivre le déplacement de la souris/du doigt en tenant compte de l'offset
        Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.WorldToScreenPoint(transform.position).z);
        Vector3 currentPosition = mainCamera.ScreenToWorldPoint(currentScreenPoint) + offset;

        // Limiter les valeurs de X et Y aux limites définies
        currentPosition.x = Mathf.Clamp(currentPosition.x, -1.21f, 1.21f);
        currentPosition.y = Mathf.Clamp(currentPosition.y, -2.11f, 2.75f);

        // Mettre à jour la position de l'objet
        transform.position = currentPosition;
    }
}
