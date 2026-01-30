using UnityEngine;

public class MaskSelector : MonoBehaviour {
  [SerializeField] private float maxDistance = 20f;
  [SerializeField] private LayerMask maskLayer;

  private MaskSelectable currentSelection;

  private void Update() {
    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    MaskSelectable newSelection = null;

    if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, maskLayer)) {
      newSelection = hit.collider.GetComponent<MaskSelectable>();
    }

    if (newSelection != currentSelection) {
      currentSelection?.SetSelected(false);
      newSelection?.SetSelected(true);
      currentSelection = newSelection;
    }
  }

  private void OnDrawGizmos() {
    Gizmos.color = Color.green;
    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    Gizmos.DrawRay(ray.origin, ray.direction * maxDistance);
  }
}
