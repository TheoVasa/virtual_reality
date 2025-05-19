using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ZipLineController : MonoBehaviour
{
    [Header("Zip Line Settings")]
    [SerializeField] private float slideSpeed = 5f;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    
    private Transform xrOrigin;
    private Transform grabberHand;
    private bool isGrabbed = false;
    private float journeyLength;
    private float distanceCovered;
    private bool reachedEnd = false;
    private Vector3 handOffsetFromOrigin;
    
    void Awake()
    {
        // If startPoint or endPoint are not set, use the cylinder's ends
        if (startPoint == null)
            startPoint = transform.Find("StartPoint");
        if (endPoint == null)
            endPoint = transform.Find("EndPoint");
            
        // Make sure the grab interactable component is assigned
        if (grabInteractable == null)
            grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            
        // Calculate the total journey length once
        journeyLength = Vector3.Distance(startPoint.position, endPoint.position);
        
        // Find the XR Origin in the scene
        xrOrigin = FindObjectOfType<Unity.XR.CoreUtils.XROrigin>()?.transform;
        if (xrOrigin == null)
        {
            Debug.LogError("Could not find XR Origin in the scene!");
        }
    }
    
    void Start()
    {
        // Subscribe to grab events
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
        else
        {
            Debug.LogError("XRGrabInteractable component not found on the zip line object!");
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (xrOrigin == null) return;
        
        isGrabbed = true;
        reachedEnd = false;
        grabberHand = args.interactorObject.transform;
        
        // Calculate the offset between the hand and the XR Origin
        handOffsetFromOrigin = grabberHand.position - xrOrigin.position;
        
        // Reset distance if we're starting a new grab
        distanceCovered = 0f;
        
        // Move the XR Origin to the start, maintaining the same offset between hand and origin
        Vector3 initialPosition = startPoint.position - handOffsetFromOrigin;
        xrOrigin.position = initialPosition;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        grabberHand = null;
    }

    void Update()
    {
        if (isGrabbed && xrOrigin != null && grabberHand != null && !reachedEnd)
        {
            // Calculate how far we've moved
            distanceCovered += slideSpeed * Time.deltaTime;
            
            // Calculate the fraction of the journey completed
            float fractionOfJourney = distanceCovered / journeyLength;
            
            // Ensure we don't go past the end
            if (fractionOfJourney > 1f)
            {
                fractionOfJourney = 1f;
                reachedEnd = true;
            }
            
            // Calculate the new position for the hand based on the journey fraction
            Vector3 newHandPosition = Vector3.Lerp(startPoint.position, endPoint.position, fractionOfJourney);
            
            // Move the XR Origin so the hand stays at the calculated position
            xrOrigin.position = newHandPosition - handOffsetFromOrigin;
        }
    }
}
