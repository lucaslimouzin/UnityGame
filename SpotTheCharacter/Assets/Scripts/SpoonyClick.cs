using UnityEngine;

public class SpoonyClick : MonoBehaviour
{
    private float lastClickTime;
    private const float doubleClickThreshold = 0.2f;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Trouver le GameManager dans la scène
    }

    void OnMouseDown()
    {
        if (Time.time - lastClickTime < doubleClickThreshold)
        {
            gameManager.CheckSpoonySelection(gameObject);
        }
        lastClickTime = Time.time;
    }
}
