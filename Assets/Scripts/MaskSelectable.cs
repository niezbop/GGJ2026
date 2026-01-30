using PrimeTween;
using UnityEngine;

public class MaskSelectable : MonoBehaviour {
  [SerializeField] private Light selectionLight;
  [SerializeField] private float fadeDuration = 3f;

  private float maxIntensity = 0f;
  private Tween currentTween;

  private void Awake() {
    maxIntensity = selectionLight.intensity;
  }

  private void Start() {
    selectionLight.intensity = 0f;
  }

  private void OnDestroy() {
    currentTween.Stop();
  }

  public void SetSelected(bool selected) {
    currentTween.Stop();
    float targetIntensity = selected ? maxIntensity : 0f;
    currentTween = Tween.LightIntensity(selectionLight, targetIntensity, fadeDuration, Ease.OutQuad);
  }
}
