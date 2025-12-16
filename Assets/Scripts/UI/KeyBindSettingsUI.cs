using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyBindSettingsUI : MonoBehaviour
{
    public Button moveForwardButton;
    public Button moveForwardResetButton;

    public Button moveBackwardButton;
    public Button moveBackwardResetButton;

    public Button moveLeftButton;
    public Button moveLeftResetButton;

    public Button moveRightButton;
    public Button moveRightResetButton;

    public Button sprintButton;
    public Button sprintResetButton;

    public Button crouchButton;
    public Button crouchResetButton;

    public Button jumpButton;
    public Button jumpResetButton;

    public Button interactButton;
    public Button interactResetButton;

    public Button attackButton;
    public Button attackResetButton;

    public Button useButton;
    public Button useResetButton;

    public Button inventoryButton;
    public Button inventoryResetButton;

    public Button pauseButton;
    public Button pauseResetButton;

    // Dictionary to track button references
    private Dictionary<KeyBindController.GameAction, Button> keyBindButtons = new Dictionary<KeyBindController.GameAction, Button>();

    // Currently rebinding
    private KeyBindController.GameAction? currentlyRebinding = null;

    // Check if buttons are initialized
    private bool isInitialized = false;

    void OnEnable()
    {
        // Reinitialize when the menu is enabled
        isInitialized = false;
        InitializeButtons();
    }

    // Initialize button references and listeners
    private void InitializeButtons()
    {
        // Validate KeyBindController
        if (KeyBindController.Instance == null)
        {
            Debug.LogError("KeyBindController not found in scene!");
            return;
        }

        foreach (var kvp in keyBindButtons)
        {
            kvp.Value.onClick.RemoveAllListeners();
        }

        keyBindButtons.Clear();

        // Setup movement binds
        SetupKeybindButton(KeyBindController.GameAction.moveForward, moveForwardButton, moveForwardResetButton);
        SetupKeybindButton(KeyBindController.GameAction.moveBackward, moveBackwardButton, moveBackwardResetButton);
        SetupKeybindButton(KeyBindController.GameAction.moveLeft, moveLeftButton, moveLeftResetButton);
        SetupKeybindButton(KeyBindController.GameAction.moveRight, moveRightButton, moveRightResetButton);

        // Setup action binds
        SetupKeybindButton(KeyBindController.GameAction.jump, jumpButton, jumpResetButton);
        SetupKeybindButton(KeyBindController.GameAction.sprint, sprintButton, sprintResetButton);
        SetupKeybindButton(KeyBindController.GameAction.crouch, crouchButton, crouchResetButton);

        // Setup interaction binds
        SetupKeybindButton(KeyBindController.GameAction.interact, interactButton, interactResetButton);
        SetupKeybindButton(KeyBindController.GameAction.attack, attackButton, attackResetButton);
        SetupKeybindButton(KeyBindController.GameAction.use, useButton, useResetButton);
        SetupKeybindButton(KeyBindController.GameAction.inventory, inventoryButton, inventoryResetButton);

        // Setup menu binds
        SetupKeybindButton(KeyBindController.GameAction.pause, pauseButton, pauseResetButton);

        isInitialized = true;
        Debug.Log("KeyBindSettingsUI initialized.");
    }

    // Initialize a keybind button pair (change button + reset button)
    private void SetupKeybindButton(KeyBindController.GameAction action, Button keyButton, Button resetButton)
    {
        // Validate buttons are assigned
        if (keyButton == null)
        {
            Debug.LogError($"Key button not assigned for action: {action}");
            return;
        }

        if (resetButton == null)
        {
            Debug.LogError($"Reset button not assigned for action: {action}");
            return;
        }

        // Update button text to show current binding
        UpdateKeyButtonText(action, keyButton);

        // Store button reference
        keyBindButtons[action] = keyButton;

        // Setup button click listeners
        keyButton.onClick.AddListener(() => StartRebinding(action, keyButton));
        resetButton.onClick.AddListener(() => ResetBinding(action, keyButton));

        Debug.Log($"Setup keybind button for: {action}");
    }

    // Listen for a key press to rebind a key
    private void StartRebinding(KeyBindController.GameAction action, Button button)
    {
        currentlyRebinding = action;
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = "Press any key...";
        }
        button.interactable = false;
        Debug.Log($"Started rebinding: {action}");
    }

    // Update button's text to show the current keybind
    private void UpdateKeyButtonText(KeyBindController.GameAction action, Button button)
    {
        KeyCode boundKey = KeyBindController.Instance.GetBoundKey(action);
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = boundKey.ToString();
            Debug.Log($"{action} bound to: {boundKey}");
        }
    }

    // Reset a keybind
    private void ResetBinding(KeyBindController.GameAction action, Button button)
    {
        KeyBindController.Instance.ResetKeyBinding(action);
        UpdateKeyButtonText(action, button);
        Debug.Log($"Reset keybind: {action}");
    }

    // Use LateUpdate instead of Update so it runs even when Time.timeScale = 0
    void LateUpdate()
    {
        // Skip if not initialized
        if (!isInitialized)
        {
            return;
        }

        // Listen for any key press if currently rebinding
        if (currentlyRebinding != null)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    KeyBindController.GameAction action = currentlyRebinding.Value;
                    Debug.Log($"Key pressed: {keyCode}");

                    // Check for conflicts
                    if (KeyBindController.Instance.IsKeyAlreadyBound(keyCode, action))
                    {
                        Debug.LogWarning($"{keyCode} is already bound.");
                        StartCoroutine(ShowConflictWarning(keyBindButtons[action]));
                    }
                    else
                    {
                        // Apply new binding
                        KeyBindController.Instance.SetKeyBinding(action, keyCode);
                        UpdateKeyButtonText(action, keyBindButtons[action]);
                        Debug.Log($"Rebound {action} to {keyCode}");
                    }

                    // Stop rebinding
                    keyBindButtons[action].interactable = true;
                    currentlyRebinding = null;
                    break;
                }
            }
        }
    }

    // Conflict warning message
    private IEnumerator ShowConflictWarning(Button button)
    {
        TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
        if (text == null)
        {
            yield break;
        }

        string originalText = text.text;

        // Show warning
        text.text = "Key taken!";
        button.interactable = false;

        yield return new WaitForSeconds(1f);

        // Restore
        text.text = originalText;
        button.interactable = true;
        
        button.OnDeselect(null);
    }
}