using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class AccueilTests
{
    private GameObject gameObject;
    private Accueil accueil;

    [SetUp]
    public void Setup()
    {
        // Créer un nouveau GameObject pour attacher le script Accueil
        gameObject = new GameObject();
        accueil = gameObject.AddComponent<Accueil>();

        // Initialiser les images des personnages
        accueil.characters = new Image[3];
        for (int i = 0; i < accueil.characters.Length; i++)
        {
            GameObject characterObject = new GameObject();
            accueil.characters[i] = characterObject.AddComponent<Image>();
        }

        // Appeler la méthode Start manuellement
        accueil.Start();
    }

    [TearDown]
    public void Teardown()
    {
        // Détruire le GameObject après chaque test
        Object.DestroyImmediate(gameObject);
    }

    [Test]
    public void TestStartMethod()
    {
        // Vérifier que le premier personnage est activé et les autres désactivés
        Assert.IsTrue(accueil.characters[0].enabled);
        Assert.IsFalse(accueil.characters[1].enabled);
        Assert.IsFalse(accueil.characters[2].enabled);
    }

    [Test]
    public void TestNextCharacter()
    {
        // Appeler la méthode NextCharacter
        accueil.NextCharacter();

        // Vérifier que le deuxième personnage est activé
        Assert.IsFalse(accueil.characters[0].enabled);
        Assert.IsTrue(accueil.characters[1].enabled);
        Assert.IsFalse(accueil.characters[2].enabled);
    }

    [Test]
    public void TestPreviousCharacter()
    {
        // Appeler la méthode PreviousCharacter
        accueil.PreviousCharacter();

        // Vérifier que le dernier personnage est activé
        Assert.IsFalse(accueil.characters[0].enabled);
        Assert.IsFalse(accueil.characters[1].enabled);
        Assert.IsTrue(accueil.characters[2].enabled);
    }

    [UnityTest]
    public IEnumerator TestPlayInstruction()
    {
        // Créer un GameManager simulé
        var gameManagerObject = new GameObject();
        var gameManager = gameManagerObject.AddComponent<MainGameManager>();
        MainGameManager.Instance = gameManager;

        // Appeler la méthode PlayInstruction
        accueil.PlayInstruction();

        // Vérifier les variables du GameManager
        Assert.AreEqual(accueil.selectedCharacter, MainGameManager.Instance.selectedCharacter);
        Assert.AreEqual("Instruction", MainGameManager.Instance.jeuEnCours);
        Assert.AreEqual("Introduction", MainGameManager.Instance.cinematiqueEnCours);

        // Vérifier que la scène "Cinematiques" est chargée
        yield return null;
        Assert.AreEqual("Cinematiques", SceneManager.GetActiveScene().name);
    }
}
