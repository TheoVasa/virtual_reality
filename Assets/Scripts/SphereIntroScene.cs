using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class SphereIntroScene : MonoBehaviour
{
    public float displayDuration = 3f;
    public float textFadeInDuration = 1f;
    public float fadeOutDuration = 1.5f;

    public float minDistToTeleporter = 5f;

    public Renderer sphereRenderer;
    public TextMeshPro titleText;

    private Material sphereMaterial;

    public SceneTeleporter sceneTeleporter;

    void Start()
    {
        if (sphereRenderer != null)
            sphereMaterial = sphereRenderer.material;

        // Begin fade after delay
        Invoke(nameof(StartFadeOut), displayDuration);
        //make the text fade in
        if (titleText != null)
        {
            titleText.alpha = 0f; // Start with text invisible
            StartCoroutine(FadeInText());
        }
    }

    void Update()
    {
        //check the distance to the scene teleporter
        if (sceneTeleporter.isActiveAndEnabled && Vector3.Distance(transform.position, sceneTeleporter.transform.position) < minDistToTeleporter)
        {
            // If close enough, update the sphere transparency
            Vector3 dist = sceneTeleporter.transform.position - transform.position;
            float distance = dist.magnitude;
            if (sphereMaterial != null)
            {
                // Calculate alpha based on distance
                float alpha = Mathf.Clamp01(1f - (distance / minDistToTeleporter));
                // if the distance is less than 0.5, set alpha to 1
                if (distance < 0.5f)
                {
                    alpha = 1f;
                }
                Color color = sphereMaterial.color;
                color.a = alpha;
                sphereMaterial.color = color;
            }
        }
    }

    void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    System.Collections.IEnumerator FadeOut()
    {
        float elapsed = 0f;

        Color startColor = sphereMaterial.color;
        float startAlpha = titleText.alpha;

        while (elapsed < fadeOutDuration)
        {
            float t = elapsed / fadeOutDuration;

            // Fade text
            titleText.alpha = Mathf.Lerp(startAlpha, 0f, t);

            // Fade sphere (assuming shader supports alpha)
            if (sphereMaterial != null)
            {
                Color c = sphereMaterial.color;
                c.a = Mathf.Lerp(startColor.a, 0f, t);
                sphereMaterial.color = c;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        titleText.alpha = 0f;
        if (sphereMaterial != null)
        {
            Color finalColor = sphereMaterial.color;
            finalColor.a = 0f;
            sphereMaterial.color = finalColor;
        }

    }
    System.Collections.IEnumerator FadeInText()
    {
        float elapsed = 0f;
        float startAlpha = titleText.alpha;

        while (elapsed < textFadeInDuration)
        {
            float t = elapsed / textFadeInDuration;
            titleText.alpha = Mathf.Lerp(startAlpha, 1f, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        titleText.alpha = 1f; // Ensure it ends at fully visible
    }
}