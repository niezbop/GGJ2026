using PrimeTween;
using UnityEngine;

public class MaskSelectable : MonoBehaviour {
  [SerializeField] private Light selectionLight;
  [SerializeField] private float fadeDuration = 3f;

  [Header("Selected SFX")]
  [SerializeField] private AudioClip[] maskSelectedSfx;
  [SerializeField] private float sfxVolume = .5f;

  private float maxIntensity = 0f;
  private Tween currentLightTween;
  private Tween currentSfxTween;
  private AudioSource sfxSource;

  public Light SelectionLight => selectionLight;

  private void Awake() {
    maxIntensity = selectionLight.intensity;
  }

  private void OnEnable() {
    selectionLight.intensity = 0f;
    selectionLight.gameObject.SetActive(false);
  }

  private void OnDisable() {
    currentLightTween.Stop();
    FadeOutSFX();
  }

  private void OnDestroy() {
    currentLightTween.Stop();
    FadeOutSFX();
  }

  private void FadeOutSFX() {
    if (sfxSource != null && sfxSource.volume > 0f) {
      currentSfxTween = Tween.AudioVolume(sfxSource, 0f, .6f, Ease.OutQuad).OnComplete(() => {
        sfxSource.Stop();
      });
    }
  }

  private void AnimateSelectionLightOff() {
    currentLightTween = Tween.LightIntensity(selectionLight, 0f, fadeDuration, Ease.OutQuad).OnComplete(() => {
      selectionLight.gameObject.SetActive(false);
    });
    FadeOutSFX();
  }

  private void AnimateSelectionLightOn() {
    selectionLight.gameObject.SetActive(true);
    currentLightTween = Tween.LightIntensity(selectionLight, maxIntensity, fadeDuration, Ease.OutQuad);

    // Play mask selected SFX
    currentSfxTween.Stop();
    sfxSource.volume = sfxVolume;
    int randomIndex = Random.Range(0, maskSelectedSfx.Length);
    sfxSource.PlayOneShot(maskSelectedSfx[randomIndex]);
  }

  public void SetAudioSource(AudioSource source) {
    sfxSource = source;
  }

  public void SetSelected(bool selected) {
    currentLightTween.Stop();

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
