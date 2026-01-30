using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class MaskFeatures : MonoBehaviour {
  [Header("This mask's current features")]
  [SerializeField, Range(0, 2)] private int expressionFeature;
  [SerializeField, Range(0, 2)] private int hornFeature;
  [SerializeField, Range(0, 2)] private int maneFeature;

  [Header("All mask feature elements")]
  [SerializeField] private List<GameObject> expressionElements;
  [SerializeField] private List<GameObject> hornElements;
  [SerializeField] private List<GameObject> maneElements;

  private void Start() {
    UpdateMaskVisuals();
  }

  private void OnValidate() {
    UpdateMaskVisuals();
  }

  private void UpdateMaskVisuals() {
    for (int i = 0; i < expressionElements.Count; i++) {
      expressionElements[i].SetActive(i == expressionFeature);
    }
    for (int i = 0; i < hornElements.Count; i++) {
      hornElements[i].SetActive(i == hornFeature);
    }
    for (int i = 0; i < maneElements.Count; i++) {
      maneElements[i].SetActive(i == maneFeature);
    }
  }
}
