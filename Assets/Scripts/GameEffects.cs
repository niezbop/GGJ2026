using System;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;

public class GameEffects : MonoBehaviour {
  [Header("SFX")]
  [SerializeField] private AudioSource sfxSource;
  [SerializeField] private AudioClip correctMaskSelectedSfx;
  [SerializeField] private float sfxVolume = .6f;

  [Header("Lights")]
  [SerializeField] private Light sceneSpotlight;
  [SerializeField] private Light[] flashlights;

  [Header("Transition Timings")]
  [SerializeField] private float transitionDuration = 2f;
  [SerializeField] private float blackoutDuration = 0.15f;

  [Header("Flashlight Flicker")]
  [SerializeField] private float flickerIntensityMin = 0.1f;
  [SerializeField] private float flickerIntensityMid = 0.5f;

  [Header("Mask Approach")]
  [SerializeField] private float maskApproachDistance = 1.5f;
  [SerializeField] private float maskScaleMultiplier = 1.3f;

  [Header("Camera Shake")]
  [SerializeField] private CinemachineImpulseSource impulseSource;
  [SerializeField] private float shakeStrength = 0.3f;
  [SerializeField] private float shakeDuration = 1.5f;

  private float originalSpotlightIntensity;
  private float[] originalFlashlightIntensities;
  private Sequence currentTransitionSequence;

  private void Start() {
    if (sceneSpotlight != null) {
      originalSpotlightIntensity = sceneSpotlight.intensity;
    }

    // Store original intensities for all flashlights
    if (flashlights != null && flashlights.Length > 0) {
      originalFlashlightIntensities = new float[flashlights.Length];
      for (int i = 0; i < flashlights.Length; i++) {
        if (flashlights[i] != null) {
          originalFlashlightIntensities[i] = flashlights[i].intensity;
        }
      }
    }
  }

  private void OnDestroy() {
    currentTransitionSequence.Stop();
  }

  /// <summary>
  /// Plays the full level win transition effect:
  /// - Mask approaches player ominously
  /// - Flashlight flickers and turns off
  /// - Scene spotlight dims
  /// - Mask's selection light fades
  /// - Camera shakes
  /// - Blackout moment (callback invoked here to load next level)
  /// - Lights restore
  /// </summary>
  /// <param name="selectedMask">The winning mask's MaskSelectable component</param>
  /// <param name="onBlackout">Callback invoked at blackout moment (use to load next level)</param>
  public void PlayLevelWinTransition(MaskSelectable selectedMask, Action onBlackout) {
    currentTransitionSequence.Stop();

    var maskTransform = selectedMask.transform;
    var maskSelectionLight = selectedMask.SelectionLight;
    var camera = Camera.main;

    // Calculate mask target position (towards camera)
    var directionToCamera = (camera.transform.position - maskTransform.position).normalized;
    var targetPosition = maskTransform.position + directionToCamera * maskApproachDistance;
    var originalMaskScale = maskTransform.localScale;
    var targetMaskScale = originalMaskScale * maskScaleMultiplier;

    // Store original selection light intensity
    float originalSelectionLightIntensity = maskSelectionLight != null ? maskSelectionLight.intensity : 0f;

    // === BUILD THE TRANSITION SEQUENCE ===
    currentTransitionSequence = Sequence.Create()
      // --- Phase 1: Ominous approach with flickering (parallel animations) ---
      // Mask moves towards player
      .Group(Tween.Position(maskTransform, targetPosition, transitionDuration, Ease.InQuad))
      // Mask scales up ominously
      .Group(Tween.Scale(maskTransform, targetMaskScale, transitionDuration, Ease.InQuad))

      // Scene spotlight gradually dims
      .Group(Tween.LightIntensity(sceneSpotlight, 0f, transitionDuration, Ease.InQuad))

      // Mask's selection light intensifies briefly then fades
      .Group(CreateSelectionLightFade(maskSelectionLight, originalSelectionLightIntensity, transitionDuration))

      // Flashlight flickers erratically then dies
      .Group(CreateFlashlightFlickerOff(transitionDuration))

      // Trigger camera shake at middle of the transition
      .InsertCallback(transitionDuration * 0.5f, () => impulseSource?.GenerateImpulse(shakeStrength))

      // Play correct mask selected SFX
      .InsertCallback(transitionDuration * 0.2f, () => {
        if (sfxSource != null && correctMaskSelectedSfx != null) {
          sfxSource.volume = sfxVolume;
          sfxSource.PlayOneShot(correctMaskSelectedSfx);
        }
      })

      // --- Phase 2: Blackout ---
      .ChainDelay(blackoutDuration)
      .ChainCallback(() => onBlackout?.Invoke())
      .ChainDelay(blackoutDuration)

      // --- Phase 3: Lights restore ---
      // Scene spotlight snaps back on
      .Chain(Tween.LightIntensity(sceneSpotlight, originalSpotlightIntensity, 0.1f, Ease.OutQuad))
      // Flashlight flickers back on
      .Chain(CreateFlashlightFlickerOn());
  }

  /// <summary>
  /// Creates a sequence that makes the mask's selection light intensify then fade out
  /// </summary>
  private Sequence CreateSelectionLightFade(Light selectionLight, float originalIntensity, float duration) {
    if (selectionLight == null) return Sequence.Create();

    float intensifyDuration = duration * 0.65f;  // Long ominous glow
    float fadeDuration = duration * 0.20f;        // Quick snap to black
    float peakIntensity = originalIntensity * 2f;

    return Tween.LightIntensity(selectionLight, peakIntensity, intensifyDuration, Ease.OutQuad)
      .Chain(Tween.LightIntensity(selectionLight, 0f, fadeDuration, Ease.InCubic));  // InCubic for sharper cutoff
  }

  /// <summary>
  /// Creates a sequence of erratic flashlight flickers ending in darkness
  /// </summary>
  private Sequence CreateFlashlightFlickerOff(float totalDuration) {
    if (flashlights == null || flashlights.Length == 0) return Sequence.Create();

    float t1 = totalDuration * 0.15f;
    float t2 = totalDuration * 0.1f;
    float t3 = totalDuration * 0.15f;
    float t4 = totalDuration * 0.1f;
    float t5 = totalDuration * 0.2f;
    float t6 = totalDuration * 0.1f;
    float t7 = totalDuration * 0.2f;

    var sequence = Sequence.Create();

    for (int i = 0; i < flashlights.Length; i++) {
      if (flashlights[i] == null) continue;
      float originalIntensity = originalFlashlightIntensities[i];
      var light = flashlights[i];

      sequence.Group(
        Tween.LightIntensity(light, flickerIntensityMid, t1, Ease.OutQuad)
          .Chain(Tween.LightIntensity(light, originalIntensity, t2, Ease.InQuad))
          .Chain(Tween.LightIntensity(light, flickerIntensityMin, t3, Ease.OutQuad))
          .Chain(Tween.LightIntensity(light, flickerIntensityMid, t4, Ease.InQuad))
          .Chain(Tween.LightIntensity(light, 0f, t5, Ease.OutQuad))
          .Chain(Tween.LightIntensity(light, flickerIntensityMin, t6, Ease.InQuad))
          .Chain(Tween.LightIntensity(light, 0f, t7, Ease.InQuad))
      );
    }

    return sequence;
  }

  /// <summary>
  /// Creates a sequence of flashlight flickering back to life
  /// </summary>
  private Sequence CreateFlashlightFlickerOn() {
    if (flashlights == null || flashlights.Length == 0) return Sequence.Create();

    var sequence = Sequence.Create();

    for (int i = 0; i < flashlights.Length; i++) {
      if (flashlights[i] == null) continue;
      float originalIntensity = originalFlashlightIntensities[i];
      var light = flashlights[i];

      sequence.Group(
        Tween.LightIntensity(light, flickerIntensityMin, 0.05f, Ease.OutQuad)
          .Chain(Tween.LightIntensity(light, 0f, 0.05f, Ease.InQuad))
          .Chain(Tween.LightIntensity(light, flickerIntensityMid, 0.08f, Ease.OutQuad))
          .Chain(Tween.LightIntensity(light, flickerIntensityMin, 0.05f, Ease.InQuad))
          .Chain(Tween.LightIntensity(light, originalIntensity, 0.15f, Ease.OutQuad))
      );
    }

    return sequence;
  }

  /// <summary>
  /// Simple standalone flashlight flicker (for other uses)
  /// </summary>
  public void FlickerFlashlight() {
    if (flashlights == null || flashlights.Length == 0) return;

    for (int i = 0; i < flashlights.Length; i++) {
      if (flashlights[i] == null) continue;
      float originalIntensity = originalFlashlightIntensities[i];
      var light = flashlights[i];

      Tween.LightIntensity(light, 0f, 0.05f, Ease.Linear)
        .Chain(Tween.LightIntensity(light, originalIntensity, 0.05f, Ease.Linear));
    }
  }
}
