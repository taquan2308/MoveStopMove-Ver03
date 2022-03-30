using UnityEngine;
using UnityEngine.UI;
public class CanvasJoystick : UICanvas, IInitializeVariables
{
    [SerializeField] private Button settingBtn;
    public void OpenSetting()
    {
        UIManager.Instance.OpenUI(UIName.Setting);
        UIManager.Instance.CloseUI(UIName.Joystick);
    }
    void Start()
    {
        InitializeVariables();
    }
    public void InitializeVariables()
    {
        settingBtn.onClick.AddListener(OpenSetting);
        nameUI = UIName.Joystick;
    }
}
