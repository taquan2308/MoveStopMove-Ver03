using UnityEngine;

public class SpawnArrow : Singleton<SpawnArrow>
{
    public GameObject Spawns(GameObject elementUIPrefab)
    {
        var element = LightPool.Instance.GetPrefab(elementUIPrefab);
        return element;
    }
}
