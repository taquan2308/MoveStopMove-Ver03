using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CanvasSetting : UICanvas, IInitializeVariables
{
    //Button
    [SerializeField] private Button homeBtn;
    [SerializeField] private Button continueBtn;
    //UI
    private PlayerMain playerMain;
    // Load weapon to UI
   
    public void Home()
    {
        UIManager.Instance.CloseUI(UIName.Setting);
        UIManager.Instance.OpenUI(UIName.Joystick);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Continue()
    {
        UIManager.Instance.CloseUI(UIName.Setting);
        UIManager.Instance.OpenUI(UIName.Joystick);
    }
    private void OnEnable()
    {
        InitializeVariables();
    }
    public void InitializeVariables()
    {
        playerMain = PlayerMain.Instance;
        homeBtn.onClick.AddListener(Home);
        continueBtn.onClick.AddListener(Continue);
        //
        nameUI = UIName.Setting;
    }
}
