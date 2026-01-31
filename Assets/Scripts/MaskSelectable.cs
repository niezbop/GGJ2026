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

  private void OnEnable() {
    selectionLight.intensity = 0f;
    selectionLight.gameObject.SetActive(false);
  }

  private void OnDisable() {
    currentTween.Stop();
  }

  private void OnDestroy() {
    currentTween.Stop();
  }

  private void AnimateSelectionLightOff() {
    currentTween = Tween.LightIntensity(selectionLight, 0f, fadeDuration, Ease.OutQuad).OnComplete(() => {
      selectionLight.gameObject.SetActive(false);
    });
  }

  private void AnimateSelectionLightOn() {
    selectionLight.gameObject.SetActive(true);
    currentTween = Tween.LightIntensity(selectionLight, maxIntensity, fadeDuration, Ease.OutQuad);
  }

  public void SetSelected(bool selected) {
    currentTween.Stop();

    if (selectionLight == null) {
      return;
    }

    if (selected) {
      AnimateSelectionLightOn();
    } else {
      AnimateSelectionLightOff();
    }
  }
}
