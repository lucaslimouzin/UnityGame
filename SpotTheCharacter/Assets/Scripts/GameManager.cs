using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject spoonyPrefab;
    public Sprite[] bodySprites, faceSprites, brasDroitSprites, brasGaucheSprites;
    public Image uiCorps, uiFace, uiBrasDroit, uiBrasGauche; // Références aux éléments UI 

    private GameObject selectedSpoonyForUI;

    void Start()
    {
        GameObject spoony1 = SpawnSpoony();
        GameObject spoony2 = SpawnSpoony(); // Spawn deux fois

        // Sélectionnez aléatoirement l'un des deux Spoonies
        selectedSpoonyForUI = Random.value < 0.5 ? spoony1 : spoony2;
        UpdateUI(selectedSpoonyForUI);
    }

    GameObject SpawnSpoony()
    {
        // Définir une position aléatoire basée sur les dimensions données
        float randomX = Random.Range(-1.21f, 1.21f);
        float randomY = Random.Range(-2.11f, 2.75f);
        Vector3 randomPosition = new Vector3(randomX, randomY, 0);

        // Instancier le prefab Spoony
        GameObject newSpoony = Instantiate(spoonyPrefab, randomPosition, Quaternion.identity);

        // Attribuer des sprites aléatoires
        AssignRandomSprite(newSpoony.transform.Find("Body").gameObject, bodySprites);
        AssignRandomSprite(newSpoony.transform.Find("Face").gameObject, faceSprites);
        AssignRandomSprite(newSpoony.transform.Find("BrasDroit").gameObject, brasDroitSprites);
        AssignRandomSprite(newSpoony.transform.Find("BrasGauche").gameObject, brasGaucheSprites);

        return newSpoony;
    }

    void AssignRandomSprite(GameObject obj, Sprite[] sprites)
    {
        if (sprites.Length > 0)
        {
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
            }
        }
    }

    void UpdateUI(GameObject spoony)
    {
        // Mettre à jour chaque élément d'UI avec les sprites correspondants du Spoony
        uiCorps.sprite = spoony.transform.Find("Body").GetComponent<SpriteRenderer>().sprite;
        uiFace.sprite = spoony.transform.Find("Face").GetComponent<SpriteRenderer>().sprite;
        uiBrasDroit.sprite = spoony.transform.Find("BrasDroit").GetComponent<SpriteRenderer>().sprite;
        uiBrasGauche.sprite = spoony.transform.Find("BrasGauche").GetComponent<SpriteRenderer>().sprite;
    }

    public void CheckSpoonySelection(GameObject clickedSpoony)
    {
        if (clickedSpoony == selectedSpoonyForUI)
        {
            Debug.Log("Win");
        }
        else
        {
            Debug.Log("Lose");
        }
    }
}
