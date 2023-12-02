using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeplacementEtSpawn : MonoBehaviour
{
    private bool estEnTrainDeTenir = false;
    private Vector3 positionInitiale;
    private float dernierClicTime;
    public GameObject[] objetsPossibles;
    public float delaiEntreClics = 2.8f;
    private GameObject objetSpawned;
    private Rigidbody2D objetRigidbody;
    private GameObject prochainObjet;
    private bool clicAutorise = true;
    public Image nextImage;
    public Sprite[] spritePossibles;
    private int num;
    private bool firstTime = true;

    void Start()
    {
        // Trouver le GameObject par son nom
        GameObject objetAvecImage = GameObject.Find("nextImage");
        // Récupérer le composant SpriteRenderer attaché au GameObject
        nextImage= objetAvecImage.GetComponent<Image>();
        ChoisirProchainObjet();
    }

    void Update()
    {
        // Vérifier si un bouton d'UI est en cours d'interaction
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; // Ne rien faire si le clic est sur un bouton d'UI
        }
        if (objetSpawned == null && firstTime)
        {
            firstTime = false;
            SpawnObjet();
            
        }

        if (Input.GetMouseButton(0) && clicAutorise)
        {
            RelacherObjet();
        }
        
    }

   

    void RelacherObjet()
    {
        // Vérifier si le clic est autorisé
        if (clicAutorise)
        {
            estEnTrainDeTenir = false;
            // Utiliser la position du clic en X comme nouvelle position X du spawner
            Vector3 positionSouris = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            positionInitiale = new Vector3(positionSouris.x, transform.position.y, transform.position.z);
            transform.position = positionInitiale; // Mettre à jour la position immédiatement

            // Activer le Rigidbody2D lorsqu'on relâche
            if (objetRigidbody != null)
            {
                objetRigidbody.bodyType = RigidbodyType2D.Dynamic;
                // Changer la couche à "ObjetSpawned"
                objetRigidbody.gameObject.layer = LayerMask.NameToLayer("ObjetSpawned");
                objetRigidbody.gravityScale = 1;
            }
            objetSpawned.transform.position = transform.position + new Vector3(0, -1, 0);
            // Désactiver le clic de la souris pendant 1.5 secondes
            clicAutorise = false;
            Invoke("SpawnObjet", 0.8f);
            Invoke("AutoriserClic",1f);
        }
    }

    void ChoisirProchainObjet()
    {
        num = Random.Range(0, objetsPossibles.Length);
        nextImage.sprite = spritePossibles[num];
        prochainObjet = objetsPossibles[num];
    }

    void SpawnObjet()
    {
        // Spawner l'objet à une position décalée en Z
        objetSpawned = Instantiate(prochainObjet, transform.position + new Vector3(0, -1, 0), Quaternion.identity);

        // Désactiver le Rigidbody2D temporairement et obtenir une référence à celui-ci
        objetRigidbody = objetSpawned.GetComponent<Rigidbody2D>();
        if (objetRigidbody != null)
        {
            objetRigidbody.bodyType = RigidbodyType2D.Dynamic;
            // Changer la couche à "Default"
            objetRigidbody.gameObject.layer = LayerMask.NameToLayer("Default");
            objetRigidbody.gravityScale = 0;
        }

        // Choisir le prochain objet à spawner
        ChoisirProchainObjet();
    }

    void AutoriserClic()
    {
        
        dernierClicTime = Time.time;
        // Autoriser à nouveau le clic après le délai
        clicAutorise = true;
    }
}