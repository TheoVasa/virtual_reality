using UnityEngine;
using System;
using System.Collections;
using TMPro;
using UnityEngine.Events;

public class DoorOpenerZone : MonoBehaviour
{
    public UnityEvent onZoneEntered;
    private bool isDooropened = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void AccessGranted()
    {
        if (isDooropened) return;
        onZoneEntered?.Invoke();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
        AccessGranted();
        isDooropened = true;
        }
    }
}
