using UnityEngine;
using Random = System.Random;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class CanvasGetReward : UICanvas, IInitializeVariables
{
    [SerializeField] private Button openBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private GameObject itemReward;
    [SerializeField] private Image imageItemReward;
    [SerializeField] private GameObject chestClose;
    [SerializeField] private GameObject chestOpen;
    private HornSO[] arrayHornSO;
    private ArrowSO[] arrayArrowSO;
    private HornSO hornSOReward;
    private ArrowSO arrowSOReward;
    [SerializeField] private GameObject textHaveFullItemGameObject;
    private bool isFirstUpdate;

    public Button OpenBtn { get => openBtn; set => openBtn = value; }
    public Image ImageItemReward { get => imageItemReward; set => imageItemReward = value; }
    private void OnEnable()
    {
        InitializeVariables();
    }
    private void Update()
    {
        if (!isFirstUpdate)
        {
            isFirstUpdate = true;
            arrayArrowSO = (UIManager.Instance.CanvasManagers[UIName.WeaponChose] as CanvasWeapon).ArrowList;// giu thu tu vi tri dong nay va dong duoi thi arrayArrowSO != null
            arrayHornSO = (UIManager.Instance.CanvasManagers[UIName.SkinShop] as CanvasSkinShop).ArrayHornSO;//doi thu tu vi tri dong nay va dong tren thi arrayArrowSO = null ? ko hieu
        }
    }
    public void Open()
    {
        chestClose.SetActive(false);
        itemReward.SetActive(true);
        chestOpen.SetActive(true);
        openBtn.gameObject.SetActive(false);
        if (!GetItemHornSO(arrayHornSO, arrayArrowSO))// check if not yet have full item
        {
            if (hornSOReward != null)
            {
                imageItemReward.sprite = hornSOReward.iconHorn;
                (UIManager.Instance.CanvasManagers[UIName.SkinShop] as CanvasSkinShop).EquipItemReward(hornSOReward);
            }
            else
            {
                imageItemReward.sprite = arrowSOReward.iconArrow;
                (UIManager.Instance.CanvasManagers[UIName.WeaponChose] as CanvasWeapon).EquipItemReward(arrowSOReward);
            }
        }
        else//  already have full item
        {
            itemReward.SetActive(false);
            textHaveFullItemGameObject.SetActive(true);
        }
    }
    public void ExitBtn()
    {
        UIManager.Instance.CloseUI(UIName.GetReward);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public bool GetItemHornSO(HornSO[] _arrayHorn, ArrowSO[] _arrayArrow)
    {
        bool isHaveFullItem = false;
        List<HornSO> notYetHornList = new List<HornSO>();
        List<ArrowSO> notYetArrowList = new List<ArrowSO>();
        notYetHornList.Clear();
        notYetArrowList.Clear();
        for (int i = 0; i < _arrayHorn.Length; i++)
        {
            if (PlayerPrefs.GetInt(_arrayHorn[i].nameItem) == 0)
            {
                notYetHornList.Add(_arrayHorn[i]);
            }
        }
        for (int i = 0; i < _arrayArrow.Length; i++)
        {
            if (PlayerPrefs.GetInt(_arrayArrow[i].nameArrow) == 0)
            {
                notYetArrowList.Add(_arrayArrow[i]);
            }
        }
        if ( (notYetHornList.Count + notYetArrowList.Count) == 0)
        {
            isHaveFullItem = true;
        }
        if (!isHaveFullItem)
        {
            Random rnd = new Random();
            int indexItem = rnd.Next(0, notYetHornList.Count + notYetArrowList.Count - 1);  // creates a number between 1 and 12
            if (indexItem < notYetHornList.Count && notYetHornList.Count > 0)
            {
                hornSOReward = notYetHornList[indexItem];
            }
            else
            {
                arrowSOReward = notYetArrowList[indexItem - notYetHornList.Count];
            }
        }
        return isHaveFullItem;
    }
    public void InitializeVariables()
    {
        //
        nameUI = UIName.GetReward;
        openBtn.onClick.AddListener(Open);
        exitBtn.onClick.AddListener(ExitBtn);
        isFirstUpdate = false;
        hornSOReward = null;
        arrowSOReward = null;
        textHaveFullItemGameObject.SetActive(false);
    }
}
