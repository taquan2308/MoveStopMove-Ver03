using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "ArrowSO", menuName = "ScriptableObjects/ArrowSO")]
public class ArrowSO : ScriptableObject
{
    public GameObject arrowPrefabs;
    public float speedArrow2;
    public bool isRoteArrow;
    public int speedRote;
    public bool isThreeDirection;
    public Sprite iconArrow;
    public int priceArrow;
    public string nameArrow;
}
