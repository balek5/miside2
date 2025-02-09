using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToGame : MonoBehaviour
{
    
        public GameObject miniGameCanvas; // Reference to the mini-game canvas
        public GameObject player; // Reference to the player
        public Camera playerCamera; // The camera showing the player
        public Camera gameCamera;   // The camera showing the mini-game

        public void ReturnToMainGame()
        {
            miniGameCanvas.SetActive(false); // Deactivate the mini-game UI
            player.SetActive(true); // Reactivate the player
            playerCamera.gameObject.SetActive(true); // Reactivate the player camera
            gameCamera.gameObject.SetActive(false); // Deactivate the mini-game camera
        }
    }