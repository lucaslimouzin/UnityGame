// InteractionObjet.cs
using UnityEngine;

public class InteractionObjet : MonoBehaviour
{
    public string tagObjetCible;
    public GameObject objetCiblePrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // V�rifier si l'objet en collision a le m�me tag
        if (collision.gameObject.CompareTag(tagObjetCible))
        {
            // R�cup�rer le gestionnaire et lui envoyer une notification avec l'objet actuel et l'objet cible
            GestionInteractions.instance.GererCollisionObjet(gameObject, objetCiblePrefab);
        }
    }
}
