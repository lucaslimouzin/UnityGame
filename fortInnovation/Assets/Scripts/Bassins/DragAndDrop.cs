using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 offset;

    private void OnMouseDown()
    {
        // Calculer la différence entre la position de la souris et la position de l'objet
        offset = transform.position - GetMouseWorldPos();
    }

    private void OnMouseDrag()
    {
        // Obtenir la nouvelle position de la souris dans le monde
        Vector3 newPosition = GetMouseWorldPos() + offset;

        // Assurez-vous que la position Z reste constante
        newPosition.z = transform.position.z;

        // Mettre à jour la position de l'objet
        transform.position = newPosition;
    }

    private Vector3 GetMouseWorldPos()
    {
        // Convertir la position de la souris en coordonnées mondiales
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
