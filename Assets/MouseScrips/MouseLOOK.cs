using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLOOK : MonoBehaviour
{
    public float mouseSensitivy = 100f;

    public Transform playerBody;
    float xRotation = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivy * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivy * Time.deltaTime;
            
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90, 90);
            
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            
            playerBody.Rotate(Vector3.up * mouseX);
    }
}

