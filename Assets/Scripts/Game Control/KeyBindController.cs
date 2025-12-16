using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindController : MonoBehaviour
{
    public enum GameAction
    {
        moveForward,
        moveBackward,
        moveLeft,
        moveRight,

        sprint,
        crouch,
        jump,

        interact,
        attack,
        use,
        inventory,

        pause
    }

    // Store default key bind maps
    private Dictionary<GameAction, KeyCode> keyBindings;

    /// Store user key binds
    private Dictionary<GameAction, KeyCode> customKeyBindings;

    public static KeyBindController instance;

    public static KeyBindController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KeyBindController>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("KeyBindController");
                    instance = obj.AddComponent<KeyBindController>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Initialize both binding sets
        InitializeDefaultBindings();
        LoadCustomBindings();
        
        Debug.Log("KeyBindController initialized.");
    }

    // Initialize default binds
    private void InitializeDefaultBindings()
    {
        keyBindings = new Dictionary<GameAction, KeyCode>
        {
            { GameAction.moveForward, KeyCode.W },
            { GameAction.moveBackward, KeyCode.S },
            { GameAction.moveLeft, KeyCode.A },
            { GameAction.moveRight, KeyCode.D },
            { GameAction.jump, KeyCode.Space },
            { GameAction.sprint, KeyCode.LeftShift },
            { GameAction.interact, KeyCode.E },
            { GameAction.pause, KeyCode.Escape },
            { GameAction.inventory, KeyCode.I },
            { GameAction.attack, KeyCode.Mouse0 },
            { GameAction.use, KeyCode.Mouse1 },
            { GameAction.crouch, KeyCode.LeftControl }
        };

        Debug.Log($"Default key bindings initialized. Count: {keyBindings.Count}");
    }

    // Checks if a bound key is held
    public bool IsActionHeld(GameAction action)
    {
        KeyCode key = GetBoundKey(action);
        return Input.GetKey(key);
    }

    // Checks if a bound key is pressed
    public bool IsActionPressed(GameAction action)
    {
        KeyCode key = GetBoundKey(action);
        return Input.GetKeyDown(key);
    }

    /// Checks if a bound key is released
    public bool IsActionReleased(GameAction action)
    {
        KeyCode key = GetBoundKey(action);
        return Input.GetKeyUp(key);
    }

    // Gets the currently bound key for an action
    public KeyCode GetBoundKey(GameAction action)
    {
        // Ensure dictionaries are initialized
        if (keyBindings == null || keyBindings.Count == 0)
        {
            Debug.LogError("Key binds not initialized!");
            InitializeDefaultBindings();
        }

        // Check custom bindings first
        if (customKeyBindings != null && customKeyBindings.ContainsKey(action))
        {
            return customKeyBindings[action];
        }

        // Fall back to default bindings
        if (keyBindings != null && keyBindings.ContainsKey(action))
        {
            return keyBindings[action];
        }

        Debug.LogWarning($"No key binding found for action: {action}");
        return KeyCode.None;
    }

    // Set a custom key bind
    public void SetKeyBinding(GameAction action, KeyCode newKey)
    {
        if (customKeyBindings == null)
        {
            customKeyBindings = new Dictionary<GameAction, KeyCode>();
        }

        if (customKeyBindings.ContainsKey(action))
        {
            customKeyBindings[action] = newKey;
        }
        else
        {
            customKeyBindings.Add(action, newKey);
        }

        SaveCustomBindings();
        Debug.Log($"Set keybinding: {action} -> {newKey}");
    }

    // Reset a key bind to its default value
    public void ResetKeyBinding(GameAction action)
    {
        // Remove from custom bindings dictionary
        if (customKeyBindings != null && customKeyBindings.ContainsKey(action))
        {
            customKeyBindings.Remove(action);
        }

        // Remove from PlayerPrefs
        string playerPrefsKey = $"KeyBind_{action}";
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            PlayerPrefs.DeleteKey(playerPrefsKey);
        }

        PlayerPrefs.Save();
        Debug.Log($"Reset keybinding: {action} to default {keyBindings[action]}");
    }

    // Reset all key bindings to default values
    public void ResetAllKeyBindings()
    {
        if (customKeyBindings != null)
        {
            customKeyBindings.Clear();
        }

        // Clear PlayerPrefs keybinding entries
        foreach (GameAction action in System.Enum.GetValues(typeof(GameAction)))
        {
            string playerPrefsKey = $"KeyBind_{action}";
            if (PlayerPrefs.HasKey(playerPrefsKey))
            {
                PlayerPrefs.DeleteKey(playerPrefsKey);
            }
        }

        PlayerPrefs.Save();
        Debug.Log("All keybindings reset to defaults.");
    }

    /// Save current key binds
    private void SaveCustomBindings()
    {
        if (customKeyBindings == null)
        {
            return;
        }

        foreach (var binding in customKeyBindings)
        {
            PlayerPrefs.SetInt($"KeyBind_{binding.Key}", (int)binding.Value);
        }

        PlayerPrefs.Save();
    }

    // Load saved key bindings
    private void LoadCustomBindings()
    {
        customKeyBindings = new Dictionary<GameAction, KeyCode>();

        foreach (GameAction action in System.Enum.GetValues(typeof(GameAction)))
        {
            string key = $"KeyBind_{action}";
            if (PlayerPrefs.HasKey(key))
            {
                int keyCodeValue = PlayerPrefs.GetInt(key);
                customKeyBindings[action] = (KeyCode)keyCodeValue;
            }
        }

        Debug.Log($"Custom key bindings loaded. Count: {customKeyBindings.Count}");
    }

    // Get all current key binds
    public Dictionary<GameAction, KeyCode> GetAllKeyBindings()
    {
        Dictionary<GameAction, KeyCode> allBindings = new Dictionary<GameAction, KeyCode>(keyBindings);

        if (customKeyBindings != null)
        {
            foreach (var customBinding in customKeyBindings)
            {
                allBindings[customBinding.Key] = customBinding.Value;
            }
        }

        return allBindings;
    }

    // Check if a key is already bound
    public bool IsKeyAlreadyBound(KeyCode key, GameAction excludeAction)
    {
        foreach (var binding in GetAllKeyBindings())
        {
            if (binding.Key != excludeAction && binding.Value == key)
            {
                return true;
            }
        }

        return false;
    }
}