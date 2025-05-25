using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using System.Reflection;

[RequireComponent(typeof(XRInputModalityManager))]
public class XRInputModalityForcer : MonoBehaviour
{
    [Header("Controller Settings")]
    [SerializeField] private bool forceLeftController = true;
    [SerializeField] private bool forceRightController = true;

    [Header("Hand Tracking Settings")]
    [SerializeField] private bool forceLeftHand = true;
    [SerializeField] private bool forceRightHand = true;

    private XRInputModalityManager inputModalityManager;
    private GameObject leftController;
    private GameObject rightController;
    private GameObject leftHand;
    private GameObject rightHand;

    // Fields to access private members of XRInputModalityManager
    private FieldInfo leftInputModeField;
    private FieldInfo rightInputModeField;
    private FieldInfo currentInputModeField;

    private void Awake()
    {
        inputModalityManager = GetComponent<XRInputModalityManager>();
        
        // Get references to the GameObjects
        leftController = inputModalityManager.leftController;
        rightController = inputModalityManager.rightController;
        leftHand = inputModalityManager.leftHand;
        rightHand = inputModalityManager.rightHand;

        // Get private fields using reflection
        leftInputModeField = typeof(XRInputModalityManager).GetField("m_LeftInputMode", BindingFlags.NonPublic | BindingFlags.Instance);
        rightInputModeField = typeof(XRInputModalityManager).GetField("m_RightInputMode", BindingFlags.NonPublic | BindingFlags.Instance);
        currentInputModeField = typeof(XRInputModalityManager).GetField("s_CurrentInputMode", BindingFlags.NonPublic | BindingFlags.Static);
    }

    private void OnEnable()
    {
        // Subscribe to the input modality manager's events
        inputModalityManager.trackedHandModeStarted.AddListener(OnTrackedHandModeStarted);
        inputModalityManager.trackedHandModeEnded.AddListener(OnTrackedHandModeEnded);
        inputModalityManager.motionControllerModeStarted.AddListener(OnMotionControllerModeStarted);
        inputModalityManager.motionControllerModeEnded.AddListener(OnMotionControllerModeEnded);
    }

    private void OnDisable()
    {
        // Unsubscribe from the input modality manager's events
        inputModalityManager.trackedHandModeStarted.RemoveListener(OnTrackedHandModeStarted);
        inputModalityManager.trackedHandModeEnded.RemoveListener(OnTrackedHandModeEnded);
        inputModalityManager.motionControllerModeStarted.RemoveListener(OnMotionControllerModeStarted);
        inputModalityManager.motionControllerModeEnded.RemoveListener(OnMotionControllerModeEnded);
    }

    private void OnTrackedHandModeStarted()
    {
        // Force hand tracking GameObjects to be active based on settings
        if (forceLeftHand && leftHand != null)
            leftHand.SetActive(true);
        if (forceRightHand && rightHand != null)
            rightHand.SetActive(true);

        // Override input modes
        UpdateInputModes();
    }

    private void OnTrackedHandModeEnded()
    {
        // Force hand tracking GameObjects to be active based on settings
        if (forceLeftHand && leftHand != null)
            leftHand.SetActive(true);
        if (forceRightHand && rightHand != null)
            rightHand.SetActive(true);

        // Override input modes
        UpdateInputModes();
    }

    private void OnMotionControllerModeStarted()
    {
        // Force controller GameObjects to be active based on settings
        if (forceLeftController && leftController != null)
            leftController.SetActive(true);
        if (forceRightController && rightController != null)
            rightController.SetActive(true);

        // Override input modes
        UpdateInputModes();
    }

    private void OnMotionControllerModeEnded()
    {
        // Force controller GameObjects to be active based on settings
        if (forceLeftController && leftController != null)
            leftController.SetActive(true);
        if (forceRightController && rightController != null)
            rightController.SetActive(true);

        // Override input modes
        UpdateInputModes();
    }

    private void Update()
    {
        // Continuously force the GameObjects to be active based on settings
        if (forceLeftController && leftController != null)
            leftController.SetActive(true);
        if (forceRightController && rightController != null)
            rightController.SetActive(true);
        if (forceLeftHand && leftHand != null)
            leftHand.SetActive(true);
        if (forceRightHand && rightHand != null)
            rightHand.SetActive(true);

        // Continuously override input modes
        UpdateInputModes();
    }

    private void UpdateInputModes()
    {
        // Get current input modes
        var leftMode = (XRInputModalityManager.InputMode)leftInputModeField.GetValue(inputModalityManager);
        var rightMode = (XRInputModalityManager.InputMode)rightInputModeField.GetValue(inputModalityManager);

        // Override left mode if needed
        if (forceLeftController && forceLeftHand)
        {
            // If both are forced, keep the current mode
        }
        else if (forceLeftController)
        {
            leftMode = XRInputModalityManager.InputMode.MotionController;
        }
        else if (forceLeftHand)
        {
            leftMode = XRInputModalityManager.InputMode.TrackedHand;
        }

        // Override right mode if needed
        if (forceRightController && forceRightHand)
        {
            // If both are forced, keep the current mode
        }
        else if (forceRightController)
        {
            rightMode = XRInputModalityManager.InputMode.MotionController;
        }
        else if (forceRightHand)
        {
            rightMode = XRInputModalityManager.InputMode.TrackedHand;
        }

        // Set the input modes
        leftInputModeField.SetValue(inputModalityManager, leftMode);
        rightInputModeField.SetValue(inputModalityManager, rightMode);

        // Update the current input mode to allow both
        var currentInputMode = (XRInputModalityManager.InputMode)currentInputModeField.GetValue(null);
        if (currentInputMode == XRInputModalityManager.InputMode.None)
        {
            // If no mode is set, set it to the first active mode
            if (leftMode != XRInputModalityManager.InputMode.None)
                currentInputModeField.SetValue(null, leftMode);
            else if (rightMode != XRInputModalityManager.InputMode.None)
                currentInputModeField.SetValue(null, rightMode);
        }
    }
} 