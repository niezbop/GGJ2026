using UnityEngine;

public class Timer : MonoBehaviour
{
  [SerializeField] private int seconds;

  private float startTime = -1.0f;
  public bool Started { get { return startTime > 0; } }

  public delegate void TimerEvent();
  public event TimerEvent OnTimerElapsed = delegate { };

  public void StartCountdown() {
    startTime = Time.time;
  }

  public void StopCountdown() {
    startTime = -1.0f;
  }

  public void RestartCountdown() {
    StartCountdown();
  }

  private void Update() {
    if (Time.time - startTime > seconds) {
      OnTimerElapsed?.Invoke();
    }
  }
}
