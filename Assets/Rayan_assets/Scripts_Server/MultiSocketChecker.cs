using System.Collections;
using UnityEngine;


public class MultiSocketChecker : MonoBehaviour
{
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor[] sockets;
    [SerializeField] private MonoBehaviour actionScript; // Should implement ISocketAction

    private ISocketAction action;
    private bool isActive = false; // Current state of the system

    private void Awake()
    {
        action = actionScript as ISocketAction;
        if (action == null)
        {
            Debug.LogError("Assigned script does not implement ISocketAction!");
        }
    }

    private void Start()
    {
        foreach (var socket in sockets)
        {
            socket.selectEntered.AddListener(_ => CheckAllSockets());
            socket.selectExited.AddListener(_ => CheckAllSockets());
        }
    }

    private void CheckAllSockets()
    {
        if (action == null) return;

        bool allPlugged = true;

        foreach (var socket in sockets)
        {
            if (!socket.hasSelection)
            {
                allPlugged = false;
                break;
            }
        }

        // If all are plugged and was not active → activate
        if (allPlugged && !isActive)
        {
            action.ExecuteAction();
            isActive = true;
        }
        // If one is unplugged and was active → deactivate
        else if (!allPlugged && isActive)
        {
            action.UndoAction();
            isActive = false;
        }
    }
}
