using UnityEngine;

public class GrabDistanceController : MonoBehaviour
{
    public CanBeGrabbed grabInteractable;

    [Tooltip("If true, only objects tagged 'Player' will activate grab")]
    public bool usePlayerTrigger = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!usePlayerTrigger || (usePlayerTrigger && other.CompareTag("Player")))
        {
            grabInteractable.canBeGrabbed = true;
            Debug.Log("? Player entered ? can grab");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!usePlayerTrigger || (usePlayerTrigger && other.CompareTag("Player")))
        {
            grabInteractable.canBeGrabbed = false;
            Debug.Log("?? Player exited ? cannot grab");
        }
    }
}
