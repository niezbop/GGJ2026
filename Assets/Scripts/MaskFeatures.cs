using System.Collections.Generic;
using UnityEngine;

public class MaskFeatures : MonoBehaviour {
  [Header("This mask's current features")]
  [SerializeField, Range(0, 2)] private int maneFeature;
  [SerializeField, Range(0, 2)] private int hornFeature;
  [SerializeField, Range(0, 2)] private int expressionFeature;

  [Header("All mask feature elements")]
  [SerializeField] private List<GameObject> maneElements;
  [SerializeField] private List<GameObject> hornElements;
  [SerializeField] private List<GameObject> expressionElements;

  private void Start() {
    UpdateMaskVisuals();
  }

  private void OnValidate() {
    UpdateMaskVisuals();
  }

  private void UpdateMaskVisuals() {
    for (int i = 0; i < maneElements.Count; i++) {
      maneElements[i].SetActive(i == maneFeature);
    }
    for (int i = 0; i < hornElements.Count; i++) {
      hornElements[i].SetActive(i == hornFeature);
    }
    for (int i = 0; i < expressionElements.Count; i++) {
      expressionElements[i].SetActive(i == expressionFeature);
    }
  }
}
