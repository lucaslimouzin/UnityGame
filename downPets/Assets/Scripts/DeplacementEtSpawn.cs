using UnityEngine;

public class DeplacementEtSpawn : MonoBehaviour
{
    private bool estEnTrainDeTenir = false;
    private Vector3 positionInitiale;
    private float dernierClicTime;
    public GameObject objetPrefab; // Assurez-vous de définir le prefab de l'objet dans l'éditeur Unity.
    public float delaiEntreClics = 2f; // Délai en secondes

    void Update()
    {
        // Détection du maintien du clic de la souris
        if (Input.GetMouseButton(0))
        {
            if (!estEnTrainDeTenir)
            {
                CommencerTenir();
            }

            DeplacerObjet();
        }
        else
        {
            if (estEnTrainDeTenir)
            {
                RelacherObjet();
            }
        }

        // Détection du clic de la souris
        if (Input.GetMouseButtonDown(0) && !estEnTrainDeTenir && (Time.time - dernierClicTime) > delaiEntreClics)
        {
            SpawnObjet();
            dernierClicTime = Time.time;
        }
    }

    void CommencerTenir()
    {
        estEnTrainDeTenir = true;
        positionInitiale = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void DeplacerObjet()
    {
        Vector3 positionSouris = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float deplacementX = positionSouris.x - positionInitiale.x;
        transform.position += new Vector3(deplacementX, 0, 0);
        positionInitiale = positionSouris;
    }

    void RelacherObjet()
    {
        estEnTrainDeTenir = false;
        SpawnObjet();
    }

    void SpawnObjet()
    {
        // Spawner l'objet à une position décalée en Z
        Vector3 spawnPosition = transform.position + new Vector3(0, -1, 0); // Vous pouvez ajuster la valeur Z selon vos besoins
        Instantiate(objetPrefab, spawnPosition, Quaternion.identity);
    }
}
