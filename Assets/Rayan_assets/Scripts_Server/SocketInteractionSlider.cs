using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor))]
public class SocketInteractionSlider : MonoBehaviour
{
    [Header("Slide Settings")]
    public Transform objectToSlide;
    public float slideDistance = 1f;
    public float slideDuration = 1f;
    public float speedMultiplier = 1f;

    [Header("Plug Reference")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable plugInteractable;

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketInteractor;
    private Vector3 initialPosition;
    private Vector3 downPosition;
    private Coroutine slideCoroutine;

    private void Awake()
    {
        socketInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
        initialPosition = objectToSlide.position;
        downPosition = initialPosition + Vector3.down * slideDistance;
    }

    private void OnEnable()
    {
        socketInteractor.selectEntered.AddListener(OnSocketEnter);

        if (plugInteractable != null)
        {
            plugInteractable.selectEntered.AddListener(OnPlugGrabbed);
        }
    }

    private void OnDisable()
    {
        socketInteractor.selectEntered.RemoveListener(OnSocketEnter);

        if (plugInteractable != null)
        {
            plugInteractable.selectEntered.RemoveListener(OnPlugGrabbed);
        }
    }

    private void OnSocketEnter(SelectEnterEventArgs args)
    {
        StartSlide(downPosition);
    }

    private void OnPlugGrabbed(SelectEnterEventArgs args)
    {
        if (args.interactorObject != socketInteractor)
        {
            StartSlide(initialPosition);
        }
    }

    private void StartSlide(Vector3 target)
    {
        if (slideCoroutine != null)
        {
            StopCoroutine(slideCoroutine);
        }

        slideCoroutine = StartCoroutine(SlideTo(target));
    }

    private IEnumerator SlideTo(Vector3 target)
    {
        Vector3 start = objectToSlide.position;
        float elapsed = 0f;
        float duration = slideDuration / Mathf.Max(speedMultiplier, 0.0001f);

        while (elapsed < duration)
        {
            objectToSlide.position = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        objectToSlide.position = target;
        slideCoroutine = null;
    }
}
