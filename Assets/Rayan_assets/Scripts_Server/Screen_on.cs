using System.Collections;
using UnityEngine;

public class Screen_on : MonoBehaviour
{
    public Material bootingMaterial;    
    public Material screenOnMaterial;   
    public Material screenOffMaterial; 
    public float delay = 2.5f;          

    private Renderer screenRenderer;
    private Coroutine bootSequence;

    void Start()
    {
        screenRenderer = GetComponent<Renderer>();
        screenRenderer.material = screenOffMaterial;
    }

    public void TurnOnScreen()
    {
        if (bootSequence != null)
        {
            StopCoroutine(bootSequence);
        }
        bootSequence = StartCoroutine(BootSequence());
    }

    IEnumerator BootSequence()
    {
        // Step 1: Blue glow to simulate power up
        screenRenderer.material = bootingMaterial;

        // Step 2: Wait for delay
        yield return new WaitForSeconds(delay);

        // Step 3: Show final image
        screenRenderer.material = screenOnMaterial;
    }

    // Optional method to turn off again
    public void TurnOffScreen()
    {
        if (bootSequence != null)
        {
            StopCoroutine(bootSequence);
        }
        screenRenderer.material = screenOffMaterial;
    }
}
