using UnityEngine;


public class CanBeGrabbed : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    public bool canBeGrabbed = false;

    public override bool IsSelectableBy(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor)
    {
        return base.IsSelectableBy(interactor) && canBeGrabbed;
    }
}
