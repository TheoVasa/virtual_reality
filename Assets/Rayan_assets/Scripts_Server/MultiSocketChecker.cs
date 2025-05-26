using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MultiSocketChecker : MonoBehaviour
{
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor[] sockets;
    [SerializeField] private MonoBehaviour actionScript;

    public UnityEvent onAllSocketsPlugged;
    public UnityEvent onAnySocketUnplugged;

    private ISocketAction action;
    private bool isActive = false;

    private void Awake()
    {
        action = actionScript as ISocketAction;
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

        if (allPlugged && !isActive)
        {
            action.ExecuteAction();
            onAllSocketsPlugged.Invoke();
            isActive = true;
        }
        else if (!allPlugged && isActive)
        {
            action.UndoAction();
            onAnySocketUnplugged.Invoke();
            isActive = false;
        }
    }
}
