using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject batonPrefab;
    public Transform batonsContainer;
    public TextMeshProUGUI gameStateText;
    public Button bouton1;
    public Button bouton2;
    public Button bouton3;
    public Button restartButton;  // Référence au bouton Restart

    private int batonsRemaining;
    private bool playerTurn;
    private int lastMove;
    private bool gameOver;  // Nouvelle variable pour suivre l'état du jeu

    void Start()
    {
        batonsRemaining = 21;
        playerTurn = true;
        gameOver = false;

        bouton1.onClick.AddListener(() => OnBoutonClick(1));
        bouton2.onClick.AddListener(() => OnBoutonClick(2));
        bouton3.onClick.AddListener(() => OnBoutonClick(3));

        restartButton.onClick.AddListener(RestartGame);
        restartButton.gameObject.SetActive(false);  // Désactiver le bouton au démarrage

        // Créer les 21 bâtons au lancement du jeu
        for (int i = 0; i < batonsRemaining; i++)
        {
            GameObject baton = Instantiate(batonPrefab, GetFixedPosition(i), Quaternion.identity, batonsContainer);
        }

        UpdateGameStateText();
    }

    void OnBoutonClick(int batonsToRemove)
    {
        if (!gameOver)  // Vérifier si le jeu n'est pas encore terminé
        {
            if (playerTurn)
            {
                if (batonsToRemove > batonsRemaining)
                {
                    // Le joueur a essayé de retirer plus de bâtons que ce qui reste, rien ne se passe
                    return;
                }
                lastMove = batonsToRemove;
                // Retirer les bâtons en fonction du bouton appuyé
                for (int i = 0; i < batonsToRemove; i++)
                {
                    Destroy(batonsContainer.GetChild(batonsRemaining - i - 1).gameObject);
                }

                batonsRemaining -= batonsToRemove;
                playerTurn = false;

                if (batonsRemaining == 1)
                {
                    // Afficher le gagnant lorsque qu'il reste un bâton
                    gameStateText.text = "L'ordinateur a gagné !";
                    // Désactiver les boutons
                    bouton1.interactable = false;
                    bouton2.interactable = false;
                    bouton3.interactable = false;
                    // Activer le bouton Restart
                    restartButton.gameObject.SetActive(true);
                    gameOver = true;  // Mettre le jeu en état terminé
                }
                else
                {
                    Invoke("ComputerTurn", 1f);  // Laisser un court délai avant que l'ordinateur joue
                }
            }

            UpdateGameStateText();
        }
    }

    void ComputerTurn()
    {
        int batonsToRemove = ComputeComputerMove();

        // Retirer les bâtons en fonction de la stratégie de l'ordinateur
        for (int i = 0; i < batonsToRemove; i++)
        {
            Destroy(batonsContainer.GetChild(batonsRemaining - i - 1).gameObject);
        }

        batonsRemaining -= batonsToRemove;
        playerTurn = true;

        if (batonsRemaining == 1)
        {
            // Afficher le gagnant lorsque qu'il reste un bâton
            gameStateText.text = "Vous avez gagné(e) !";
            // Désactiver les boutons
            bouton1.interactable = false;
            bouton2.interactable = false;
            bouton3.interactable = false;
            // Activer le bouton Restart
            restartButton.gameObject.SetActive(true);
            gameOver = true;  // Mettre le jeu en état terminé
        }

        UpdateGameStateText();
    }

   int ComputeComputerMove()
    {
        // Laisser l'adversaire commencer
        if (batonsRemaining == 21)
        {
            return 0; // L'ordinateur ne retire aucun bâtonnet au premier tour
        }

        // Appliquer la stratégie gagnante
        if (lastMove == 1)
        {
            // Si l'adversaire a retiré 1 bâtonnet, en retirer 3
            return 3;
        }
        else if (lastMove == 2)
        {
            // Si l'adversaire a retiré 2 bâtonnets, en retirer 2
            return 2;
        }
        else if (lastMove == 3)
        {
            // Si l'adversaire a retiré 3 bâtonnets, en retirer 1
            return 1;
        }
        else
        {
            // Si l'adversaire a retiré 0 bâtonnet (multiple de 4), en retirer 3 pour appliquer la stratégie gagnante
            return 3;
        }
    }

    Vector3 GetFixedPosition(int index)
    {
        Vector3 basePosition = batonsContainer.position;

        // Alignez les bâtons horizontalement avec une distance fixe
        float distanceBetweenBatons = 0.8f;  // Ajustez la distance souhaitée entre les bâtons

        return basePosition + new Vector3(index * distanceBetweenBatons, 0, 0);
    }

    void UpdateGameStateText()
    {
        gameStateText.text = DetermineTurnText();
    }

    string DetermineTurnText()
    {
        if (batonsRemaining == 1)
        {
            // Un joueur a gagné
            return "Gagnant : " + (playerTurn ? "Ordinateur" : "Vous");
        }
        else
        {
            // Indiquer le tour actuel
            return (playerTurn ? "À votre tour !" : "Tour de l'ordinateur...");
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
