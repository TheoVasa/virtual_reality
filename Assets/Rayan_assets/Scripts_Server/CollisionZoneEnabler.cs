using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CollisionZoneEnabler : MonoBehaviour
{
    public GameObject[] blockingWalls; // liste des murs physiques à activer seulement quand grab

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    void Start()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
        SetWallsActive(false);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        SetWallsActive(true);
    }

    void OnRelease(SelectExitEventArgs args)
    {
        SetWallsActive(false);
    }

    void SetWallsActive(bool active)
    {
        foreach (GameObject wall in blockingWalls)
        {
            Collider col = wall.GetComponent<Collider>();
            if (col != null)
                col.enabled = active;
        }
    }
}
