using UnityEngine;
[CreateAssetMenu(fileName = "PlayerSO", menuName = "ScriptableObjects/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    public float rangeAttack;
    public GameObject arrowPrefabs;
    public float speedAttack;
    public float turnSpeed;
    public int experience;
    public int gold;
}
