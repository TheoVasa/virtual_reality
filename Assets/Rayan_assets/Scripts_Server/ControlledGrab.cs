using UnityEngine;


public class ControlledGrab : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    private void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grab.enabled = false; // start disabled
    }

    public void SetGrabbable(bool canGrab)
    {
        grab.enabled = canGrab;
    }
}