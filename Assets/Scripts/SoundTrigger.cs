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
        print("Collision");
        print(Alreadyplay);
        print(other.CompareTag("Player"));
        if (other.CompareTag("Player") && Alreadyplay == false)
        {
            print("Cool");
            Alreadyplay = true;
            sound.Play();
        }
        
    }
}
