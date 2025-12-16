using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f * 2 * 1.5f;
    public float jumpHeight = 6f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float accelerationTime = 0.1f;
    public float decelerationTime = 0.15f;

    Vector3 velocity;
    Vector3 currentVelocity;
    Vector3 velocitySmoothing; // Required for SmoothDamp

    public bool isGrounded;

    // Slope handling
    public float slopeForce = 0f;
    public float maxSlopeAngle = 45f;

    void Update()
    {
        // STOP FALLING THROUGH THE FLOOR PLEASE
        if (!Application.isFocused || Time.timeScale == 0f)
        {
            currentVelocity = Vector3.zero;
            velocitySmoothing = Vector3.zero;
            velocity.y = 0f;

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded)
            {
                controller.Move(new Vector3(0f, 0.2f, 0f));
            }

            return;
        }

        // Check if grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Slope drag to prevent sliding and ensure grounding
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            slopeForce = 0f;
        }

        // Get movement input from custom key binds
        float x = 0f;
        float z = 0f;

        if (KeyBindController.Instance.IsActionHeld(KeyBindController.GameAction.moveRight))
            x += 1f;
        if (KeyBindController.Instance.IsActionHeld(KeyBindController.GameAction.moveLeft))
            x -= 1f;

        if (KeyBindController.Instance.IsActionHeld(KeyBindController.GameAction.moveForward))
            z += 1f;
        if (KeyBindController.Instance.IsActionHeld(KeyBindController.GameAction.moveBackward))
            z -= 1f;

        // Normalize input to prevent faster diagonal movement
        Vector3 inputDirection = new Vector3(x, 0f, z).normalized;
        Vector3 desiredVelocity = (transform.right * inputDirection.x + transform.forward * inputDirection.z) * speed;

        // Smoothen acceleration and decceleration
        float smoothTime = inputDirection.magnitude > 0f ? accelerationTime : decelerationTime;
        currentVelocity = Vector3.SmoothDamp(currentVelocity, desiredVelocity, ref velocitySmoothing, smoothTime);

        // Apply smoothed horizontal movement
        controller.Move(currentVelocity * Time.deltaTime);

        // Check if the player should jump
        if (KeyBindController.Instance.IsActionPressed(KeyBindController.GameAction.jump) && isGrounded)
        {
            // The equation for jumping
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}