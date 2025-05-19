using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.InputSystem;
using System.Reflection;
using System;

public class ModeSwitcher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ControllerInputActionManager inputManager;
    [SerializeField] private Transform playerTransform;

    [Header("Settings")]
    [SerializeField] private float continuousRotationThreshold = 0.5f;
    [SerializeField] private float snapRotationAngle = 45f;
    [SerializeField] private float continuousRotationSpeed = 30f; // degrees per second

    // State variables
    private bool isSmoothMotionEnabled = false; // Start with teleport mode active
    private bool isNearFarTeleportEnabled = true; // Start with teleport enabled
    private bool isSmoothTurnEnabled = false;   // Start with snap turn active
    private float joystickHoldTime = 0f;
    private bool wasJoystickPressedForRotation = false;
    private Vector2 lastJoystickAxisValue = Vector2.zero;

    // Input Actions
    private InputAction m_LeftJoystickClickAction;
    private InputAction m_LeftJoystickAxisAction;

    private void Awake()
    {
        if (inputManager == null)
        {
            Debug.LogError("ModeSwitcher: ControllerInputActionManager reference is missing!", this);
            enabled = false;
            return;
        }

        if (playerTransform == null)
        {
            playerTransform = transform;
            Debug.LogWarning("ModeSwitcher: Player Transform not assigned, using this object's transform.", this);
        }

        // Get the input actions from the XR Controller
        var leftController = new InputActionMap("XRI LeftHand");
        
        // Set up joystick click action
        m_LeftJoystickClickAction = new InputAction("JoystickClick", binding: "<XRController>{LeftHand}/thumbstickClicked");
        m_LeftJoystickClickAction.Enable();

        // Set up joystick axis action
        m_LeftJoystickAxisAction = new InputAction("JoystickAxis", binding: "<XRController>{LeftHand}/primary2DAxis");
        m_LeftJoystickAxisAction.Enable();

        // Set initial state
        UpdateLocomotionModes();
    }

    private void OnEnable()
    {
        if (m_LeftJoystickClickAction != null)
        {
            m_LeftJoystickClickAction.performed += OnJoystickClick;
        }

        if (m_LeftJoystickAxisAction != null)
        {
            m_LeftJoystickAxisAction.performed += OnJoystickAxisPerformed;
            m_LeftJoystickAxisAction.canceled += OnJoystickAxisCanceled;
        }

        // Set initial state
        UpdateLocomotionModes();
    }

    private void OnDisable()
    {
        if (m_LeftJoystickClickAction != null)
        {
            m_LeftJoystickClickAction.performed -= OnJoystickClick;
        }

        if (m_LeftJoystickAxisAction != null)
        {
            m_LeftJoystickAxisAction.performed -= OnJoystickAxisPerformed;
            m_LeftJoystickAxisAction.canceled -= OnJoystickAxisCanceled;
        }
    }

    private void Update()
    {
        // Handle continuous rotation timing only when joystick is held left/right in teleport mode
        if (!isSmoothMotionEnabled && wasJoystickPressedForRotation)
        {
            joystickHoldTime += Time.deltaTime;
            if (!isSmoothTurnEnabled && joystickHoldTime >= continuousRotationThreshold)
            {
                isSmoothTurnEnabled = true;
                UpdateLocomotionModes();
            }
        }
        // Perform continuous rotation if enabled
        if (!isSmoothMotionEnabled && isSmoothTurnEnabled)
        {
            float horizontalInput = lastJoystickAxisValue.x;
            if (Mathf.Abs(horizontalInput) > 0.5f)
            {
                float rotationAmount = Mathf.Sign(horizontalInput) * continuousRotationSpeed * Time.deltaTime;
                if (playerTransform != null)
                    playerTransform.Rotate(0f, rotationAmount, 0f, Space.World);
            }
        }
    }

    private void OnJoystickClick(InputAction.CallbackContext context)
    {
        Debug.Log("Joystick Clicked - Toggling Mode");
        
        // Toggle smooth motion mode
        isSmoothMotionEnabled = !isSmoothMotionEnabled;
        
        // Toggle near-far teleport (inverse of smooth motion)
        isNearFarTeleportEnabled = !isSmoothMotionEnabled;
        
        // Reset turn mode when switching
        isSmoothTurnEnabled = false;
        
        // Update all modes
        UpdateLocomotionModes();
        
        Debug.Log($"Mode Switched - Smooth Motion: {isSmoothMotionEnabled}, Near-Far Teleport: {isNearFarTeleportEnabled}");
    }

    private void OnJoystickAxisPerformed(InputAction.CallbackContext context)
    {
        if (!isSmoothMotionEnabled) // Only handle rotation in teleport mode
        {
            lastJoystickAxisValue = context.ReadValue<Vector2>();
            float horizontalInput = lastJoystickAxisValue.x;
            // Only consider left/right movement for rotation
            if (Mathf.Abs(horizontalInput) > 0.5f)
            {
                if (!wasJoystickPressedForRotation)
                {
                    wasJoystickPressedForRotation = true;
                    joystickHoldTime = 0f;
                }
            }
            else
            {
                // If joystick returns to center or is not left/right, reset
                wasJoystickPressedForRotation = false;
                joystickHoldTime = 0f;
                //isSmoothTurnEnabled = false;
                UpdateLocomotionModes();
            }
        }
    }

    private void OnJoystickAxisCanceled(InputAction.CallbackContext context)
    {
        if (!isSmoothMotionEnabled)
        {
            // Reset rotation state
            wasJoystickPressedForRotation = false;
            joystickHoldTime = 0f;
            isSmoothTurnEnabled = false;
            UpdateLocomotionModes();
            lastJoystickAxisValue = Vector2.zero;
        }
    }

    private void PerformSnapRotation(Vector2 axisValue)
    {
        if (playerTransform == null) return;

        float horizontalInput = axisValue.x;
        if (Mathf.Abs(horizontalInput) > 0.5f)
        {
            float rotationAmount = Mathf.Sign(horizontalInput) * snapRotationAngle;
            playerTransform.Rotate(0f, rotationAmount, 0f, Space.World);
        }
    }

    private void UpdateLocomotionModes()
    {
        if (inputManager == null) return;

        // Update smooth motion and turn settings
        inputManager.smoothMotionEnabled = isSmoothMotionEnabled;
        inputManager.smoothTurnEnabled = isSmoothTurnEnabled;

        // Update near-far teleport setting using reflection
        try
        {
            FieldInfo nearFarField = typeof(ControllerInputActionManager).GetField(
                "m_NearFarEnableTeleportDuringNearInteraction",
                BindingFlags.NonPublic | BindingFlags.Instance);
                
            if (nearFarField != null)
            {
                nearFarField.SetValue(inputManager, isNearFarTeleportEnabled);
                Debug.Log($"Updated Near-Far Teleport setting to: {isNearFarTeleportEnabled}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to update near-far teleport setting: {e.Message}");
        }
    }
}