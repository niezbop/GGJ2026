using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
  [SerializeField] private string mainSceneName = "MainScene";
  [SerializeField] private Button startButton;
  [SerializeField] private Button quitButton;

  [Header("SFX")]
  [SerializeField] private AudioSource sfxSource;
  [SerializeField] private AudioClip menuSfx;

  private IDisposable disposableImplementation;

  private void Start() {
    startButton.onClick.AddListener(LoadMainScene);
    quitButton.onClick.AddListener(QuitGame);

    Tween.Delay(1f).OnComplete(() => {
      sfxSource.PlayOneShot(menuSfx);
    });
  }

  private void LoadMainScene() {
    SceneManager.LoadScene(mainSceneName);
  }

  private void QuitGame() {
    Application.Quit();
  }
}
