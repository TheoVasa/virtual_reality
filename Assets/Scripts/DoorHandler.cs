using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DoorHandler : MonoBehaviour
{
    public GameObject door; // Reference to the door GameObject
    public float AnimationTime = 2.0f; // Time in seconds to fully open the door
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioClip doorOpenSound; // Sound to play when the door opens
    public AudioSource audioSource; // Reference to the AudioSource component
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Call this method to open the door
    public void OpenDoor()
    {
        // Start the coroutine to open the door
        StartCoroutine(OpenDoorCoroutine());
        // Play the door open sound
        audioSource.PlayOneShot(doorOpenSound);
    }

 
    public IEnumerator OpenDoorCoroutine()
    {
        float duration = AnimationTime; // Time in seconds to fully open the door
        float elapsedTime = 0f;
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - 90f, transform.eulerAngles.z);

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation; // Ensure final rotation is exact
    }  
}
