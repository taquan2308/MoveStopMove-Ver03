using UnityEngine;
using UnityEngine.UI;
public class CanvasVictory : UICanvas, IInitializeVariables
{
    [SerializeField] private Button claimBtn;
    public Button ClaimBtn { get => claimBtn; set => claimBtn = value; }
    private void OnEnable()
    {
        InitializeVariables();
    }
    public void Claim()
    {
        UIManager.Instance.OpenUI(UIName.GetReward);
        UIManager.Instance.CloseUI(UIName.Victory);
    }
    public void InitializeVariables()
    {
        //
        nameUI = UIName.Victory;
        claimBtn.onClick.AddListener(Claim);
    }
}
