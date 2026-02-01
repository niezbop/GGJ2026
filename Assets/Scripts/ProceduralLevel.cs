using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Level", menuName = "Levels/Procedural")]
public class ProceduralLevel : AbstractLevel {
  [Range(6,30)]
  [SerializeField] private int maskCount;

  [Header("Placement configuration")]
  [Range(0,360)]
  [SerializeField] private float placementAngle;
  [SerializeField] private float offsetAngle = 90;
  [SerializeField] private float radius = 7;
  [SerializeField] private float radiusJiggle = .5f;
  [SerializeField] private float height = 2;
  [SerializeField] private float heightJiggle = .2f;

  private List<MaskFeatures.Configuration> maskConfigurations;
  private int intruderIndex;

  private class FeaturePool {
    private List<int> possibilities;

    public FeaturePool(int count, int variantsCount) {
      possibilities = new List<int>();
      // Add two examples of each variant to prevent uniqueness of each feature
      for (int i = 0; i < variantsCount; i++) {
        possibilities.Add(i);
        possibilities.Add(i);
      }

      for (int i = 0; i < count - variantsCount * 2; i++) {
        possibilities.Add(Random.Range(0, variantsCount));
      }
    }

    public FeaturePool(int count, int variantsCount, int lockedVariant) {
      possibilities = new List<int>();
      // Add two examples of the other variants to prevent uniqueness of other features
      var unlockedVariants = new List<int>();
      for (int i = 0; i < variantsCount; i++) {
        if (lockedVariant == i) { continue; }
        unlockedVariants.Add(i);
        possibilities.Add(i);
        possibilities.Add(i);
      }

      for (int i = 0; i < count - (variantsCount - 1) * 2; i++) {
        possibilities.Add(unlockedVariants[Random.Range(0, unlockedVariants.Count)]);
      }
    }

    public int Draw() {
      if (possibilities.Count == 0) {
        throw new IndexOutOfRangeException("No more possibilities to draw from");
      }

      var index = Random.Range(0, possibilities.Count);
      var drawn = possibilities[index];
      possibilities.RemoveAt(index);
      return drawn;
    }
  }

  public override List<Tuple<MaskFeatures.Configuration, CylindricalVector3>> GetMasks() {
    maskConfigurations = new List<MaskFeatures.Configuration>();

    /* FEATURES
     * 0 - expression
     * 1 - horns
     * 2 - mane
     * NOT YET 3 - decorations
     */

    int uniqueFeatureType = Random.Range(0, 3); // 3 features
    int uniqueFeatureVariant = Random.Range(0, 3); // 3 variants for each feature

    var expressionPool = uniqueFeatureType == 0
      ? new FeaturePool(maskCount, 3, uniqueFeatureVariant)
      : new FeaturePool(maskCount, 3);
    var hornsPool = uniqueFeatureType == 1
      ? new FeaturePool(maskCount, 3, uniqueFeatureVariant)
      : new FeaturePool(maskCount, 3);
    var manePool = uniqueFeatureType == 2
      ? new FeaturePool(maskCount, 3, uniqueFeatureVariant)
      : new FeaturePool(maskCount, 3);

    for (int i = 0; i < maskCount - 1; i++) {
      var maskFeature = new MaskFeatures.Configuration();
      maskFeature.expressionType = (ExpressionType)expressionPool.Draw();
      maskFeature.hornFeature = (HornType)hornsPool.Draw();
      maskFeature.maneFeature = (ManeType)manePool.Draw();

      maskConfigurations.Add(maskFeature);
    }

    // Generate intruder
    intruderIndex = Random.Range(0, maskCount - 1);
    var intruderConfiguration = new MaskFeatures.Configuration();
    intruderConfiguration.expressionType = (ExpressionType)(uniqueFeatureType == 0 ? uniqueFeatureVariant : expressionPool.Draw());
    intruderConfiguration.hornFeature = (HornType)(uniqueFeatureType == 1 ? uniqueFeatureVariant : hornsPool.Draw());
    intruderConfiguration.maneFeature = (ManeType)(uniqueFeatureType == 2 ? uniqueFeatureVariant : manePool.Draw());

    maskConfigurations.Insert(intruderIndex, intruderConfiguration);

    var angleIncrement = placementAngle / (maskCount - 1);
    var currentAngle = -placementAngle / 2 + offsetAngle;
    var maskPositions = new List<Tuple<MaskFeatures.Configuration, CylindricalVector3>>();
    foreach (var mask in maskConfigurations) {
      var position = new CylindricalVector3(
        radius + Random.Range(-radiusJiggle, radiusJiggle),
        currentAngle,
        height + Random.Range(-heightJiggle, heightJiggle)
      );
      maskPositions.Add(new Tuple<MaskFeatures.Configuration, CylindricalVector3>(mask, position));
      currentAngle += angleIncrement;
    }

    return maskPositions;
  }

  public override bool IsIntruder(MaskFeatures.Configuration mask) {
    var maskIndex = maskConfigurations.ToList().IndexOf(mask);
    return maskIndex == intruderIndex;
  }
}
