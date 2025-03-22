using System;
using Unity.Netcode;
using UnityEngine;

    public class UICanvasControllerInput : NetworkBehaviour
    {

        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;

        public void Start()
        {
            
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
        
        public void FindLocalPlayer()
        {
            // Lấy tất cả GameObject có tag "Player"
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                // Lấy NetworkObject từ GameObject
                NetworkObject networkObject = player.GetComponent<NetworkObject>();
            
                if (networkObject != null && networkObject.IsOwner)
                {
                    starterAssetsInputs = networkObject.GetComponent<StarterAssetsInputs>();
                    return;
                }
            }

            Debug.LogWarning("No local player found!");
        }
    }
        
    

