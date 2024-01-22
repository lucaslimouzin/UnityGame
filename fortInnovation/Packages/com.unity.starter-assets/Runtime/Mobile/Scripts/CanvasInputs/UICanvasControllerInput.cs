using System.Collections;
using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;

        private void Start()
        {
            StartCoroutine(InitializeStarterAssetsInputs());
        }

        private IEnumerator InitializeStarterAssetsInputs()
        {
            // Continuer à chercher jusqu'à ce que le player soit trouvé
            while (starterAssetsInputs == null)
            {
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
                    break; // Sortir de la boucle si le player est trouvé
                }
                else
                {
                    Debug.LogWarning("GameObject avec le tag 'Player' n'a pas été trouvé dans la scène. Réessai...");
                    yield return new WaitForSeconds(1); // Attendre 1 seconde avant de réessayer
                }
            }
        }
        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            starterAssetsInputs.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            starterAssetsInputs.LookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            starterAssetsInputs.JumpInput(virtualJumpState);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            starterAssetsInputs.SprintInput(virtualSprintState);
        }
        
    }

}
