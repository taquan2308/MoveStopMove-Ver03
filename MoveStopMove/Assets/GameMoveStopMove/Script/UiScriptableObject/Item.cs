using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour, IInitializeVariables
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject lockImage;
    private Color choseColor;
    private Color normalColor;
    private PlayerMain playerMain;
    private HornSO hornSOThisItem;
    private CanvasSkinShop canvasSkinShop;
    private int indexHorn;
    public HornSO HornSOThisItem { get => hornSOThisItem; set => hornSOThisItem = value; }
    public CanvasSkinShop CanvasSkinShop { get => canvasSkinShop; set => canvasSkinShop = value; }
    public int IndexHorn { get => indexHorn; set => indexHorn = value; }
    public GameObject LockImage { get => lockImage; set => lockImage = value; }
    
    private void OnEnable()
    {
        InitializeVariables();
    }
    // Start is called before the first frame update
    public void ClickBtn()
    {
        canvasSkinShop = UIManager.Instance.canvasManagers[UIName.SkinShop] as CanvasSkinShop;
        foreach (Button btn in canvasSkinShop.ListBtnItem)
        {
            if (btn != null)
            {
                if (btn != this.button)
                {
                    var colors1 = btn.colors;
                    colors1.normalColor = normalColor;
                    btn.colors = colors1;
                }
                else
                {
                    var colors1 = btn.colors;
                    colors1.normalColor = choseColor;
                    btn.colors = colors1;
                }
            }
        }
        canvasSkinShop.SetPriceTxt(hornSOThisItem.priceHorn);
        canvasSkinShop.hornScriptableObjectChosen = hornSOThisItem;
        canvasSkinShop.IndexHorn = indexHorn;
        canvasSkinShop.CheckStateShowUi();
        canvasSkinShop.LockImage = lockImage;
    }

    public void InitializeVariables()
    {
        choseColor = new Color(1f, 1f, 1f);
        normalColor = new Color(0.8f, 0.8f, 0.8f);
        button.onClick.AddListener(ClickBtn);
        canvasSkinShop = UIManager.Instance.GetUI(UIName.SkinShop) as CanvasSkinShop;
        playerMain = PlayerMain.Instance;
        lockImage = transform.GetChild(1).gameObject;
    }
}
