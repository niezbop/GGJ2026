using UnityEngine;

public class MaskPlacementManager : MonoBehaviour {
  [SerializeField] private Transform maskCylinderOrigin;
  [SerializeField] private Transform maskLookTarget;

  public void PlaceMask(Transform mask, float angleDeg, float radius, float height) {
    float angleRad = angleDeg * Mathf.Deg2Rad;

    Vector3 offset = new Vector3(
            radius * Mathf.Cos(angleRad),
            height,
            radius * Mathf.Sin(angleRad)
        );

    mask.position = maskCylinderOrigin.position + offset;
    mask.LookAt(maskLookTarget);
  }
}
