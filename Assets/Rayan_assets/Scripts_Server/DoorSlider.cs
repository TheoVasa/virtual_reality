using UnityEngine;

public class DoorSlider : MonoBehaviour, ISocketAction
{
    public Vector3 slideOffset = new Vector3(0, 0, 1f);
    public float slideSpeed = 2f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isOpen = false;
    private bool isMoving = false;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + transform.TransformDirection(slideOffset);
    }

    void Update()
    {
        if (isMoving)
        {
            Vector3 desiredPosition = isOpen ? targetPosition : initialPosition;
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, slideSpeed * Time.deltaTime);

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
