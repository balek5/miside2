using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
   private InputManager inputManager;
   private Animator animator;

   private Vector3 moveDirection;
   Transform cameraObject;
   Rigidbody playerRigidbody;

   public bool isSprinting;
   public bool isJumping;

   public float walkingspeed = 1.5f;
   public float runningspeed = 5;
   public float sprintingSpeed = 7;
   public float rotationSpeed = 15;

   public float maxStamina = 100f;
   public float currentStamina;
   public float staminaDrain = 10f;
   public float staminaRegen = 5f;

   private void Awake()
   {
      inputManager = GetComponent<InputManager>();
      playerRigidbody = GetComponent<Rigidbody>();
      cameraObject = Camera.main.transform;
      animator = GetComponent<Animator>();
      currentStamina = maxStamina;
   }

   private void Update()
   {
      RegenerateStamina();
   }

   public void HandleAllMovement()
   {
      HandleMovement();
      HandleRotation();
   }

   private void HandleMovement()
   {
      moveDirection = cameraObject.forward * inputManager.verticalInput;
      moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
      moveDirection.Normalize();
      moveDirection.y = 0;

      if (isSprinting && currentStamina > 0)
      {
         moveDirection = moveDirection * sprintingSpeed;
         currentStamina -= staminaDrain * Time.deltaTime;
         animator.SetBool("isRunning", true);
         animator.SetBool("isWalking", false);
      }
      else
      {
         if (inputManager.moveAmount >= 0.5f)
         {
            moveDirection = moveDirection * runningspeed;
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
         }
         else
         {
            moveDirection = moveDirection * walkingspeed;
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
         }
      }

      Vector3 movementVelocity = moveDirection;
      playerRigidbody.velocity = movementVelocity;
   }

   private void HandleRotation()
   {
      Vector3 targetDirection = Vector3.zero;
      targetDirection = cameraObject.forward * inputManager.verticalInput;
      targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
      targetDirection.Normalize();
      targetDirection.y = 0;

      if (targetDirection == Vector3.zero)
         targetDirection = transform.forward;

      Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
      Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

      transform.rotation = playerRotation;
   }

   private void RegenerateStamina()
   {
      if (!isSprinting && currentStamina < maxStamina)
      {
         currentStamina += staminaRegen * Time.deltaTime;
      }
   }
   
}
