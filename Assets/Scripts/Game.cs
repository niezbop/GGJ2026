using System;
using UnityEngine;

public class Game : MonoBehaviour, IDisposable {
  [SerializeField] private MaskSelector maskSelector;
  [SerializeField] private Timer timer;
  [SerializeField] private LevelManager levelManager;
  [SerializeField] private GameMenuManager gameMenuManager;

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

  private void WinWholeGame() {
    gameMenuManager.ShowWinMenu();
  }

  private void WinLevel() {
    if (levelManager.IsLastLevel()) {
      WinWholeGame();
      return;
    }

    timer.RestartCountdown();
    levelManager.NextLevel();
  }

  private void Lose() {
    timer.StopCountdown();
    gameMenuManager.ShowGameOverMenu();
  }

  public void Dispose() {
    maskSelector.OnMaskSelected -= OnMaskSelected;
    timer.OnTimerElapsed -= Lose;
  }

  [ContextMenu("Lose game")]
  private void LoseGame() {
    Lose();
  }

  [ContextMenu("Win whole game")]
  private void WinGame() {
    WinWholeGame();
  }

  [ContextMenu("Win level")]
  private void WinLevelDebug() {
    WinLevel();
  }
}
