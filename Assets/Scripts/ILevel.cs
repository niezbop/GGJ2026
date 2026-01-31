using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ILevel {
  public Dictionary<MaskFeatures.Configuration, CylindricalVector3> GetMasks();
  public bool IsIntruder(MaskFeatures.Configuration mask);
}

public class AbstractLevel : ScriptableObject, ILevel {
  public virtual Dictionary<MaskFeatures.Configuration, CylindricalVector3> GetMasks() => throw new System.NotImplementedException();
  public virtual bool IsIntruder(MaskFeatures.Configuration mask) => throw new System.NotImplementedException();
}
