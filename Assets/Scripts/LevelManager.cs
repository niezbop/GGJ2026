using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
  [SerializeField] private MaskPlacementManager maskPlacer;
  [SerializeField] private GameObject maskPrefab;
  [SerializeField] private Transform maskParentTransform;

  // Test stuff
  [SerializeField] private float testPlacerAngle = 0f;
  [SerializeField] private float testPlacerRadius = 2f;
  [SerializeField] private float testPlacerHeight = 1f;
  [SerializeField] private float testPlacerMaskCount = 1f;

  private List<MaskFeatures> maskInstances = new();

  private void Start() {
  }

  public void SpawnMaskAtPosition(float angleDeg, float radius, float height) {
    var newMaskInstance = Instantiate(maskPrefab, maskParentTransform);
    maskPlacer.PlaceMask(newMaskInstance.transform, angleDeg, radius, height);

    var newMaskFeatures = newMaskInstance.GetComponent<MaskFeatures>();
    newMaskFeatures.SetRandomFeatures();
    maskInstances.Add(newMaskFeatures);
  }

  [ContextMenu("Test Place Mask")]
  private void TestSpawnMask() {
    SpawnMaskAtPosition(testPlacerAngle, testPlacerRadius, testPlacerHeight);
  }

  [ContextMenu("Test Place Multiple Masks")]
  private void TestSpawnMultipleMasks() {
    for (int i = 0; i < testPlacerMaskCount; i++) {
      float angle = i * 360f / testPlacerMaskCount;
      SpawnMaskAtPosition(angle, testPlacerRadius, testPlacerHeight);
    }
  }
}
/*  */
