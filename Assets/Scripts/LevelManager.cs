using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
  [SerializeField] private MaskPlacementManager maskPlacer;
  [SerializeField] private GameObject maskPrefab;

  [SerializeField] private Transform testMaskObject;
  [SerializeField] private Transform maskParentTransform;

  [SerializeField] private float testPlacerAngle = 0f;
  [SerializeField] private float testPlacerRadius = 2f;
  [SerializeField] private float testPlacerHeight = 1f;

  private List<GameObject> maskInstances = new();

  private void Start() {
    if (testMaskObject != null) {
      maskPlacer.PlaceMask(testMaskObject, testPlacerAngle, testPlacerRadius, testPlacerHeight);
    }
  }

  public void SpawnMaskAtPosition(float angleDeg, float radius, float height) {
    var newMaskInstance = Instantiate(maskPrefab, maskParentTransform);
    maskPlacer.PlaceMask(newMaskInstance.transform, angleDeg, radius, height);

    maskInstances.Add(newMaskInstance);
  }

  [ContextMenu("Test Place Mask")]
  private void TestSpawnMask() {
    SpawnMaskAtPosition(testPlacerAngle, testPlacerRadius, testPlacerHeight);
  }

  [ContextMenu("Delete All Masks")]
  private void DeleteAllMasks() {
    foreach (var mask in maskInstances) {
      Destroy(mask);
    }
    maskInstances.Clear();
  }
}
