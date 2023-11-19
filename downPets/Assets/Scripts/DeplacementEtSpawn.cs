using UnityEngine;
using UnityEngine.EventSystems;

public class DeplacementEtSpawn : MonoBehaviour
{
    private bool estEnTrainDeTenir = false;
    private Vector3 positionInitiale;
    private float dernierClicTime;
    public GameObject[] objetsPossibles;
    public float delaiEntreClics = 2f;
    private GameObject objetSpawned;
    private Rigidbody2D objetRigidbody;
    private GameObject prochainObjet;
    private bool clicAutorise = true;

    void Start()
    {
        ChoisirProchainObjet();
    }

    void Update()
    {
        // Vérifier si un bouton d'UI est en cours d'interaction
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; // Ne rien faire si le clic est sur un bouton d'UI
        }
        if (objetSpawned == null)
        {
            SpawnObjet();
        }

        if (Input.GetMouseButton(0) && clicAutorise)
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
                dernierClicTime = Time.time;
                RelacherObjet();
            }
        }

        if (Input.GetMouseButtonDown(0) && !estEnTrainDeTenir && (Time.time - dernierClicTime) > delaiEntreClics && clicAutorise)
        {
            dernierClicTime = Time.time;
        }

        if (objetSpawned != null && estEnTrainDeTenir)
        {
            objetSpawned.transform.position = transform.position + new Vector3(0, -1, 0);
        }
    }

    void CommencerTenir()
    {
        // Vérifier si le bouton est toujours enfoncé
        if (Input.GetMouseButton(0))
        {
            estEnTrainDeTenir = true;
            // Utiliser la position du clic en X comme nouvelle position X du spawner
            Vector3 positionSouris = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            positionInitiale = new Vector3(positionSouris.x, transform.position.y, transform.position.z);
            transform.position = positionInitiale; // Mettre à jour la position immédiatement
        }

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
        // Vérifier si le clic est autorisé
        if (clicAutorise)
        {
            estEnTrainDeTenir = false;

            // Activer le Rigidbody2D lorsqu'on relâche
            if (objetRigidbody != null)
            {
                objetRigidbody.bodyType = RigidbodyType2D.Dynamic;
                // Changer la couche à "ObjetSpawned"
                objetRigidbody.gameObject.layer = LayerMask.NameToLayer("ObjetSpawned");
            }

            // Désactiver le clic de la souris pendant 1.5 secondes
            clicAutorise = false;
            Invoke("AutoriserClic", 0.5f);

            // Si le délai entre les clics a été respecté, spawn un nouvel objet après 0.5 secondes
            if ((Time.time - dernierClicTime) < delaiEntreClics)
            {
                Invoke("SpawnObjet", 0.5f);
                dernierClicTime = Time.time;
            }
        }
    }

    void ChoisirProchainObjet()
    {
        prochainObjet = objetsPossibles[Random.Range(0, objetsPossibles.Length)];
    }

    void SpawnObjet()
    {
        // Spawner l'objet à une position décalée en Z
        objetSpawned = Instantiate(prochainObjet, transform.position + new Vector3(0, -1, 0), Quaternion.identity);

        // Désactiver le Rigidbody2D temporairement et obtenir une référence à celui-ci
        objetRigidbody = objetSpawned.GetComponent<Rigidbody2D>();
        if (objetRigidbody != null)
        {
            objetRigidbody.bodyType = RigidbodyType2D.Static;
            // Changer la couche à "Default"
            objetRigidbody.gameObject.layer = LayerMask.NameToLayer("Default");
        }

        // Choisir le prochain objet à spawner
        ChoisirProchainObjet();
    }

    void AutoriserClic()
    {
        // Autoriser à nouveau le clic après le délai
        clicAutorise = true;
    }
}