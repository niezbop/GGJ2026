using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour {
  [SerializeField] private LevelManager levelManager;

  [Header("Game over menu config")]
  [SerializeField] private GameObject gameOverMenuUI;
  [SerializeField] private TMP_Text gameOverLevelLabel;
  [SerializeField] private Button gameOverRestartButton;

  [Header("Win menu config")]
  [SerializeField] private GameObject winMenuUI;
  [SerializeField] private Button winRestartButton;

  private void Start() {
    gameOverMenuUI.SetActive(false);
    winMenuUI.SetActive(false);

    gameOverRestartButton.onClick.AddListener(OnRestartClicked);
    winRestartButton.onClick.AddListener(OnRestartClicked);
  }

  private void OnEnable() {
    gameOverRestartButton.onClick.AddListener(OnRestartClicked);
    winRestartButton.onClick.AddListener(OnRestartClicked);
  }

  private void OnDisable() {
    gameOverRestartButton.onClick.RemoveListener(OnRestartClicked);
    winRestartButton.onClick.RemoveListener(OnRestartClicked);
  }

  private void OnRestartClicked() {
    Debug.Log("Restarting game...");
    UnityEngine.SceneManagement.SceneManager.LoadScene(
      UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
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
