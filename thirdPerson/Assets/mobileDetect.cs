using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileDetect : MonoBehaviour
{
    public GameObject mobileUI; // Référence à l'objet UI que vous souhaitez activer sur mobile

    // Définissez ici le rapport d'aspect maximal pour considérer comme une résolution de tablette ou mobile
    public float maxMobileAspectRatio = 1.7f;

    void Start()
    {
        // Obtenez le rapport d'aspect de l'écran
        float aspectRatio = (float)Screen.width / Screen.height;

        // Vérifiez si le rapport d'aspect est inférieur ou égal à la valeur maximale pour une tablette ou un mobile
        if (aspectRatio <= maxMobileAspectRatio)
        {
            // Activez l'objet UI pour mobile et désactivez l'objet UI pour bureau
                mobileUI.SetActive(true);

        }
        else
        {
                mobileUI.SetActive(false);
        }
    }
}
