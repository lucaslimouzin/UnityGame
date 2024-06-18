using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class LinkHandler : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
       TMP_Text pTextMeshPro = GetComponent<TMP_Text>();
        // Vérifier si un lien a été cliqué
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, eventData.position, null);
        if (linkIndex != -1)
        {
            // Récupérer les informations du lien cliqué
            TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];

            // Ouvrir l'URL dans le navigateur par défaut
            Application.OpenURL(linkInfo.GetLinkID());
        }
        Debug.Log("il a cliqué");
    }
}
