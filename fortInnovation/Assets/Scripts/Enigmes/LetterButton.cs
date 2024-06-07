using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterButton : MonoBehaviour
{
    public bool isSelected = false; // État de sélection
    private Image buttonImage; // Référence à l'image du bouton

    void Start()
    {
        buttonImage = GetComponent<Image>(); // Récupération de l'image du bouton
    }

    public void ToggleSelection()
    {
        isSelected = !isSelected;
        buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, isSelected ? 0.5f : 0f);
    }

    // Nouvelle méthode pour désélectionner le bouton
    public void Deselect()
    {
        isSelected = false;
        buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0f);
    }
}
