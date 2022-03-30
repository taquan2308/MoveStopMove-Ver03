using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum UIName {Setting, Play, GameOver, SkinShop, WeaponChose, CenterBoot, RightTop, Joystick, Lose, Live, Victory, GetReward}
public class UIManager : Singleton<UIManager>, IInitializeVariables, ISubcribers
{
    
    public Dictionary<UIName, UICanvas> canvasPrefabs = new Dictionary<UIName, UICanvas>();
    public Dictionary<UIName, UICanvas> canvasManagers = new Dictionary<UIName, UICanvas>();
    private Transform parent;
    [SerializeField] private TextMeshProUGUI txtLive;
    [SerializeField] private TextMeshProUGUI txtLevel;
    [SerializeField] private GameObject showPositionOnUi;
    [SerializeField] private GameObject canvasLive;
    //
    [SerializeField] private UICanvas canvasCenterBoot;
    [SerializeField] private UICanvas canvasGameOver;
    [SerializeField] private UICanvas canvasGameLose;
    [SerializeField] private UICanvas canvasSkinShop;
    [SerializeField] private UICanvas canvasWeapon;
    [SerializeField] private UICanvas canvasRightTop;
    [SerializeField] private UICanvas canvasJoystick;
    [SerializeField] private UICanvas canvasLiveUI;
    [SerializeField] private UICanvas canvasSetting;
    [SerializeField] private UICanvas canvasVictory;
    [SerializeField] private UICanvas canvasGetReward;

    public TextMeshProUGUI TxtLive { get => txtLive; set => txtLive = value; }
    public GameObject ShowPositionOnUi { get => showPositionOnUi; set => showPositionOnUi = value; }
    public GameObject CanvasLive { get => canvasLive; set => canvasLive = value; }
    public Dictionary<UIName, UICanvas> CanvasManagers { get => canvasManagers; set => canvasManagers = value; }

    void Start()
    {
        InitializeVariables();
        SubscribeEvent();
    }
    public UICanvas OpenUI(UIName name)
    {
        UICanvas canvas = null;
        if (!canvasManagers.ContainsKey(name) || canvasManagers[name] == null)
        {
            canvas = Instantiate(canvasPrefabs[name], parent);
            canvasManagers.Add(name, canvas);
        }
        else
        {
            canvas = canvasManagers[name];
        }
        canvas.OnInit();
        canvas.Open();
        return canvas;
    }
    public void CloseUI(UIName name)
    {
        if (canvasManagers.ContainsKey(name))
        {
            canvasManagers[name].Close();
        }
    }
    public bool IsOpenedUI(UIName name)
    {
        bool isOpen = false;
        
        if (canvasManagers.ContainsKey(name))
        {
            if (canvasManagers[name].gameObject.activeSelf)
            {
                isOpen = true;
            }
        }
        return isOpen;
    }
    public UICanvas GetUI(UIName name)
    {
        UICanvas canvas = null;
        
        if (canvasManagers.ContainsKey(name))
        {
            canvas = canvasManagers[name];
        }
        else
        {
            Debug.Log("Can't find UI...");
        }
        return null;
    }
    public UICanvas GetUIPrefabs(UIName name)
    {
        UICanvas canvas = null;
        
        if (canvasPrefabs.ContainsKey(name))
        {
            canvas = canvasPrefabs[name];
        }
        else
        {
            Debug.Log("Can't find UI...");
        }
        return canvas;
    }
    //
    public void SubscribeEvent()
    {
        GameManager.Instance.OnGameStarted += GameStarted;
        GameManager.Instance.OnLevelCompleted += ShowCanvasGameOver;
        GameManager.Instance.OnLevelCompleted += HideCanvasJoystick;
        GameManager.Instance.OnGameLose += ShowCanvasGameLose;
        GameManager.Instance.OnGameLose += HideCanvasJoystick;
    }

    public void UnSubscribeEvent()
    {
        GameManager.Instance.OnGameStarted -= GameStarted;
        GameManager.Instance.OnLevelCompleted -= ShowCanvasGameOver;
        GameManager.Instance.OnLevelCompleted -= HideCanvasJoystick;
        GameManager.Instance.OnGameLose -= HideCanvasJoystick;
        GameManager.Instance.OnGameLose -= ShowCanvasGameLose;
    }
    //
    public void UpdateAlives()
    {
        txtLive.text = "Alive: " + GameManager.Instance.EnemyAlive.ToString();
    }
    public void ShowCanvasGameOver()
    {
        OpenUI(UIName.GameOver);
    }
    public void GameStarted(int currentLevel)
    {
        txtLevel.text = "LEVEL " + currentLevel.ToString();
    }
    public void ShowCanvasGameLose()
    {
        OpenUI(UIName.Lose);
    }
    public void HideCanvasJoystick()
    {
        CloseUI(UIName.Joystick);
    }
    public void InitializeVariables()
    {
        canvasManagers.Add(UIName.CenterBoot, canvasCenterBoot);
        canvasManagers.Add(UIName.GameOver, canvasGameOver);
        canvasManagers.Add(UIName.SkinShop, canvasSkinShop);
        canvasManagers.Add(UIName.WeaponChose, canvasWeapon);
        canvasManagers.Add(UIName.RightTop, canvasRightTop);
        canvasManagers.Add(UIName.Joystick, canvasJoystick);
        canvasManagers.Add(UIName.Lose, canvasGameLose);
        canvasManagers.Add(UIName.Live, canvasLiveUI);
        canvasManagers.Add(UIName.Setting, canvasSetting);
        canvasManagers.Add(UIName.Victory, canvasVictory);
        canvasManagers.Add(UIName.GetReward, canvasGetReward);
        OpenUI(UIName.RightTop);
        OpenUI(UIName.CenterBoot);
        OpenUI(UIName.Live);
        CloseUI(UIName.WeaponChose);
        CloseUI(UIName.SkinShop);
        CloseUI(UIName.GameOver);
        CloseUI(UIName.Lose);
        CloseUI(UIName.Joystick);
        CloseUI(UIName.Setting);
        CloseUI(UIName.Victory);
        CloseUI(UIName.GetReward);
        txtLevel.text = "LEVEL " + GameManager.Instance.CurrentLevel.ToString();
    }
}

//private void Awake()
//{
//    UICanvas[] canvas = Resources.LoadAll<UICanvas>("UI/");
//    for (int i = 0; i < canvas.Length; i++)
//    {
//        canvasPrefabs.Add(canvas[i].nameUI, canvas[i]);
//    }
//}