using System;
using PrimeTweenDemo;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaskSelector : MonoBehaviour, IDisposable {
  [SerializeField] private float maxDistance = 20f;
  [SerializeField] private LayerMask maskLayer;

  public delegate void MaskSelectedEvent(MaskSelectable mask);
  public event MaskSelectedEvent OnMaskSelected = delegate { };

  [SerializeField] private InputAction clickAction;

  private MaskSelectable currentSelection;

  private void Start() {
    clickAction.performed += OnClick;
  }

  private void Update() {
    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    MaskSelectable newSelection = null;

    if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, maskLayer)) {
      newSelection = hit.collider.GetComponent<MaskSelectable>();
    }

    if (newSelection != currentSelection) {
      currentSelection?.SetSelected(false);
      newSelection?.SetSelected(true);
      currentSelection = newSelection;
    }
  }

  private void OnDrawGizmos() {
    Gizmos.color = Color.green;
    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    Gizmos.DrawRay(ray.origin, ray.direction * maxDistance);
  }

  private void OnClick(InputAction.CallbackContext _) {
    if (currentSelection != null) {
      OnMaskSelected?.Invoke(currentSelection);
    }
  }

  public void Dispose() {
    clickAction.performed -= OnClick;
  }
}
