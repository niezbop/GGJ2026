using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorFollower : MonoBehaviour {
  [SerializeField] private float gameStartActivationDelay = 6f;

  private Mouse mouse;
  private Camera mainCamera;
  private bool isFollowing = true;

  private void Start() {
    mouse = Mouse.current;
    mainCamera = Camera.main;

    // DisableCursorFollowAtGameStart();
  }

  private void Update() {

    var mouseScreenPosition = mouse.position.ReadValue();
    var cursorDirection = mainCamera.ScreenPointToRay(mouseScreenPosition).direction.normalized;

    if (!isFollowing) return;
    transform.forward = cursorDirection;
  }

  private void DisableCursorFollowAtGameStart() {
    isFollowing = false;
    Tween.Delay(gameStartActivationDelay).OnComplete(() => {
      isFollowing = true;
    });
  }
}
