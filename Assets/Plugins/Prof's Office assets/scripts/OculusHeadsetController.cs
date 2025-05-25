using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OculusHeadsetController : MonoBehaviour
{
    [SerializeField] private Transform headsetTransform;
    [SerializeField] private Camera xrCamera;
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private float activationDistance = 0.3f; // Distance in meters
    [SerializeField] private AudioSource activationSound; // AudioSource to play when activated
    [SerializeField] private AudioSource grabSound; // AudioSource to play when grabbed

    [Header("Door Settings")]
    [SerializeField] private Transform doorTransform;
    [SerializeField] private Transform doorRotationAnchor;
    [SerializeField] private float doorRotationAngle = -90f; // Degrees to rotate
    [SerializeField] private float doorRotationDuration = 1.0f; // Duration of rotation in seconds
    
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;
    private bool specialModeActive = false;
    private bool grabSoundPlayed = false; // Flag to track if grab sound has been played
    
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
        grabSoundPlayed = false; // Initialize the flag
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

        // Play activation sound
        if (activationSound != null && activationSound.clip != null)
        {
            activationSound.Play();
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
        
        // Play grab sound if it hasn't been played yet
        if (grabSound != null && grabSound.clip != null && !grabSoundPlayed)
        {
            grabSound.Play();
            grabSoundPlayed = true;
        }
        
        if (doorTransform != null && doorRotationAnchor != null && doorRotationDuration > 0f)
        {
            StartCoroutine(RotateDoorOverTimeCoroutine(doorRotationAngle, doorRotationDuration));
        }
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

    private System.Collections.IEnumerator RotateDoorOverTimeCoroutine(float totalAngle, float duration)
    {
        if (doorTransform == null || doorRotationAnchor == null) // Duration check is done by caller
            yield break;

        float elapsedTime = 0f;
        float rotatedSoFar = 0f;
        // Assuming the door rotates around the 'up' vector of the anchor (like a hinge)
        Vector3 rotationAxis = doorRotationAnchor.up; 

        while (elapsedTime < duration)
        {
            float angleThisFrame = (totalAngle / duration) * Time.deltaTime;

            // Prevent overshooting the target angle
            if (Mathf.Abs(rotatedSoFar + angleThisFrame) > Mathf.Abs(totalAngle))
            {
                angleThisFrame = totalAngle - rotatedSoFar;
            }
            
            doorTransform.RotateAround(doorRotationAnchor.position, rotationAxis, angleThisFrame);
            rotatedSoFar += angleThisFrame;
            elapsedTime += Time.deltaTime;

            if (Mathf.Approximately(rotatedSoFar, totalAngle))
            {
                break; // Exact rotation achieved
            }
            yield return null;
        }

        // Apply any minor correction if the loop finished due to time, to ensure exact final rotation
        float finalCorrection = totalAngle - rotatedSoFar;
        if (!Mathf.Approximately(finalCorrection, 0f))
        {
            doorTransform.RotateAround(doorRotationAnchor.position, rotationAxis, finalCorrection);
        }
    }
}
