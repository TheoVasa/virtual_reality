using UnityEngine;

public class DoorSlider : MonoBehaviour, ISocketAction
{
    [Header("Slide Settings")]
    public Vector3 slideOffset = new Vector3(0, 0, 1f);
    public float slideSpeed = 2f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isOpen = false;
    private bool isMoving = false;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + slideOffset;
    }

    void Update()
    {
        if (isMoving)
        {
            Vector3 desiredPosition = isOpen ? targetPosition : initialPosition;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * slideSpeed);

            if (Vector3.Distance(transform.position, desiredPosition) < 0.01f)
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
