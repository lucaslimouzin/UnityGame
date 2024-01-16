using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LinkOpener : MonoBehaviour
{
    public TMP_Text site1; // R�f�rence au premier TextMeshPro
    public TMP_Text site2; // R�f�rence au second TextMeshPro

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(DelayedCheckForLinks());
        }
    }

    IEnumerator DelayedCheckForLinks()
    {
        yield return new WaitForSeconds(1f); // Attendre 1 seconde

        if (site1 != null) CheckForLink(site1);
        if (site2 != null) CheckForLink(site2);
    }

    void CheckForLink(TMP_Text textComponent)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textComponent, Input.mousePosition, null);
        if (linkIndex != -1)
        {
            // Obtenez le lien cliqu�
            TMP_LinkInfo linkInfo = textComponent.textInfo.linkInfo[linkIndex];

            // Effectuez une action en fonction de l'identifiant du lien
            // Par exemple, ouvrir une URL ou effectuer une autre action
            OpenLink(linkInfo.GetLinkID());
        }
    }

    void OpenLink(string linkID)
    {
        switch (linkID)
        {
            case "kenney":
                Application.OpenURL("https://www.kenney.nl/");
                break;
            case "flaticon":
                Application.OpenURL("https://www.flaticon.com/");
                break;
                // Ajoutez d'autres cas au besoin
        }
    }
}
