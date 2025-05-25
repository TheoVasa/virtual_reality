using UnityEngine;

public class OpenDoor : MonoBehaviour
{
  
    public float openAngle = 90f;
    public float xOffset = 1.0f;
    public Vector3 rotationAxis = Vector3.up;

    void Start()
    {

        Vector3 pivot = transform.position + transform.right * xOffset;
        transform.RotateAround(pivot, rotationAxis, openAngle);
    }
}
