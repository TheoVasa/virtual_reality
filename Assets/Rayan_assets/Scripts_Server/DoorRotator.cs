using UnityEngine;

public class DoorRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Vector3 rotationAxis = Vector3.up; // Y-axis by default
    public float rotationAngle = 90f;         // Degrees to rotate
    public float rotationSpeed = 2f;          // Speed of rotation

    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private bool isRotating = false;
    private bool isOpen = false;

    void Start()
    {
        initialRotation = transform.rotation;
        targetRotation = Quaternion.Euler(rotationAxis * rotationAngle) * initialRotation;
    }

    void Update()
    {
        if (isRotating)
        {
            Quaternion desiredRotation = isOpen ? targetRotation : initialRotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);

            if (Quaternion.Angle(transform.rotation, desiredRotation) < 0.5f)
            {
                transform.rotation = desiredRotation;
                isRotating = false;
            }
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
        isRotating = true;
    }

    public void CloseDoor()
    {
        isOpen = false;
        isRotating = true;
    }
}
