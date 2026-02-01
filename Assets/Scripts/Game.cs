using System;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour, IDisposable {
  [SerializeField] private MaskSelector maskSelector;
  [SerializeField] private Timer timer;
  [SerializeField] private LevelManager levelManager;
  [SerializeField] private GameMenuManager gameMenuManager;
  [SerializeField] private GameEffects gameEffects;


  [SerializeField] private bool debugWinOnAnyMask = false;

  private bool DebugWinOnAnyMask {
    get {
#if UNITY_EDITOR
      return debugWinOnAnyMask;
#else
      return false;
#endif
    }
  }

  private void Start() {
    maskSelector.OnMaskSelected += OnMaskSelected;
    timer.OnTimerElapsed += Lose;
  }

  private void OnMaskSelected(MaskSelectable mask) {
    var maskObject = mask.gameObject;
    var maskFeatures = maskObject.GetComponent<MaskFeatures>();

    if (levelManager.CurrentLevel.IsIntruder(maskFeatures.MaskConfiguration) || debugWinOnAnyMask) {
      if (levelManager.IsLastLevel()) {
        WinWholeGame();
        return;
      }

      WinLevel(mask);
    } else {
      Lose();
    }
  }

  private void WinWholeGame() {
    gameMenuManager.ShowWinMenu();
  }

  private void WinLevel(MaskSelectable selectedMask) {
    if (selectedMask == null) return;

    // Play transition effect, load next level during blackout
    gameEffects.PlayLevelWinTransition(selectedMask, () => {
      timer.RestartCountdown();
      levelManager.NextLevel();
    });
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
    WinLevel(null);
  }
}
