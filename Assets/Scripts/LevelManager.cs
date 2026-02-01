using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
  [SerializeField] private MaskPlacementManager maskPlacer;
  [SerializeField] private GameObject maskPrefab;
  [SerializeField] private Transform maskParentTransform;

  [Header("Audio configuration")]
  [SerializeField] private AudioSource maskAudioSource;

  [Header("Levels")]
  private int currentLevelIndex;
  [SerializeField] private LevelList levels;
  [SerializeField] private AbstractLevel testLevel;

  public ILevel CurrentLevel => levels[currentLevelIndex];
  public int CurrentLevelIndex => currentLevelIndex;
  public int LevelsCount => levels.Length;

  public LevelList Levels => levels;

  private List<MaskFeatures> maskInstances = new();

  private void Start() {
    Clear();
    currentLevelIndex = 0;
    LoadLevel(CurrentLevel);
  }

  public bool IsLastLevel() {
    return currentLevelIndex >= levels.Length - 1;
  }

  public void NextLevel() {
    // TODO: Handle end of game logic
    if (currentLevelIndex >= levels.Length) {

    }

    Clear();
    currentLevelIndex++;
    LoadLevel(CurrentLevel);
  }

  private void LoadLevel(ILevel level) {
    var maskPositions = level.GetMasks();

    foreach (var (maskConfiguration, configuredPosition) in maskPositions) {
      var newMaskInstance = Instantiate(maskPrefab, maskParentTransform);
      newMaskInstance.GetComponent<MaskSelectable>().SetAudioSource(maskAudioSource);

      maskPlacer.PlaceMask(newMaskInstance.transform, configuredPosition);

      var newMaskFeatures = newMaskInstance.GetComponent<MaskFeatures>();
      newMaskFeatures.SetupShadowMeshes();
      newMaskFeatures.FromConfiguration(maskConfiguration);
    }
  }

  private void Clear() {
    foreach (var maskInstance in maskInstances) {
      Destroy(maskInstance.gameObject);
    }

    foreach (Transform child in maskParentTransform) {
      Destroy(child.gameObject);
    }
  }

  [ContextMenu("Test Level Loading")]
  private void LoadTestLevel() {
    LoadLevel(testLevel);
  }
}
