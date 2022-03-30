using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyManager : Singleton<SpawnEnemyManager>, IInitializeVariables
{
    //Array of point spaw
    [SerializeField] private GameObject[] arrEnemyPrefabs;
    private int indexPrefabsInArrSpawn;
    private int maxEnemyInMapNow;
    [SerializeField] private Transform[] arrayTransformPoints = new Transform[4];
    private int indextOfCornerHasLeastEnemy;
    private int topLeft;
    private int topRight;
    private int bootLeft;
    private int bootRight;
    //
    private bool isSpawn;
    private PlayerMain playerMain;
    //Point Center to check 4 coner
    [SerializeField] private Transform pointCenterMapTrans;
    //
    private float timeStart;
    private float timeCountdown00;
    private float timeCountdown01;
    private float timeCountdown02;
    private float timeCountdown03;
    //
    private int[] arr;
    //
    private float maxDistancePlayerToPointSpawn;
    public GameObject[] ArrEnemyPrefabs { get => arrEnemyPrefabs; set => arrEnemyPrefabs = value; }
    protected override void Awake()
    {
        base.Awake();
    }
    private void OnEnable()
    {
        InitializeVariables();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (playerMain == null)
        {
            playerMain = PlayerMain.Instance;
        }
    }
    // Update is called once per frame
    private void Update()
    {
        Check();
    }
    public void Check()
    {
        // Check To Spaw
        if (GameManager.Instance.ListCharacter.Count < maxEnemyInMapNow && isSpawn && indexPrefabsInArrSpawn < arrEnemyPrefabs.Length )
        {
            isSpawn = false;
            SpawEnemy();
        }
        if (!isSpawn)
        {
            isSpawn = true;
            indextOfCornerHasLeastEnemy = -1;
            topLeft = 0;
            topRight = 0;
            bootLeft = 0;
            bootRight = 0;
        }
    }
    public void SpawEnemy()
    {
        FindCornerToSpaw();
        if (indextOfCornerHasLeastEnemy != -1)
        {
            GameObject enemySpawed = (GameObject)Instantiate(arrEnemyPrefabs[indexPrefabsInArrSpawn], arrayTransformPoints[indextOfCornerHasLeastEnemy].position, transform.rotation);
            enemySpawed.GetComponent<EnemyMain>().StartDeadInverse();
            //StartCoroutine(delay(enemySpawed));// because conflict order inistalize isPlay two time so delay set bool to actualy isPlay = true
            indexPrefabsInArrSpawn += 1;
            isSpawn = false;
        }
    }
    #region FindCornerToSpaw
    public void FindCornerToSpaw() 
    {
        
        for (int i = 0; i < GameManager.Instance.ListTransformCharacter.Count; i++)
        {
            if (GameManager.Instance.ListTransformCharacter[i] != null)
            {
                if (GameManager.Instance.ListTransformCharacter[i].position.z < pointCenterMapTrans.position.z && GameManager.Instance.ListTransformCharacter[i].position.x < pointCenterMapTrans.position.x)
                {
                    topLeft += 1;
                    if (playerMain != null)
                    {
                        if (Vector3.Magnitude(playerMain.gameObject.transform.position - arrayTransformPoints[0].position) <= maxDistancePlayerToPointSpawn)
                        {
                            topLeft += 1000;
                        }
                    }
                }
                else if (GameManager.Instance.ListTransformCharacter[i].position.z > pointCenterMapTrans.position.z && GameManager.Instance.ListTransformCharacter[i].position.x < pointCenterMapTrans.position.x)
                {
                    topRight += 1;
                    if (playerMain != null)
                    {
                        if (Vector3.Magnitude(playerMain.gameObject.transform.position - arrayTransformPoints[1].position) <= maxDistancePlayerToPointSpawn)
                        {
                            topRight += 1000;
                        }
                    }
                }
                else if (GameManager.Instance.ListTransformCharacter[i].position.z > pointCenterMapTrans.position.z && GameManager.Instance.ListTransformCharacter[i].position.x > pointCenterMapTrans.position.x)
                {
                    bootRight += 1;
                    if (playerMain != null)
                    {
                        if (Vector3.Magnitude(playerMain.gameObject.transform.position - arrayTransformPoints[2].position) <= maxDistancePlayerToPointSpawn)
                        {
                            bootRight += 1000;
                        }
                    }
                }
                else if (GameManager.Instance.ListTransformCharacter[i].position.z < pointCenterMapTrans.position.z && GameManager.Instance.ListTransformCharacter[i].position.x > pointCenterMapTrans.position.x)
                {
                    bootLeft += 1;
                    if (playerMain != null)
                    {
                        if (Vector3.Magnitude(playerMain.gameObject.transform.position - arrayTransformPoints[3].position) <= maxDistancePlayerToPointSpawn)
                        {
                            bootLeft += 1000;
                        }
                    }
                }
            }
        }
        arr = new int[] { topLeft, topRight, bootLeft, bootRight };
        arr = SortArrayIncrease(arr);
        int minCorner = arr[0];
        if (minCorner == topLeft)
        {
            if(timeCountdown00 <= 0)
            {
                indextOfCornerHasLeastEnemy = 0;
                timeCountdown00 = timeStart;
            }
            else
            {
                minCorner = arr[1];
            }
        }
        else if (minCorner == topRight)
        {
            if (timeCountdown01 <= 0)
            {
                indextOfCornerHasLeastEnemy = 1;
                timeCountdown01 = timeStart;
            }
            else
            {
                minCorner = arr[2];
            }
        }
        else if (minCorner == bootRight)
        {
            if (timeCountdown02 <= 0)
            {
                indextOfCornerHasLeastEnemy = 2;
                timeCountdown02 = timeStart;
            }
            else
            {
                minCorner = arr[3];
            }
        }
        else if (minCorner == bootLeft)
        {
            if (timeCountdown03 <= 0)
            {
                indextOfCornerHasLeastEnemy = 3;
                timeCountdown03 = timeStart;
            }
        }
        //
        timeCountdown00 -= Time.deltaTime;
        timeCountdown00 = Mathf.Clamp(timeCountdown00, 0, Mathf.Infinity);
        timeCountdown01 -= Time.deltaTime;
        timeCountdown01 = Mathf.Clamp(timeCountdown01, 0, Mathf.Infinity);
        timeCountdown02 -= Time.deltaTime;
        timeCountdown02 = Mathf.Clamp(timeCountdown02, 0, Mathf.Infinity);
        timeCountdown03 -= Time.deltaTime;
        timeCountdown03 = Mathf.Clamp(timeCountdown03, 0, Mathf.Infinity);
        // find indextOfCornerHasLeastEnemy
    }
    #endregion
    public void InitializeVariables()
    {
        indexPrefabsInArrSpawn = 0;
        maxEnemyInMapNow = 8;
        isSpawn = true;
        topLeft = 0;
        topRight = 0;
        bootLeft = 0;
        bootRight = 0;
        indextOfCornerHasLeastEnemy = -1;
        //
        timeStart = 4f;
        timeCountdown00 = 0;
        timeCountdown01 = 0;
        timeCountdown02 = 0;
        timeCountdown03 = 0;
        maxDistancePlayerToPointSpawn = 5;
}
    public int[] SortArrayIncrease(int[] _array)
    {
        int lenght = _array.Length;
        for (int i = 0; i < lenght; i++)
        {
            for (int j = i + 1; j < lenght; j++)
            {
                if (_array[i] > _array[j])
                {
                    int temp = _array[i];
                    _array[i] = _array[j];
                    _array[j] = temp;
                }
            }
        }
        return _array;
    }
}
