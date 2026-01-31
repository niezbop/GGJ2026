using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
  [SerializeField] private float timeLimitSeconds = 30f;

  [SerializeField] private Image uiFillBar;

  private float startTime = -1.0f;
  public bool Started => startTime >= 0;

  public float RemainingTimeNormalized() {
    if (Started) {
      var elapsed = Time.time - startTime;
      return Mathf.Clamp01(elapsed / timeLimitSeconds);
    } else {
      return 0f;
    }
  }

  public delegate void TimerEvent();
  public event TimerEvent OnTimerElapsed = delegate { };

  void Start() {
    StartCountdown();
  }

  private void Update() {
    if (!Started) return;

    UpdateUI();

    if (Time.time - startTime > timeLimitSeconds) {
      OnTimerElapsed?.Invoke();
      StopCountdown();
    }
  }

  public void StartCountdown() {
    startTime = Time.time;
  }

  public void StopCountdown() {
    startTime = -1.0f;
  }

  public void RestartCountdown() {
    StartCountdown();
  }

  private void UpdateUI() {
    if (uiFillBar != null) {
      uiFillBar.fillAmount = 1f - RemainingTimeNormalized();
    }
  }
}
