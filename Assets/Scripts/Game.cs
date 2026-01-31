using System;
using UnityEngine;

public class Game : MonoBehaviour, IDisposable {
  [SerializeField] private MaskSelector maskSelector;
  [SerializeField] private Timer timer;

  private void Start() {
    maskSelector.OnMaskSelected += OnMaskSelected;
    timer.OnTimerElapsed += Lose;
  }

  private void OnMaskSelected(MaskSelectable mask) {
    
  }

  private void Lose() {

  }

  public void Dispose() {
    maskSelector.OnMaskSelected -= OnMaskSelected;
    timer.OnTimerElapsed -= Lose;
  }
}
