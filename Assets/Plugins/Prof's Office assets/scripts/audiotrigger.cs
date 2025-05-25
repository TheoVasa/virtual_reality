using UnityEngine;

public class AudioTrigger : MonoBehaviour // Renamed class
{
    public AudioSource audioSource; // AudioSource to play
    public GameObject player;       // Player GameObject to detect
    private bool hasPlayed = false; // Flag to ensure audio plays only once

    // OnTriggerEnter is called when the Collider other enters the trigger
    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        // and if the audio hasn't played yet
        if (other.gameObject == player && !hasPlayed)
        {
            if (audioSource != null)
            {
                audioSource.Play();
                hasPlayed = true; // Set the flag to true so it doesn't play again
            }
            else
            {
                Debug.LogError("AudioSource not assigned in AudioTrigger script on " + gameObject.name);
            }
        }
    }

    // Optional: Make the cube invisible at runtime
    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
