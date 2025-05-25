using System;
using UnityEngine;

public class VoiceOverManager : MonoBehaviour
{
    // create a list of audio sources
    public AudioClip[] voiceOverClips; // array of audio clips
    private Boolean[] hasPlayed; // array to track if the clips have been played
    private AudioSource mainAudioSource; // main audio source to play the clips
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get the audio source component attached to this game object
        mainAudioSource = GetComponent<AudioSource>();
        // check if the audio source is not null
        if (mainAudioSource == null)
        {
            Debug.LogError("No AudioSource component found on this GameObject.");
            return;
        }
        // check if the voiceOverClips array is not empty
        if (voiceOverClips.Length == 0)
        {
            Debug.LogError("No voice over clips assigned.");
            return;
        }
        //fill the hasPlayed array with false values
        hasPlayed = new Boolean[voiceOverClips.Length];
        for (int i = 0; i < hasPlayed.Length; i++)
        {
            hasPlayed[i] = false;
        }
    }

    // method to play a desired clip from the array
    public void PlayVoiceOver(AudioClip clip)
    {
        // check if the array contains the clip
        int clipIndex = Array.IndexOf(voiceOverClips, clip);
        if (clipIndex == -1)
        {
            Debug.LogError("Clip not found in the array.");
            return;
        }
        // play the audio clip at the specified index if it has not been played before
        if (hasPlayed[clipIndex])
        {
            Debug.Log("This clip has already been played.");
            return;
        }
        hasPlayed[clipIndex] = true; // mark the clip as played
        mainAudioSource.clip = voiceOverClips[clipIndex];
        mainAudioSource.Play();
    }
}
