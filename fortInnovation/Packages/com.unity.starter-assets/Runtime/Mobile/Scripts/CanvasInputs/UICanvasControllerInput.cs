using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;

        private void Start()
        {
            InitializeStarterAssetsInputs();
        }

        private void InitializeStarterAssetsInputs()
        {
            // Rechercher un GameObject avec le tag "Player"
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                // Obtenir le composant StarterAssetsInputs de l'objet trouvé
                starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
            }
            else
            {
                Debug.LogWarning("GameObject avec le tag 'Player' n'a pas été trouvé dans la scène");
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
