using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    private AudioSource sound; //sound to be trigger
    private bool Alreadyplay = false; //check if the sound was already play

    void Start()
    {
        sound = GetComponent<AudioSource>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Alreadyplay == false)
        {
            Alreadyplay = true;
            sound.Play();
        }
        
    }
}
