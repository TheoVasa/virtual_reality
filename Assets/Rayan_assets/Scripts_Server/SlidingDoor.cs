using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SlidingDoor : MonoBehaviour
{
    public Transform handle;                  // Assign: usually the drawer itself
    public Vector3 localSlideAxis = Vector3.forward;  // Local axis (e.g., Z forward)
    public float maxSlideDistance = 0.5f;     // Maximum allowed slide distance
    public float slideSpeed = 10f;            // Smooth follow speed

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Vector3 initialLocalPosition;
    private bool isGrabbed = false;
    private Transform interactorTransform;

    void Start()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        initialLocalPosition = transform.localPosition;

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        interactorTransform = args.interactorObject.transform;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        interactorTransform = null;
    }

    void Update()
    {
        if (isGrabbed && interactorTransform != null)
        {
            // Get controller position in local space of the handle
            Vector3 localInteractorPos = transform.parent.InverseTransformPoint(interactorTransform.position);
            Vector3 localInitialPos = transform.parent.InverseTransformPoint(transform.position);

            // Calculate movement only along the desired axis
            Vector3 delta = localInteractorPos - localInitialPos;
            float movementAlongAxis = Vector3.Dot(delta, localSlideAxis.normalized);

            // Clamp to limits
            movementAlongAxis = Mathf.Clamp(movementAlongAxis, 0, maxSlideDistance);

            // Calculate new local position
            Vector3 newLocalPos = initialLocalPosition + localSlideAxis.normalized * movementAlongAxis;

            // Apply smoothed position
            transform.localPosition = Vector3.Lerp(transform.localPosition, newLocalPos, Time.deltaTime * slideSpeed);
        }
        else
        {
            // Return to initial position when not grabbed
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialLocalPosition, Time.deltaTime * slideSpeed);
        }
    }
}
