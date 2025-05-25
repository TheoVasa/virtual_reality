using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    [Tooltip("The flashlight GameObject to toggle.")]
    public GameObject flashlightObject;

    [Tooltip("The InputActionReference for the button that activates/deactivates the flashlight.")]
    public InputActionReference toggleButtonAction;

    private bool isFlashlightOn = false;

    void Awake()
    {
        // Ensure the flashlight is initially off, or sync with its current state
        if (flashlightObject != null)
        {
            isFlashlightOn = flashlightObject.activeSelf;
        }
        else
        {
            Debug.LogError("Flashlight GameObject not assigned in the Inspector!");
            enabled = false; // Disable the script if no flashlight is assigned
            return;
        }

        if (toggleButtonAction == null)
        {
            Debug.LogError("Toggle Button Action not assigned in the Inspector!");
            enabled = false; // Disable the script if no action is assigned
            return;
        }
    }

    void OnEnable()
    {
        toggleButtonAction.action.performed += OnToggleButtonPressed;
        toggleButtonAction.action.Enable();
    }

    void OnDisable()
    {
        toggleButtonAction.action.performed -= OnToggleButtonPressed;
        toggleButtonAction.action.Disable();
    }

    private void OnToggleButtonPressed(InputAction.CallbackContext context)
    {
        if (flashlightObject != null)
        {
            isFlashlightOn = !isFlashlightOn;
            flashlightObject.SetActive(isFlashlightOn);
        }
    }

    // Optional: To ensure the flashlight state is correct at the start
    void Start()
    {
        if (flashlightObject != null)
        {
            flashlightObject.SetActive(isFlashlightOn);
        }
    }
} 