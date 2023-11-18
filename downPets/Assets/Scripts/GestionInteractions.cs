// GestionInteractions.cs
using UnityEngine;

public class GestionInteractions : MonoBehaviour
{
    public static GestionInteractions instance;

    private int objetsEnCours = 0;

    private GameObject premierObjetEnCollision;
    private GameObject deuxiemeObjetEnCollision;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GererCollisionObjet(GameObject objetEnCollision, GameObject objetCiblePrefab)
    {
        // Incr�menter le nombre d'objets en collision
        objetsEnCours++;

        // Enregistrer les objets en collision
        if (objetsEnCours == 1)
        {
            premierObjetEnCollision = objetEnCollision;
        }
        else if (objetsEnCours == 2)
        {
            deuxiemeObjetEnCollision = objetEnCollision;

            // R�cup�rer la position du premier objet en collision
            Vector3 positionSpawn = premierObjetEnCollision.transform.position;

            // D�truire les deux objets en collision
            Destroy(premierObjetEnCollision);
            Destroy(deuxiemeObjetEnCollision);

            // R�initialiser le nombre d'objets en collision
            objetsEnCours = 0;

            // Invoquer l'objet cible � la position du premier objet en collision
            SpawnObjetCible(objetCiblePrefab, positionSpawn);
        }
    }

    private void SpawnObjetCible(GameObject objetCiblePrefab, Vector3 positionSpawn)
    {
        // Spawner l'objet cible � la position du premier objet en collision
        Instantiate(objetCiblePrefab, positionSpawn, Quaternion.identity);
    }
}
