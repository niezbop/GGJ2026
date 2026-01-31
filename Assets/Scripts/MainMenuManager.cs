using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
  [SerializeField] private string mainSceneName = "MainScene";
  [SerializeField] private Button startButton;
  private IDisposable disposableImplementation;

  private void Start() {
    startButton.onClick.AddListener(LoadMainScene);
  }

  private void LoadMainScene() {
    SceneManager.LoadScene(mainSceneName);
  }
}
