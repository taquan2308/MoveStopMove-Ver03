using UnityEngine;
[CreateAssetMenu(fileName = "HornSO", menuName = "ScriptableObjects/HornSO")]
public class HornSO : ScriptableObject
{
    public Sprite iconHorn;
    public GameObject prefabsArrow;
    public GameObject prefabsHead;
    public int priceHorn;
    public int rangeAddHorn;
    public Material materialFullSet;
    public GameObject prefabsWing;
    public GameObject prefabsPan;
    public GameObject prefabsShield;
    public GameObject prefabsTail;
    public GameObject prefabsBeard;
    public Material materialPan;
    public string nameGroup;
    public string nameItem;
}
