using UnityEngine;


public class GrabOnlyInsideZone : MonoBehaviour
{
    public ControlledGrab controlledObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // or "MainCamera", depending on your setup
        {
            controlledObject.SetGrabbable(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controlledObject.SetGrabbable(false);
        }
    }
}