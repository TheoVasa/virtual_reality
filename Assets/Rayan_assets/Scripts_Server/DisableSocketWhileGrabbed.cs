using UnityEngine;


public class DisableSocketWhileGrabbed : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketInteractor;

    void Awake()
    {
        if (grabInteractable == null)
            grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(_ => socketInteractor.socketActive = false);
        grabInteractable.selectExited.AddListener(_ => socketInteractor.socketActive = true);
    }
}