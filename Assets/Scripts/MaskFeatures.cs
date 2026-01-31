using System.Collections.Generic;
using UnityEngine;

public enum ExpressionType { Surprise, Creepy, Stoic }
public enum HornType { Ears, Goat, Halo }
public enum ManeType { None, Fur, Feather }

[SelectionBase]
public class MaskFeatures : MonoBehaviour {
  [System.Serializable]
  public struct Configuration {
    [SerializeField] private ExpressionType expressionType;
    [SerializeField] private HornType hornFeature;
    [SerializeField] private ManeType maneFeature;
  }

  [Header("This mask's current features")]
  [SerializeField] private ExpressionType expressionFeature;
  [SerializeField] private HornType hornFeature;
  [SerializeField] private ManeType maneFeature;

  [Header("Feature element references (order must match enum order)")]
  [SerializeField] private List<GameObject> expressionElements;
  [SerializeField] private List<GameObject> hornElements;
  [SerializeField] private List<GameObject> maneElements;

  private void Start() {
    UpdateMaskVisuals();
#if UNITY_EDITOR
    ValidateFeatureLists();
#endif
  }

  private void OnValidate() {
    UpdateMaskVisuals();
    ValidateFeatureLists();
  }

  private void ValidateFeatureLists() {
    if (expressionElements.Count != ExpressionCount) {
      Debug.LogError($"[{gameObject.name}] Expression elements count ({expressionElements.Count}) doesn't match ExpressionType enum count ({ExpressionCount})", this);
    }
    if (hornElements.Count != HornCount) {
      Debug.LogError($"[{gameObject.name}] Horn elements count ({hornElements.Count}) doesn't match HornType enum count ({HornCount})", this);
    }
    if (maneElements.Count != ManeCount) {
      Debug.LogError($"[{gameObject.name}] Mane elements count ({maneElements.Count}) doesn't match ManeType enum count ({ManeCount})", this);
    }
  }

  private void UpdateMaskVisuals() {
    for (int i = 0; i < expressionElements.Count; i++) {
      expressionElements[i].SetActive(i == (int)expressionFeature);
    }
    for (int i = 0; i < hornElements.Count; i++) {
      hornElements[i].SetActive(i == (int)hornFeature);/*  */
    }
    for (int i = 0; i < maneElements.Count; i++) {
      maneElements[i].SetActive(i == (int)maneFeature);
    }
  }

  public void SetMaskFeatures(ExpressionType expr, HornType horn, ManeType mane) {
    expressionFeature = expr;
    hornFeature = horn;
    maneFeature = mane;
    UpdateMaskVisuals();
  }

  public void SetRandomFeatures() {
    expressionFeature = RandomExpression();
    hornFeature = RandomHorn();
    maneFeature = RandomMane();
    UpdateMaskVisuals();
  }

  public bool Matches(ExpressionType expr, HornType horn, ManeType mane) {
    return expressionFeature == expr && hornFeature == horn && maneFeature == mane;
  }

  // Static methods

  public static int ExpressionCount => System.Enum.GetValues(typeof(ExpressionType)).Length;
  public static int HornCount => System.Enum.GetValues(typeof(HornType)).Length;
  public static int ManeCount => System.Enum.GetValues(typeof(ManeType)).Length;

  public static int TotalCombinations => ExpressionCount * HornCount * ManeCount;

  public static ExpressionType RandomExpression() => (ExpressionType)Random.Range(0, ExpressionCount);
  public static HornType RandomHorn() => (HornType)Random.Range(0, HornCount);
  public static ManeType RandomMane() => (ManeType)Random.Range(0, ManeCount);

  public static (ExpressionType, HornType, ManeType) RandomCombination() =>
    (RandomExpression(), RandomHorn(), RandomMane());
}
