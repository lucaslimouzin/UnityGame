using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 offset;
    private Rigidbody rb; // Référence au Rigidbody de l'objet
    private bool isStopped = false; // Flag pour savoir si l'objet doit être arrêté

    void Start()
    {
        ChangerCouleur(Color.red);
        rb = GetComponent<Rigidbody>();
    }

    // Méthode pour désactiver le script
    public void DisableScript()
    {
        enabled = false;
    }

    private void OnMouseDown()
    {
        if (!isStopped)
        {
            // Calculer la différence entre la position de la souris et la position de l'objet
            offset = transform.position - GetMouseWorldPos();
        }
    }

    private void OnMouseDrag()
    {
        if (!isStopped)
        {
            // Obtenir la nouvelle position de la souris dans le monde
            Vector3 newPosition = GetMouseWorldPos() + offset;

            // maintenir la position Z reste constante
            newPosition.z = transform.position.z;

            // maintenir la position Y reste constante
            newPosition.y = 2.00f; 

            // Vérifier si la position en X est dans l'intervalle désiré
            if (transform.position.x >= 0.036f && transform.position.x <= 0.542f)
            {
                ChangerCouleur(Color.green); 
                rb.constraints = RigidbodyConstraints.None;
            }
            else
            {
                ChangerCouleur(Color.red); 
                rb.constraints = RigidbodyConstraints.FreezePositionY;
            }

            // Mettre à jour la position de l'objet
            transform.position = newPosition;

            // Vérifier si la position en X a atteint la valeur spécifique
            if (transform.position.x >= 0.04f && transform.position.x <= 0.3f)
            {
                StopObject();
            }
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        // Convertir la position de la souris en coordonnées mondiales
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void ChangerCouleur(Color nouvelleCouleur)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = nouvelleCouleur;
        }
    }

    private void StopObject()
    {
        isStopped = true;
        rb.constraints = RigidbodyConstraints.FreezePositionX;
        
    }
}
