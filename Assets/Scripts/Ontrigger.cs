using UnityEngine;
using UnityEngine.Events;

public class Ontrigger : MonoBehaviour
{
    public UnityEvent[] events;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && events != null)
        {
            foreach (UnityEvent unityEvent in events)
            {
                unityEvent?.Invoke();
            }
        }
    }
}
