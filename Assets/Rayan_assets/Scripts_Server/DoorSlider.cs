using UnityEngine;

public class DoorSlider : MonoBehaviour, ISocketAction
{
    [Header("Slide Settings")]
    public Vector3 slideOffset = new Vector3(0, 0, 1f); // Local space offset
    public float slideSpeed = 2f; // Units per second

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isOpen = false;
    private bool isMoving = false;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + transform.TransformDirection(slideOffset); // Local to world
    }

    void Update()
    {
        if (isMoving)
        {
            Vector3 desiredPosition = isOpen ? targetPosition : initialPosition;

            // Constant speed in either direction
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, slideSpeed * Time.deltaTime);

            // Snap into place when close enough
            if (Vector3.Distance(transform.position, desiredPosition) < 0.001f)
            {
                transform.position = desiredPosition;
                isMoving = false;
            }
        }
    }

    public void ToggleSlide()
    {
        isOpen = !isOpen;
        isMoving = true;
    }

    public void ExecuteAction()
    {
        if (!isOpen)
        {
            isOpen = true;
            isMoving = true;
        }
    }

    public void UndoAction()
    {
        if (isOpen)
        {
            isOpen = false;
            isMoving = true;
        }
    }
}
