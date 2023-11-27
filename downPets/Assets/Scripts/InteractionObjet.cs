using UnityEngine;

public class InteractionObjet : MonoBehaviour
{
    public string tagObjetCible;
    public GameObject objetCiblePrefab;
    public float seuilGameOver = 3f; // Seuil de d�clenchement du game over en secondes
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
        // R�cup�rer le composant SpriteRenderer attach� au GameObject
        spriteRenderer = objetAvecSpriteRenderer.GetComponent<SpriteRenderer>();
       
    }

    void Update()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Default")) {
            tempsDepassementZoneSpawn += Time.deltaTime;
            // V�rifier si le seuil de d�passement du temps est atteint
            if (tempsDepassementZoneSpawn >= 5.0f)
            {
                objetRigidbody =gameObject.GetComponent<Rigidbody2D>();
                objetRigidbody.bodyType = RigidbodyType2D.Dynamic;
                // Changer la couche � "ObjetSpawned"
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

            // V�rifier si le seuil de d�passement du temps est atteint
            if (tempsDepassement >= seuilGameOver)
            {
                GestionInteractions.instance.GameOver();
            }

            // Changer la couleur si le temps de d�passement est sup�rieur � la valeur seuil
            if (tempsDepassement >= tempsDepassementMax)
            {
                spriteRenderer.color = couleurGameOver;
            }
        }
        else
        {
            // R�initialiser le compteur si l'objet ne d�passe pas la limite
            tempsDepassement = 0f;
            // R�initialiser la couleur � sa valeur par d�faut
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
        // V�rifier si l'objet en collision a le m�me tag
        if (collision.gameObject.CompareTag(tagObjetCible))
        {
            // R�cup�rer le gestionnaire et lui envoyer une notification avec l'objet actuel et l'objet cible
            GestionInteractions.instance.GererCollisionObjet(gameObject, objetCiblePrefab, points);
        }
    }
}
