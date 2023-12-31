using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class LoadCharacter : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public CinemachineVirtualCamera virtualCamera;
    void Start() {
        GameObject prefab = Instantiate(characterPrefabs[MainGameManager.Instance.selectedCharacter]);
        if (virtualCamera != null )
        {
            // Trouver PlayerCameraRoot comme enfant de PlayerArmature_Homme
            Transform playerCameraRoot = prefab.transform.Find("PlayerCameraRoot");

            if (playerCameraRoot != null)
            {
                virtualCamera.Follow = playerCameraRoot;
            }
            else
            {
                Debug.LogError("Player Camera Root not found");
            }
        }
        else
        {
            Debug.LogError("Virtual Camera or Player Armature Homme is not assigned");
        }
       
    }
}
