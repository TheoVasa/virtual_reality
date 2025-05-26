using System.Collections;
using UnityEngine;

public class ScreenMaterialSequence : MonoBehaviour, ISocketAction
{
    [SerializeField] private Renderer screenRenderer;
    [SerializeField] private Material loadingMaterial;
    [SerializeField] private Material finalMaterial;
    [SerializeField] private Material offMaterial;
    [SerializeField] private float waitTime = 3f;

    private Coroutine sequenceCoroutine;

    public void StartMaterialSequence()
    {
        if (sequenceCoroutine != null)
            StopCoroutine(sequenceCoroutine);

        sequenceCoroutine = StartCoroutine(MaterialSequenceCoroutine());
    }

    private IEnumerator MaterialSequenceCoroutine()
    {
        screenRenderer.material = loadingMaterial;
        yield return new WaitForSeconds(waitTime);
        screenRenderer.material = finalMaterial;
    }

    public void ExecuteAction()
    {
        StartMaterialSequence();
    }

    public void UndoAction()
    {
        if (sequenceCoroutine != null)
        {
            StopCoroutine(sequenceCoroutine);
            sequenceCoroutine = null;
        }

        screenRenderer.material = offMaterial;
    }
}
