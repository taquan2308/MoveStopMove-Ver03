using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PointSpawSO", menuName = "ScriptableObjects/PointSpawSO")]
public class PointSpawSO : ScriptableObject
{
    public Transform[] arrayTransformPoints = new Transform[4];
}
