using UnityEngine;

public class MaskPlacementManager : MonoBehaviour {
  [SerializeField] private Transform maskCylinderOrigin;
  [SerializeField] private Transform maskLookTarget;

  public void PlaceMask(Transform mask, CylindricalVector3 position) {
    mask.position = maskCylinderOrigin.position + position.ToVector3();
    mask.LookAt(maskLookTarget);
  }
}
