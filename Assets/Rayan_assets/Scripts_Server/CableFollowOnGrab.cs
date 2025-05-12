using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using NOT_Lonely;

public class CableFollowOnGrab : MonoBehaviour
{
    public ACC_Trail cableTrail;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable == null)
        {
            Debug.LogWarning("CableFollowOnGrab: No XRGrabInteractable found on this object.");
        }
    }

    void Update()
    {
        if (grabInteractable != null && grabInteractable.isSelected && cableTrail != null)
        {
            cableTrail.UpdateCableTrail();
        }
    }
}

