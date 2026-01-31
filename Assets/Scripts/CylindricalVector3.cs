using UnityEngine;

[System.Serializable]
public struct CylindricalVector3 {
  // Radius
  [SerializeField] float r;
  // Angle (in degrees)
  [SerializeField] float a;
  // Height
  [SerializeField] float h;

  public float Radius => r;
  public float Angle => a;
  public float AngleRadiants => Angle * Mathf.Deg2Rad;
  public float Height => h;

  public CylindricalVector3(float r, float a, float h) {
    this.r = r;
    this.a = a;
    this.h = h;
  }

  public Vector3 ToVector3() {
    return new Vector3(
      Radius * Mathf.Cos(AngleRadiants),
      Height,
      Radius * Mathf.Sin(AngleRadiants)
    );
  }
}
