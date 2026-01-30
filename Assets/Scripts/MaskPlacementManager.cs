using UnityEngine;

public class MaskPlacementManager : MonoBehaviour {
  public void SetCylindricalPosition(float angleDeg, float radius, float height) {
    float angleRad = angleDeg * Mathf.Deg2Rad;

    Vector3 localPosition = new Vector3(
      radius * Mathf.Cos(angleRad),
      height,
      radius * Mathf.Sin(angleRad)
    );
    transform.position = transform.position + localPosition;
    transform.LookAt(transform);
  }
}
