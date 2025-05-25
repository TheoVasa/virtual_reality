using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketInteractionExecutor : MonoBehaviour
{
    [Tooltip("Reference to the XR Socket Interactor")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketInteractor;

    [Tooltip("Action to run when an object is inserted into the socket")]
    public UnityEvent onObjectInserted;

    [Tooltip("Action to run when an object is removed from the socket")]
    public UnityEvent onObjectRemoved;

    private void Awake()
    {
        if (socketInteractor == null)
        {
            socketInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
        }
    }

    private void OnEnable()
    {
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.AddListener(OnSelectEntered);
            socketInteractor.selectExited.AddListener(OnSelectExited);
        }
    }

    private void OnDisable()
    {
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnSelectEntered);
            socketInteractor.selectExited.RemoveListener(OnSelectExited);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.IsChildOf(socketInteractor.transform))
        {
            Debug.Log("⚠️ Ignoring self-selection");
            return;
        }

        Debug.Log("✅ Plug inserted: " + args.interactableObject.transform.name);
        onObjectInserted?.Invoke();
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.IsChildOf(socketInteractor.transform))
        {
            Debug.Log("⚠️ Ignoring self-removal");
            return;
        }

        Debug.Log("❌ Plug removed: " + args.interactableObject.transform.name);
        onObjectRemoved?.Invoke();
    }


}
