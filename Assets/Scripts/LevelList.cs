using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "Levels/List")]
public class LevelList : ScriptableObject, IEnumerable<ILevel> {
  [SerializeField] AbstractLevel[] levels;

  public AbstractLevel[] Levels => levels;
  public AbstractLevel this[int index] => levels[index];
  public int Length => levels.Length;

  public IEnumerator<ILevel> GetEnumerator() {
    return levels.GetEnumerator() as IEnumerator<ILevel>;
  }

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
