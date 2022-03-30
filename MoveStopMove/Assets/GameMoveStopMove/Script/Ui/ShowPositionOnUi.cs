using UnityEngine;
using TMPro;
using System.Collections;
public class ShowPositionOnUi : MonoBehaviour,IInitializeVariables
{
    [SerializeField] private RectTransform parent;
    [SerializeField] private RectTransform image;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform enemy;
    [SerializeField] private float height;
    [SerializeField] private float with;
    //
    [SerializeField] private RectTransform pointCenterLook;
    [SerializeField] private TextMeshProUGUI levelEnemyTxt;
    [SerializeField] private EnemyMain enemyMain;
    [SerializeField] private RectTransform iconLevelTrans;
    [SerializeField] private Vector3 offSet9h;
    [SerializeField] private Vector3 offSet1h;
    [SerializeField] private Vector3 offSet11h;
    [SerializeField] private Vector3 offSet12h;
    [SerializeField] private Vector3 offSet3h;
    [SerializeField] private Vector3 offSet5h;
    [SerializeField] private Vector3 offSet6h;
    [SerializeField] private Vector3 offSet7h;
    [SerializeField] private int a;
    private Vector3 cameraPosOffset;
    private Vector3 playerPosition;
    public void InitializeVariables()
    {
        height = Screen.height;
        with = Screen.width;
        image = GetComponentsInChildren<RectTransform>()[1];
        mainCamera = CameraManager.Instance.MainCameraTrans.gameObject.GetComponent<Camera>();
        //pointCenterLook = GetComponentsInChildren<RectTransform>()[6];
        pointCenterLook = parent;
        levelEnemyTxt = GetComponentInChildren<TextMeshProUGUI>();
        
        iconLevelTrans = GetComponentsInChildren<RectTransform>()[3];
        //y +-800
        //x +- 400
        a = (int)with/10;
        offSet9h = new Vector3(a, 0, 0);
        offSet1h = new Vector3(-a, -a, 0);
        offSet11h = new Vector3(a, -a, 0);
        offSet12h = new Vector3(0, -a, 0);
        offSet3h = new Vector3(-a, 0, 0);
        offSet5h = new Vector3(-a, a, 0);
        offSet6h = new Vector3(0, a, 0);
        offSet7h = new Vector3(a, a, 0);
        cameraPosOffset = new Vector3(15f, 15f, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();
        
    }
    public void GetTransformEnemy(Transform _enemy)
    {
        this.enemy = _enemy;
        enemyMain = enemy.gameObject.GetComponent<EnemyMain>();
    }
    // Update is called once per frame
    void Update()
    {
        playerPosition = mainCamera.gameObject.transform.position - cameraPosOffset;
        levelEnemyTxt.text = enemyMain.Experience.ToString();
        CheckActive();
        if (enemy.CompareTag("Enemy") == false)
        {
            Destroy(this.gameObject);
        }
        Vector2 anchoPos;
        Vector2 centerPos = new Vector2(with / 2, height / 2);
        Vector3 enemySeen;
        enemySeen = mainCamera.WorldToScreenPoint(enemy.position);
        Vector2 enemySeen2D = new Vector2(enemySeen.x, enemySeen.y);
        Vector2 enemychuanhoa = new Vector2(0, 0);
        float a1 = (centerPos.x * enemySeen.y - centerPos.y * enemySeen.x);
        float a2 = a/2;
        float a3 = with - a/2;
        float a4 = height - a/2;
        float a5 = enemySeen.x - centerPos.x;
        float a6 = enemySeen.y - centerPos.y;
        if (enemySeen.x >= 0 && enemySeen.x <= with && enemySeen.y >= 0 && enemySeen.y <= height)
        {
            image.gameObject.SetActive(false);
            return;
        }
        if (image.gameObject.activeSelf == false)
        {
            image.gameObject.SetActive(true);
        }
        Vector2 point11h = new Vector2(a2, a4);
        Vector2 point2h = new Vector2(a3, a4);
        Vector2 point5h = new Vector2(a3, a2);
        Vector2 point7h = new Vector2(a2, a2);
        Vector2 CenterTo11h = centerPos - point11h;
        Vector2 CenterTo2h = centerPos - point2h;
        Vector2 CenterTo5h = centerPos - point5h;
        Vector2 CenterTo7h = centerPos - point7h;
        Vector2 CenterToChuanHoa = centerPos - enemySeen2D;
        //
        int key = 3;
        if (Vector2.Angle(CenterToChuanHoa, CenterTo11h) <= Vector2.Angle(CenterTo11h, CenterTo2h) && Vector2.Angle(CenterToChuanHoa, CenterTo2h) <= Vector2.Angle(CenterTo11h, CenterTo2h) && enemy.position.x <= playerPosition.x)
        {
            key = 1;
        }
        else if (Vector2.Angle(CenterToChuanHoa, CenterTo2h) <= Vector2.Angle(CenterTo2h, CenterTo5h) && Vector2.Angle(CenterToChuanHoa, CenterTo5h) <= Vector2.Angle(CenterTo2h, CenterTo5h) && CenterToChuanHoa.x < 0)
        {
            key = 2;
        }
        else if (Vector2.Angle(CenterToChuanHoa, CenterTo5h) <= Vector2.Angle(CenterTo5h, CenterTo7h) && Vector2.Angle(CenterToChuanHoa, CenterTo7h) <= Vector2.Angle(CenterTo5h, CenterTo7h) && enemy.position.x >= playerPosition.x)
        {
            key = 3;
        }
        else if (Vector2.Angle(CenterToChuanHoa, CenterTo7h) <= Vector2.Angle(CenterTo7h, CenterTo11h) && Vector2.Angle(CenterToChuanHoa, CenterTo11h) <= Vector2.Angle(CenterTo7h, CenterTo11h))
        {
            key = 4;
        }
        //
        if (key == 1)
        {
            enemychuanhoa = new Vector2((a1 + a4 * a5) / a6, a4);
        }
        else if (key == 2)
        {
            enemychuanhoa = new Vector2(a3, (-a1 + a3 * a6) / a5);
        }
        else if (key == 3)
        {
            enemychuanhoa = new Vector2((a1 + a2 * a5) / a6, a2);
        }
        else if (key == 4)
        {
            enemychuanhoa = new Vector2(a2, (-a1 + a2 * a6) / a5);
        }
        //
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, enemychuanhoa, mainCamera, out anchoPos);
        image.anchoredPosition = anchoPos;

        Vector3 dirCenterToIcon = pointCenterLook.position - image.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dirCenterToIcon);
        image.transform.rotation = lookRotation;
        //
        CheckActive();
        //
        Debug.DrawLine(pointCenterLook.position, enemy.position);
    }
    public void CheckActive()
    {
        if (image.gameObject.activeSelf == true)
        {
            iconLevelTrans.gameObject.SetActive(true);
            SetPositionIconLevel();
        }
        else
        {
            iconLevelTrans.gameObject.SetActive(false);
        }
    }
    public void SetPositionIconLevel()
    {
        //y +-800
        //x +- 400
        if (image.position.y >= -800 && image.position.y <= 800 & image.localPosition.x < 0)
        {
            iconLevelTrans.localPosition = image.localPosition + offSet9h;
        }
        else if (image.localPosition.y > 800 && image.localPosition.x < 0)
        {
            iconLevelTrans.localPosition = image.localPosition + offSet11h;
        }
        else if (image.localPosition.y > 0 && image.localPosition.x <= 400 & image.localPosition.x > -400)
        {
            iconLevelTrans.localPosition = image.localPosition + offSet12h;//
        }
        else if (image.localPosition.y > 800 && image.localPosition.x > 0)
        {
            iconLevelTrans.localPosition = image.localPosition + offSet1h;
        }
        if (image.localPosition.y >= -800 && image.localPosition.y <= 800 & image.localPosition.x > 0)
        {
            iconLevelTrans.localPosition = image.localPosition + offSet3h;
        }
        else if (image.localPosition.y < -800 && image.localPosition.x > 0)
        {
            iconLevelTrans.localPosition = image.localPosition + offSet5h;
        }
        else if (image.localPosition.y < 0 && image.localPosition.x <= 400 & image.localPosition.x > -400)
        {
            iconLevelTrans.localPosition = image.localPosition + offSet6h;//
        }
        else if (image.localPosition.y < -800 && image.localPosition.x < 0)
        {
            iconLevelTrans.localPosition = image.localPosition + offSet7h;
        }
    }
}

//float a2 = 50;
//float a3 = with - 50;
//float a4 = height - 50;