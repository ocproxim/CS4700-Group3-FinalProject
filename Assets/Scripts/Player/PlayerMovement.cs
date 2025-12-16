using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f * 2 * 1.5f;
    public float jumpHeight = 6f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    public bool isGrounded;

    // Slope handling
    public float slopeForce = 0f;
    public float maxSlopeAngle = 45f;

    // Track previous timeScale to detect pause/resume transitions
    public float previousTimeScale = 1f;

    void Update()
    {
        // Detect pause/resume transition and reset velocity
        if (Time.timeScale > 0f && previousTimeScale == 0f)
        {
            // Game just resumed - reset downward velocity to prevent phase-through
            velocity.y = -2f;
        }
        previousTimeScale = Time.timeScale;

        // Don't process movement when game is paused
        if (Time.timeScale == 0f)
        {
            return;
        }

        // Check if grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Apply slope drag to prevent sliding and ensure grounding
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

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

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