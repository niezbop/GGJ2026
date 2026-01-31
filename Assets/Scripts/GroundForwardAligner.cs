using UnityEngine;
using UnityEngine.Serialization;

public class GroundForwardAligner : MonoBehaviour {
  [SerializeField] private Transform trackedTransform;
  [FormerlySerializedAs("speed")] [SerializeField] private float maxSpeed;
  [SerializeField] private float deadZoneAngle;

  private void Update() {
    var selfGroundDirection = new Vector2(transform.forward.x, transform.forward.z).normalized;
    var trackedGroundDirection = new Vector2(trackedTransform.forward.x, trackedTransform.forward.z).normalized;

    var angle = Vector2.SignedAngle(selfGroundDirection, trackedGroundDirection);

    if (Mathf.Abs(angle) <= deadZoneAngle) {
      return;
    }

    transform.RotateAround(transform.position, Vector3.down, Mathf.InverseLerp(0, 90, Mathf.Abs(angle)) * maxSpeed * Time.deltaTime * Mathf.Sign(angle));
  }
}
