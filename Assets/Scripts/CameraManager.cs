using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;

    public Transform targetTransform;
    public Transform cameraPivot;
    public Transform cameraTransform;
    public LayerMask colissionLayers;
    private float defaultPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    public float minimumCollsionOffSet = 0.2f;
    public float cameraCollisionOffSet = 0.2f;
    public float cameraCollisionRadius = 0.2f;
    public float camerafollowSpeed = 0.2f;
    public float cameraLookSpeed = 2;
    public float cameraPivotSpeed = 2;

    public float LookAngel;
    public float pivotAngle;

    public float minimumPivotAngle = -35;
    public float maximumPivotAngle = 35;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(
            transform.position, targetTransform.position, ref cameraFollowVelocity, camerafollowSpeed);

        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        LookAngel = LookAngel + (inputManager.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputManager.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        rotation = Vector3.zero;
        rotation.y = LookAngel;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation; // Corrected here
    }

  private void HandleCameraCollisions()
  {
      float targetPosition = defaultPosition;
      RaycastHit hit;
      Vector3 direction = cameraTransform.position - cameraPivot.position;
      direction.Normalize();
  
      Debug.DrawRay(cameraPivot.position, direction * Mathf.Abs(defaultPosition), Color.red);
  
      if (Physics.SphereCast(cameraPivot.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(defaultPosition), colissionLayers))
      {
          Debug.Log("Collision detected with: " + hit.collider.name);
          float distance = Vector3.Distance(cameraPivot.position, hit.point);
          targetPosition = -(distance - cameraCollisionOffSet);
      }
      else
      {
          Debug.Log("No collision detected");
      }
  
      if (Mathf.Abs(targetPosition) < minimumCollsionOffSet)
      {
          targetPosition = -minimumCollsionOffSet;
      }
  
      cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
      cameraTransform.localPosition = cameraVectorPosition;
  }
}
