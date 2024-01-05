using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Nécessaire pour accéder aux informations de la scène

public class AccueilManager : MonoBehaviour
{
    public Sprite[] bodySprites, faceSprites, brasDroitSprites, brasGaucheSprites;
    public Image uiCorps, uiFace, uiBrasDroit, uiBrasGauche; // Références aux éléments UI 

    void Start()
    {
        StartCoroutine(ChangeSpritesCoroutine());
    }

    IEnumerator ChangeSpritesCoroutine()
    {
        while (true)
        {
            uiCorps.sprite = bodySprites[Random.Range(0, bodySprites.Length)];
            uiFace.sprite = faceSprites[Random.Range(0, faceSprites.Length)];
            uiBrasDroit.sprite = brasDroitSprites[Random.Range(0, brasDroitSprites.Length)];
            uiBrasGauche.sprite = brasGaucheSprites[Random.Range(0, brasGaucheSprites.Length)];

            yield return new WaitForSeconds(1f); // Attendre 1 secondes
        }
    }

    public void SelectLevel() {
        // Obtenez l'index de la scène actuelle
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // Calculez l'index de la scène suivante
        int nextSceneIndex = currentSceneIndex + 1;

        // Vérifiez si la scène suivante existe dans les paramètres de build
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Chargez la scène suivante
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Il n'y a pas d'autres niveaux à charger.");
            // Gérez le cas où il n'y a pas de niveau suivant, par exemple, affichez un écran de fin de jeu
        }
    }
}
