using UnityEngine;

public class SnapTo : MonoBehaviour
{
    public Transform target; // drag the attach point here

    [ContextMenu("Snap to Target")]
    void Snap()
    {
        if (target == null) return;

        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
