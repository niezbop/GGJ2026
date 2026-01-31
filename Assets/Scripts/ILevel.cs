using System;
using System.Collections.Generic;
using UnityEngine;

public interface ILevel {
  public List<Tuple<MaskFeatures.Configuration, CylindricalVector3?>> GetMasks();
  public bool IsIntruder(MaskFeatures.Configuration mask);
}

public class AbstractLevel : ScriptableObject, ILevel {
  public virtual List<Tuple<MaskFeatures.Configuration, CylindricalVector3?>> GetMasks() => throw new System.NotImplementedException();
  public virtual bool IsIntruder(MaskFeatures.Configuration mask) => throw new System.NotImplementedException();
}
