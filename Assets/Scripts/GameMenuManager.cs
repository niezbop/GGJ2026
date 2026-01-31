using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour {
  [SerializeField] private LevelManager levelManager;

  [Header("Game over menu config")]
  [SerializeField] private GameObject gameOverMenuUI;
  [SerializeField] private TMP_Text gameOverLevelLabel;
  [SerializeField] private Button gameOverRestartButton;
  [SerializeField] private Button gameOverBackToMainMenuButton;

  [Header("Win menu config")]
  [SerializeField] private GameObject winMenuUI;
  [SerializeField] private Button winRestartButton;
  [SerializeField] private Button winBackToMainMenuButton;

  private void Start() {
    gameOverMenuUI.SetActive(false);
    winMenuUI.SetActive(false);
  }

  private void OnEnable() {
    gameOverRestartButton.onClick.AddListener(OnRestartClicked);
    gameOverBackToMainMenuButton.onClick.AddListener(OnBackToMainMenuClicked);

    winRestartButton.onClick.AddListener(OnRestartClicked);
    winBackToMainMenuButton.onClick.AddListener(OnBackToMainMenuClicked);
  }

  private void OnDisable() {
    gameOverRestartButton.onClick.RemoveListener(OnRestartClicked);
    gameOverBackToMainMenuButton.onClick.RemoveListener(OnBackToMainMenuClicked);

    winRestartButton.onClick.RemoveListener(OnRestartClicked);
    winBackToMainMenuButton.onClick.RemoveListener(OnBackToMainMenuClicked);
  }

  private void OnRestartClicked() {
    Debug.Log("Restarting game...");
    UnityEngine.SceneManagement.SceneManager.LoadScene(
      UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
  }

  private void OnBackToMainMenuClicked() {
    Debug.Log("Going back to main menu...");
    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
  }

  public void ShowWinMenu() {
    gameOverMenuUI.SetActive(false);
    winMenuUI.SetActive(true);
  }

  public void ShowGameOverMenu() {
    winMenuUI.SetActive(false);
    gameOverMenuUI.SetActive(true);

    var currentLevel = levelManager.CurrentLevelIndex + 1;
    gameOverLevelLabel.text = $"level {currentLevel}";
  }
}
