using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Levels/Configurable")]
public class ConfigurableLevel : AbstractLevel {
  [System.Serializable]
  public struct LevelConfiguration {
    [SerializeField] public MaskFeatures.Configuration maskFeatures;
    [SerializeField] public CylindricalVector3 position;
  }

  [SerializeField] private LevelConfiguration[] levelConfigurations;
  [SerializeField] private int intruderIndex;

  public override List<Tuple<MaskFeatures.Configuration, CylindricalVector3>> GetMasks() {
    var masks = new List<Tuple<MaskFeatures.Configuration, CylindricalVector3>>();
    foreach (var levelConfiguration in levelConfigurations) {
      masks.Add(new Tuple<MaskFeatures.Configuration, CylindricalVector3>(levelConfiguration.maskFeatures, levelConfiguration.position));
    }
    return masks;
  }

  public override bool IsIntruder(MaskFeatures.Configuration mask) {
    var maskIndex = levelConfigurations.Select(x => x.maskFeatures).ToList().IndexOf(mask);
    return maskIndex == intruderIndex;
  }
}
