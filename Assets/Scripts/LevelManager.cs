using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
  [SerializeField] private MaskPlacementManager maskPlacer;
  [SerializeField] private GameObject maskPrefab;
  [SerializeField] private Transform maskParentTransform;

  [Header("Placement configuration")]
  [SerializeField] private float placerRadius;

  [Header("Levels")]
  private int currentLevelIndex;
  [SerializeField] private LevelList levels;
  [SerializeField] private AbstractLevel testLevel;

  public ILevel CurrentLevel => levels[currentLevelIndex];

  private List<MaskFeatures> maskInstances = new();

  private void Start() {
    currentLevelIndex = 0;
    LoadLevel(CurrentLevel);
  }

  public void NextLevel() {
    // TODO: Handle end of game logic
    if(currentLevelIndex >= levels.Length) {}
    currentLevelIndex++;
    LoadLevel(CurrentLevel);
  }

  private void LoadLevel(ILevel level) {
    var maskPositions = level.GetMasks();
    var angleIncrement = 360.0f / maskPositions.Count;
    var currentAngle = angleIncrement / 2.0f;

    foreach(var (maskConfiguration, configuredPosition) in maskPositions)
    {
      var newMaskInstance = Instantiate(maskPrefab, maskParentTransform);
      // TODO: Handle logic for numerous masks
      var defaultPosition = new CylindricalVector3(placerRadius, currentAngle, 0);
      maskPlacer.PlaceMask(newMaskInstance.transform, configuredPosition.GetValueOrDefault(defaultPosition));

      var newMaskFeatures = newMaskInstance.GetComponent<MaskFeatures>();
      newMaskFeatures.SetupShadowMeshes();
      newMaskFeatures.FromConfiguration(maskConfiguration);

      currentAngle += angleIncrement;
    }
  }

  [ContextMenu("Test Level Loading")]
  private void LoadTestLevel() {
    LoadLevel(testLevel);
  }
}
