using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public static float mouseSensitivity = 100f;

    // Reference to the player body (for horizontal rotation)
    public Transform playerBody;

    // Reference to the camera (for vertical rotation)
    public Camera playerCamera;

    float xRotation = 0f;

    void Start()
    {
        // Locking the cursor to the middle of the screen and making it invisible
        Cursor.lockState = CursorLockMode.Locked;

        // Manually set references
        if (playerBody == null)
        {
            playerBody = transform.parent;
        }

        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }
    }

    void Update()
    {
        // Don't rotate player view when a menu is open
        if (PanelControl.inventoryOpen || PanelControl.craftingOpen)
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Control Camera-only horizontal rotation
        xRotation -= mouseY;

        // Clamp the rotation so we can't over-rotate (like in real life)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply vertical rotation to camera
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Apply horizontal rotation to player body only
        playerBody.Rotate(Vector3.up * mouseX);
    }
}