using System;
using UnityEngine;

public class FlashlightShaker : MonoBehaviour
{
  private Transform initialTransform;
  [SerializeField] private Transform referenceTransform;

  [Header("Parameters X")]
  [SerializeField] private float xIntensity;
  [SerializeField] private float xSpeed;
  [SerializeField] private float xOffset;

  [Header("Parameters Y")]
  [SerializeField] private float yIntensity;
  [SerializeField] private float ySpeed;
  [SerializeField] private float yOffset;

  private void Start() {
    initialTransform = transform;
  }

  private void Update() {
    transform.RotateAround(referenceTransform.position, referenceTransform.up, xIntensity * Mathf.Cos(xSpeed * Time.time + xOffset));
    transform.RotateAround(referenceTransform.position, referenceTransform.right, yIntensity * Mathf.Cos(ySpeed * Time.time + yOffset));
  }
}
