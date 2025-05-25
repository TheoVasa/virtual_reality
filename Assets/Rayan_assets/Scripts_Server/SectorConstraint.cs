using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SectorConstraintRigid : MonoBehaviour
{
    public Transform center;
    public float maxRadius = 2f;
    public float sectorAngle = 90f;
    public Vector3 sectorDirection = Vector3.forward;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 rel = rb.position - center.position;

        // Distance
        float distance = rel.magnitude;
        if (distance > maxRadius)
        {
            rel = rel.normalized * maxRadius;
        }

        // Angle
        float angle = Vector3.Angle(sectorDirection.normalized, rel.normalized);
        if (angle > sectorAngle / 2f)
        {
            // Reste dans le plan horizontal
            Vector3 projected = Vector3.ProjectOnPlane(rel, Vector3.up);
            float sign = Mathf.Sign(Vector3.SignedAngle(sectorDirection, projected, Vector3.up));
            float clampedAngle = sectorAngle / 2f;
            Vector3 clampedDir = Quaternion.AngleAxis(sign * clampedAngle, Vector3.up) * sectorDirection.normalized;
            rel = clampedDir * Mathf.Min(distance, maxRadius);
        }

        // Correction de la position (soft)
        Vector3 correctedPos = center.position + rel;
        rb.MovePosition(correctedPos);
    }
}
