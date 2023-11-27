using UnityEngine;

public class InteractionObjet : MonoBehaviour
{
    public string tagObjetCible;
    public GameObject objetCiblePrefab;
    public float seuilGameOver = 3f; // Seuil de déclenchement du game over en secondes
    public Color couleurGameOver = Color.red; // Couleur du game over
    public float tempsDepassementMax = 1.5f; // Seuil de temps pour changer la couleur
    private Rigidbody2D objetRigidbody;
    public int points;

    private bool enCollision = false;
    private float tempsDepassement = 0f;
    private float tempsDepassementZoneSpawn = 0f;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        // Trouver le GameObject par son nom
        GameObject objetAvecSpriteRenderer = GameObject.Find("ligneGameOver");
        // Récupérer le composant SpriteRenderer attaché au GameObject
        spriteRenderer = objetAvecSpriteRenderer.GetComponent<SpriteRenderer>();
       
    }

    void Update()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Default")) {
            tempsDepassementZoneSpawn += Time.deltaTime;
            // Vérifier si le seuil de dépassement du temps est atteint
            if (tempsDepassementZoneSpawn >= 5.0f)
            {
                objetRigidbody =gameObject.GetComponent<Rigidbody2D>();
                objetRigidbody.bodyType = RigidbodyType2D.Dynamic;
                // Changer la couche à "ObjetSpawned"
                objetRigidbody.gameObject.layer = LayerMask.NameToLayer("ObjetSpawned");
            }
        }
        else
        {
            tempsDepassementZoneSpawn = 0f;
        }
        
        // Si l'objet est en collision avec la limite
        if (enCollision)
        {
            tempsDepassement += Time.deltaTime;
           // Debug.Log(tempsDepassement);

            // Vérifier si le seuil de dépassement du temps est atteint
            if (tempsDepassement >= seuilGameOver)
            {
                GestionInteractions.instance.GameOver();
            }

            // Changer la couleur si le temps de dépassement est supérieur à la valeur seuil
            if (tempsDepassement >= tempsDepassementMax)
            {
                spriteRenderer.color = couleurGameOver;
            }
        }
        else
        {
            // Réinitialiser le compteur si l'objet ne dépasse pas la limite
            tempsDepassement = 0f;
            // Réinitialiser la couleur à sa valeur par défaut
            //spriteRenderer.color = Color.white;
        }
    }

  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("gameOver"))
        {
            //Debug.Log("enter");
            enCollision = true;
        }
    }

    

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("gameOver"))
        {
           // Debug.Log("exit");
            enCollision = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Vérifier si l'objet en collision a le même tag
        if (collision.gameObject.CompareTag(tagObjetCible))
        {
            // Récupérer le gestionnaire et lui envoyer une notification avec l'objet actuel et l'objet cible
            GestionInteractions.instance.GererCollisionObjet(gameObject, objetCiblePrefab, points);
        }
    }
}
