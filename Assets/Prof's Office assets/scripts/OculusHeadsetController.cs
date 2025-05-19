using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OculusHeadsetController : MonoBehaviour
{
    [SerializeField] private Transform headsetTransform;
    [SerializeField] private Camera xrCamera;
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private float activationDistance = 0.3f; // Distance in meters
    
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;
    private bool specialModeActive = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the grab interactable component or add it if it doesn't exist
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable == null)
        {
            grabInteractable = gameObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        }
        
        // Subscribe to grab events
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
        
        // Ensure the object starts deactivated
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbed && !specialModeActive)
        {
            CheckHeadsetProximity();
        }
    }
    
    private void CheckHeadsetProximity()
    {
        if (xrCamera == null || headsetTransform == null) return;
        
        // Calculate distance between headset and camera (user's head)
        float distance = Vector3.Distance(headsetTransform.position, xrCamera.transform.position);
        
        // If close enough, activate special mode
        if (distance <= activationDistance)
        {
            ActivateSpecialMode();
        }
    }
    
    private void ActivateSpecialMode()
    {
        specialModeActive = true;
        
        // Hide the headset
        if (headsetTransform != null)
        {
            headsetTransform.gameObject.SetActive(false);
        }
        
        // Activate the object
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
    }
    
    private void DeactivateSpecialMode()
    {
        specialModeActive = false;
        
        // Show the headset
        if (headsetTransform != null)
        {
            headsetTransform.gameObject.SetActive(true);
        }
        
        // Deactivate the object
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false);
        }
    }
    
    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }
    
    private void OnReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;
        
        // If special mode was active, deactivate it
        if (specialModeActive)
        {
            DeactivateSpecialMode();
        }
    }
    
    // Called when the script is disabled or destroyed
    private void OnDisable()
    {
        // Unsubscribe from events to prevent memory leaks
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }
}
