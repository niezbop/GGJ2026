using TMPro;
using UnityEngine;

public class LevelCounter : MonoBehaviour {
  [SerializeField] private LevelManager levelManager;
  [SerializeField] private TMP_Text counterText;

  public void Update() {
    counterText.text = $"{levelManager.CurrentLevelIndex + 1}/{levelManager.LevelsCount}";
  }
}
