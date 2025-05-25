using UnityEngine;

public class VoiceOverTrigger : MonoBehaviour
{
    public AudioClip voiceOverToPlay;
    public bool useFlashlightTrigger = false;
    public VoiceOverManager voiceOverManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!useFlashlightTrigger && other.CompareTag("Player"))
        {
            TryPlayVoiceOver();
        }
        else if (useFlashlightTrigger && other.CompareTag("FlashlightBeam"))
        {     
            TryPlayVoiceOver();
        }
    }

    private void TryPlayVoiceOver()
    {
        if (voiceOverManager != null)
        {
            voiceOverManager.PlayVoiceOver(voiceOverToPlay);
        }
    }
}
