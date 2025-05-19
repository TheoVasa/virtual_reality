using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    [Tooltip("The flashlight GameObject to toggle.")]
    public GameObject flashlightObject;

    private InputAction triggerAction;
    private bool isFlashlightOn = false;

    void Awake()
    {
        if (flashlightObject == null)
        {
            Debug.LogError("Flashlight GameObject not assigned in the Inspector! Disabling script.", this);
            enabled = false; // Disable the script if no flashlight is assigned
            return;
        }
        // Initialize flashlight state based on its state in the editor
        isFlashlightOn = flashlightObject.activeSelf;

        // Setup the input action for the left controller's trigger
        triggerAction = new InputAction(name: "FlashlightToggleTrigger", type: InputActionType.Button, binding: "<XRController>{LeftHand}/trigger");
        // Optional: Add an interaction to ensure it fires once on press, not based on value exceeding a threshold continuously.
        // 'Press' with default parameters means it triggers when the control is pressed and releases when released.
        // For a toggle, we usually care about the 'performed' event which fires on press.
        // Using just the binding often works well for triggers as their value goes from 0 to 1.
        // A "Press" interaction with default behavior (fires on press, releases on release) might be suitable if we use context.started.
        // For context.performed, it usually fires when the action completes (e.g. button fully pressed and released or passes a threshold).
        // For triggers, 'performed' typically fires when the trigger value passes the 'Press Point' (default 0.5f for Interaction "Press")
        // Let's rely on the default trigger behavior which should work for 'performed'.
        // If more specific behavior like only on press down is needed, interactions can be added:
        // triggerAction.AddInteraction("Press(behavior=PressOnly)"); // Requires Unity Input System 1.1+ for behavior enum
    }

    void OnEnable()
    {
        if (triggerAction != null)
        {
            triggerAction.performed += OnToggleButtonPressed;
            triggerAction.Enable();
        }
        else if (flashlightObject == null) // If Awake failed, triggerAction might be null
        {
            // Already logged in Awake, and script should be disabled.
            // This OnEnable might be called before Awake fully disables it.
            return;
        }
    }

    void OnDisable()
    {
        if (triggerAction != null)
        {
            triggerAction.performed -= OnToggleButtonPressed;
            triggerAction.Disable();
        }
    }

    void OnDestroy()
    {
        // Clean up the action when the GameObject is destroyed
        triggerAction?.Dispose();
    }

    private void OnToggleButtonPressed(InputAction.CallbackContext context)
    {
        if (flashlightObject != null)
        {
            isFlashlightOn = !isFlashlightOn;
            flashlightObject.SetActive(isFlashlightOn);
            Debug.Log($"Flashlight Toggled. Is On: {isFlashlightOn}"); // Added for debugging
        }
    }

    void Start()
    {
        // Ensure the flashlight's visual state matches our tracked state at start.
        // This is important if isFlashlightOn was set by its activeSelf in Awake.
        if (flashlightObject != null)
        {
            flashlightObject.SetActive(isFlashlightOn);
        }
    }
}
