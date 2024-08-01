using System.Collections;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 offset;
    private Rigidbody rb; // Référence au Rigidbody de l'objet
    private bool isStopped = false; // Flag pour savoir si l'objet doit être arrêté
    private float outOfBoundsTime = 0f; // Temps passé en dehors de la zone
    private bool isCoroutineRunning = false; // Flag pour savoir si la coroutine est en cours d'exécution

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
            if (newPosition.x >= 0.036f && newPosition.x <= 0.542f)
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

            // Vérifier si l'objet est en dehors de la zone et démarrer la coroutine si nécessaire
            if (newPosition.x < 0.036f || newPosition.x > 0.542f)
            {
                if (!isCoroutineRunning)
                {
                    StartCoroutine(CheckOutOfBounds());
                }
            }
            else
            {
                outOfBoundsTime = 0f;
                isCoroutineRunning = false;
                StopCoroutine(CheckOutOfBounds());
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

    private IEnumerator CheckOutOfBounds()
    {
        isCoroutineRunning = true;
        while (outOfBoundsTime < 10f)
        {
            outOfBoundsTime += Time.deltaTime;
            yield return null;
        }

        // Réinitialiser la position de l'objet si il reste en dehors de la zone plus de 20 secondes
        if (outOfBoundsTime >= 10f)
        {
            transform.position = new Vector3(0.2f, transform.position.y, transform.position.z);
            ChangerCouleur(Color.green);
            rb.constraints = RigidbodyConstraints.None;
        }

        outOfBoundsTime = 0f;
        isCoroutineRunning = false;
    }
}
