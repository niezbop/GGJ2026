using System;
using UnityEngine;

public class Game : MonoBehaviour, IDisposable {
  [SerializeField] private MaskSelector maskSelector;
  [SerializeField] private Timer timer;
  [SerializeField] private LevelManager levelManager;

  private void Start() {
    maskSelector.OnMaskSelected += OnMaskSelected;
    timer.OnTimerElapsed += Lose;
  }

  private void OnMaskSelected(MaskSelectable mask) {
    var maskObject = mask.gameObject;
    var maskFeatures = maskObject.GetComponent<MaskFeatures>();

    if (levelManager.CurrentLevel.IsIntruder(maskFeatures.MaskConfiguration)) {
      WinLevel();
    } else {
      Lose();
    }
  }

  private void WinLevel() {
    timer.RestartCountdown();
    levelManager.NextLevel();
  }

  private void Lose() {

  }

  public void Dispose() {
    maskSelector.OnMaskSelected -= OnMaskSelected;
    timer.OnTimerElapsed -= Lose;
  }
}
