using UnityEngine;
using UnityEngine.InputSystem;

public class CursorFollower : MonoBehaviour {
  private Mouse mouse;
  private Camera mainCamera;

  private void Start() {
    mouse = Mouse.current;
    mainCamera = Camera.main;
  }

  private void Update() {
    var mouseScreenPosition = mouse.position.ReadValue();
    var cursorDirection = mainCamera.ScreenPointToRay(mouseScreenPosition).direction.normalized;
    transform.forward = cursorDirection;
  }
}
