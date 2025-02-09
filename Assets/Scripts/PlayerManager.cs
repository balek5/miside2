using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
   
   InputManager inputManager;
   CameraManager cameraManager;
   PlayerLocomotion playerLocomotion;
   
   
   
   private void Awake()
   {
    inputManager = GetComponent<InputManager>();   
    cameraManager = FindObjectOfType<CameraManager>();
    playerLocomotion = GetComponent<PlayerLocomotion>();
   }

   public void Update()
   {
       inputManager.HandleAllInputs();
   }

   private void FixedUpdate()
   {
     playerLocomotion.HandleAllMovement();
     
   }   
   
    private void LateUpdate()
    {
      cameraManager.HandleAllCameraMovement();
      

    }
}

