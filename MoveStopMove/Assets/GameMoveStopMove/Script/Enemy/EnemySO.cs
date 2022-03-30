using UnityEngine;
[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObjects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public float rangeAttack;
    public GameObject arrowPrefabs;
    public float speedAttack;
    public float turnSpeed;
    public int experience;
    public float timeIdleStart;
    public string nameEnemy;
}
