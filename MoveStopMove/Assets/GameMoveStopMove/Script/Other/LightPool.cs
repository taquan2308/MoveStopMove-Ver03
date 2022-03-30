using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPool : Singleton<LightPool>
{
    private IDictionary<int, List<GameObject>> pools;

    protected override void Awake()
    {
        base.Awake();

        pools = new Dictionary<int, List<GameObject>>();
    }


    public GameObject GetPrefab(GameObject obj)
    {
        var id = obj.GetInstanceID();

        if (pools.TryGetValue(id, out List<GameObject> objList))
        {
            var counter = objList.Count;

            for (int i = 0; i < counter; i++)
            {
                if (!objList[i].activeSelf)
                {
                    objList[i].SetActive(true);
                    return objList[i];
                }
            }

            var newObj = Instantiate(obj) as GameObject;
            objList.Add(newObj);

            if (!newObj.activeSelf)
            {
                newObj.SetActive(true);
            }

            return newObj;
        }
        else
        {
            var newObjList = new List<GameObject>();
            var newObj = Instantiate(obj) as GameObject;
            newObjList.Add(newObj);
            pools.Add(id, newObjList);

            return newObj;
        }
    }

    public void PushToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}

