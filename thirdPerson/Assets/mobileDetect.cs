using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileDetect : MonoBehaviour
{
    public GameObject mobileUI; // R�f�rence � l'objet UI que vous souhaitez activer sur mobile

    // D�finissez ici le rapport d'aspect maximal pour consid�rer comme une r�solution de tablette ou mobile
    public float maxMobileAspectRatio = 1.7f;

    void Start()
    {
        // Obtenez le rapport d'aspect de l'�cran
        float aspectRatio = (float)Screen.width / Screen.height;

        // V�rifiez si le rapport d'aspect est inf�rieur ou �gal � la valeur maximale pour une tablette ou un mobile
        if (aspectRatio <= maxMobileAspectRatio)
        {
            // Activez l'objet UI pour mobile et d�sactivez l'objet UI pour bureau
                mobileUI.SetActive(true);

        }
        else
        {
                mobileUI.SetActive(false);
        }
    }
}
