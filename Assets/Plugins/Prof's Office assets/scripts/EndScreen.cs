using UnityEngine;
using UnityEngine.Events; // Required for UnityEvent
using System.Collections;
using TMPro; // Using TextMeshPro for UI elements. If you use standard UI Text, change this to 'using UnityEngine.UI;' and 'Text[]' below.

public class EndScreen : MonoBehaviour
{
    [Header("Object References")]
    [Tooltip("The large sphere object that will fade in.")]
    public GameObject sphereObject;
    [Tooltip("The text elements to display sequentially. Assign 5 texts here.")]
    public TextMeshProUGUI[] texts;
    [Tooltip("Sound to play when each text appears.")]
    public AudioClip textAppearSound;
    
    [Header("Timings")]
    [Tooltip("Duration for the sphere to fade in (in seconds).")]
    public float sphereFadeInDuration = 2.0f;
    [Tooltip("Delay between each text appearing (in seconds).")]
    public float delayBetweenTexts = 1.0f;
    [Tooltip("Delay after all texts are shown, before triggering the final event (in seconds).")]
    public float delayBeforeNextScript = 5.0f;

    [Header("Events")]
    [Tooltip("Event to trigger after the end screen sequence is complete.")]
    public UnityEvent onEndScreenFinished;

    private AudioSource audioSource;
    private Coroutine activeEndScreenCoroutine;
    private Material sphereMaterialInstance; // To store and modify the sphere's instanced material

    void Awake()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Initial setup for the sphere: make it transparent and inactive
        if (sphereObject != null)
        {
            Renderer sphereRenderer = sphereObject.GetComponent<Renderer>();
            if (sphereRenderer != null)
            {
                // Instance the material to avoid changing the original asset or other objects using the same material
                sphereMaterialInstance = sphereRenderer.material; 
                Color c = sphereMaterialInstance.color;
                // Set initial alpha to 0. The material's shader must support transparency (e.g., Standard shader set to Fade or Transparent).
                sphereMaterialInstance.color = new Color(c.r, c.g, c.b, 0f); 
            }
            else
            {
                Debug.LogWarning("EndScreen: Sphere object does not have a Renderer component. Fade-in will not work correctly.", sphereObject);
            }
            sphereObject.SetActive(false); // Start with the sphere object itself inactive
        }
        else
        {
            Debug.LogWarning("EndScreen: Sphere object is not assigned.", this);
        }

        // Initial setup for texts: make them inactive
        foreach (TextMeshProUGUI textElement in texts)
        {
            if (textElement != null)
            {
                textElement.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Public method to start the end screen sequence. Call this from your other scripts.
    /// </summary>
    public void TriggerEndScreen()
    {
        if (activeEndScreenCoroutine != null)
        {
            StopCoroutine(activeEndScreenCoroutine); // Stop any existing sequence
        }
        activeEndScreenCoroutine = StartCoroutine(EndScreenSequence());
    }

    private IEnumerator EndScreenSequence()
    {
        // 1. Fade in Sphere
        if (sphereObject != null)
        {
            sphereObject.SetActive(true); // Activate the sphere object so it can be rendered

            if (sphereMaterialInstance != null)
            {
                float elapsedTime = 0f;
                // The material's current color has alpha 0 from Awake setup
                Color baseColor = new Color(sphereMaterialInstance.color.r, sphereMaterialInstance.color.g, sphereMaterialInstance.color.b, 0f);

                if (sphereFadeInDuration > 0.001f) // Check for valid duration
                {
                    while (elapsedTime < sphereFadeInDuration)
                    {
                        elapsedTime += Time.deltaTime;
                        float alpha = Mathf.Clamp01(elapsedTime / sphereFadeInDuration);
                        sphereMaterialInstance.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                        yield return null;
                    }
                }
                sphereMaterialInstance.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1f); // Ensure fully opaque
            }
        }

        // 2. Display Texts one by one
        if (texts != null)
        {
            foreach (TextMeshProUGUI textElement in texts)
            {
                if (textElement != null)
                {
                    if (delayBetweenTexts > 0)
                        yield return new WaitForSeconds(delayBetweenTexts);
                    
                    textElement.gameObject.SetActive(true);
                    
                    if (textAppearSound != null && audioSource != null)
                    {
                        audioSource.PlayOneShot(textAppearSound);
                    }
                }
            }
        }

        // 3. Wait for delay before calling the next script/event
        if (delayBeforeNextScript > 0)
            yield return new WaitForSeconds(delayBeforeNextScript);

        // 4. Call the UnityEvent for the next script
        if (onEndScreenFinished != null)
        {
            onEndScreenFinished.Invoke();
        }
        
        activeEndScreenCoroutine = null; // Sequence finished
    }
}
