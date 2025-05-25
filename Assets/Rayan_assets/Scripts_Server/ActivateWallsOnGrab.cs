using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class ActivateWallsOnGrab : MonoBehaviour
{
    public GameObject[] wallsToControl;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool canTrigger = false;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void Start()
    {
        StartCoroutine(EnableGrabAfterDelay(1f));
    }

    private IEnumerator EnableGrabAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canTrigger = true;
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (!canTrigger) return;

        // Better way: Check if the interactor GameObject has a socket component
        var interactorGO = args.interactorObject.transform?.gameObject;
        if (interactorGO != null && interactorGO.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>() != null)
        {
            Debug.Log("Grab ignored: triggered by a socket.");
            return;
        }

        Debug.Log("Grabbed by user – activating walls.");
        foreach (GameObject wall in wallsToControl)
        {
            wall.SetActive(true);
        }
    }


    private void OnRelease(SelectExitEventArgs args)
    {
        if (!canTrigger)
            return;

        if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor)
        {
            Debug.Log("Release ignored: from socket.");
            return;
        }

        Debug.Log("Released by user ? deactivating walls.");
        foreach (GameObject wall in wallsToControl)
        {
            wall.SetActive(false);
        }
    }
}
