using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasWeapon : UICanvas, IInitializeVariables
{
    //Button
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button beforeBtn;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button purchaseBtn;
    [SerializeField] private Button adsBtn;
    [SerializeField] private Image iconArrow;
    //Price
    [SerializeField] private TextMeshProUGUI lockTxt;
    [SerializeField] private TextMeshProUGUI priceTxt;
    [SerializeField] private Image goldImage;
    [SerializeField] private TextMeshProUGUI adsCountTxt;
    //
    [SerializeField] private TextMeshProUGUI equipedTxt;
    [SerializeField] private TextMeshProUGUI selectTxt;
    private bool isAdsClick;
    //UI
    private PlayerMain playerMain;
    private Vector3 offsetArrow;
    // Load weapon to UI
    [SerializeField] private ArrowSO[] arrowList;
    private List <int> indexEquipedList;
    private int indexArrow;
    private int indexEquiped;
    private int priceArrow;
    private ArrowSO arrowScriptableObjectChosen;
    // State Purcchase button
    public enum StateEquipment { onlyPurchase, equiped, notYet}
    public StateEquipment[] stateIndext;
    public Vector3 OffsetArrow { get => offsetArrow; set => offsetArrow = value; }
    // Load weapon to UI
    public ArrowSO[] ArrowList { get => arrowList; set => arrowList = value; }
    public List<int> IndexEquipedList { get => indexEquipedList; set => indexEquipedList = value; }
    public int IndexArrow { get => indexArrow; set => indexArrow = value; }
    public int IndexEquiped { get => indexEquiped; set => indexEquiped = value; }
    public int PriceArrow { get => priceArrow; set => priceArrow = value; }
    public ArrowSO ArrowScriptableObjectChosen { get => arrowScriptableObjectChosen; set => arrowScriptableObjectChosen = value; }
    public StateEquipment[] StateIndext { get => stateIndext; set => stateIndext = value; }
    private void Start()
    {
        InitializeVariables();
    }
    public void Exit()
    {
        UIManager.Instance.CloseUI(UIName.WeaponChose);
        UIManager.Instance.OpenUI(UIName.CenterBoot);
    }
    public void Before()
    {
        indexArrow -= 1;
        CheckStateShowUi();
    }
    public void Next()
    {
        indexArrow += 1;
        CheckStateShowUi();
    }
    
    public void CheckStateShowUi()
    {
        indexArrow = Mathf.Clamp(indexArrow, 0, arrowList.Length - 1);
        priceArrow = arrowList[indexArrow].priceArrow;
        iconArrow.sprite = arrowList[indexArrow].iconArrow;
        priceTxt.text = arrowList[indexArrow].priceArrow.ToString();
        //++
        switch (stateIndext[indexArrow])
        {
            case StateEquipment.notYet:
                SetPriceVisible();
                break;
            case StateEquipment.onlyPurchase:
                SetSelectVisible();
                break;
            case StateEquipment.equiped:
                SetEquipedVisible();
                break;
        }

    }
    //Set type UI visible
    public void SetPriceVisible()
    {
        priceTxt.gameObject.SetActive(true);
        goldImage.gameObject.SetActive(true);
        equipedTxt.gameObject.SetActive(false);
        selectTxt.gameObject.SetActive(false);
        lockTxt.gameObject.SetActive(true);
    }
    public void SetEquipedVisible()
    {
        priceTxt.gameObject.SetActive(false);
        goldImage.gameObject.SetActive(false);
        equipedTxt.gameObject.SetActive(true);
        selectTxt.gameObject.SetActive(false);
        lockTxt.gameObject.SetActive(false);
    }
    public void SetSelectVisible()
    {
        priceTxt.gameObject.SetActive(false);
        goldImage.gameObject.SetActive(false);
        equipedTxt.gameObject.SetActive(false);
        selectTxt.gameObject.SetActive(true);
        lockTxt.gameObject.SetActive(false);
    }
    public void SpawnSetInforArrow()
    {
        PlayerPrefs.SetInt(arrowScriptableObjectChosen.nameArrow, 1);// 0 = notYet, 1 = onlyPurchase, 2 = equiped
        GameObject arrow = (GameObject)Instantiate(ArrowScriptableObjectChosen.arrowPrefabs, playerMain.PointFire.position, playerMain.PointFire.rotation);
        playerMain.ArrowObject = arrow;
        arrow.GetComponent<Arrow>().enabled = false;
        arrow.GetComponent<RoteItself>().enabled = false;
        arrow.transform.SetParent(playerMain.PointFire);
        arrow.transform.localPosition += offsetArrow;
        arrow.transform.transform.Rotate(0, -90, 0, Space.Self);
        if(stateIndext[indexArrow] == StateEquipment.notYet && !isAdsClick)
        {
            playerMain.Gold -= ArrowScriptableObjectChosen.priceArrow;
        }
        playerMain.Playerso.arrowPrefabs = ArrowScriptableObjectChosen.arrowPrefabs;
    }
    public void Purchase()
    {
        if (playerMain.Gold >= ArrowScriptableObjectChosen.priceArrow)
        {
            DestroySpawnEqip();
        }
    }
    public void AdsCount()
    {
        isAdsClick = true;
        if (stateIndext[indexArrow] == StateEquipment.notYet)
        {
            DestroySpawnEqip();
            adsCountTxt.text = "1/1";
        }
        isAdsClick = false;
    }
    public void DeEquiped()
    {
        for (int i = 0; i < stateIndext.Length; i++)
        {
            if(i != indexArrow && stateIndext[i] == StateEquipment.equiped)
            {
                stateIndext[i] = StateEquipment.onlyPurchase;
                foreach (var item in arrowList)
                {
                    if (item.nameArrow != arrowScriptableObjectChosen.nameArrow && PlayerPrefs.GetInt(item.nameArrow) == 2)
                    {
                        PlayerPrefs.SetInt(item.nameArrow, 1);// 0 = notYet, 1 = onlyPurchase, 2 = equiped
                    }
                }
            }
        }
    }
    public void DestroySpawnEqip()
    {
        if (playerMain.PointFire.childCount > 0)
        {
            Destroy(playerMain.PointFire.GetChild(0).gameObject);
        }
        ArrowScriptableObjectChosen = arrowList[indexArrow];
        SpawnSetInforArrow();
        stateIndext[indexArrow] = StateEquipment.equiped;
        DeEquiped();
        PlayerPrefs.SetInt(arrowScriptableObjectChosen.nameArrow, 2);// 0 = notYet, 1 = onlyPurchase, 2 = equiped
        SetEquipedVisible();
    }
    public void EquipItemReward(ArrowSO _arrowSO)
    {
        playerMain = PlayerMain.Instance;
        stateIndext = new StateEquipment[arrowList.Length];
        if (playerMain.PointFire.childCount > 0)
        {
            Destroy(playerMain.PointFire.GetChild(0).gameObject);
        }
        for (int i = 0; i < arrowList.Length; i++)
        {
            if (PlayerPrefs.GetInt(arrowList[i].nameArrow) == 2)
            {
                PlayerPrefs.SetInt(arrowList[i].nameArrow, 1);
            }
        }
        PlayerPrefs.SetInt(_arrowSO.nameArrow, 2);
        playerMain.Playerso.arrowPrefabs = _arrowSO.arrowPrefabs;
        for (int i = 0; i < arrowList.Length; i++)
        {
            if (PlayerPrefs.GetInt(arrowList[i].nameArrow) == 2)// 0 = notYet, 1 = onlyPurchase, 2 = equiped
            {
                GameObject arrow = (GameObject)Instantiate(_arrowSO.arrowPrefabs, playerMain.PointFire, false);
                arrow.GetComponent<Arrow>().enabled = false;
                arrow.GetComponent<RoteItself>().enabled = false;
                Vector3 offsetArrow = new Vector3(-0.23f, 0, 0);
                arrow.transform.localPosition += offsetArrow;
                arrow.transform.transform.Rotate(0, -90, 0, Space.Self);
            }
        }
    }
    public void EquipItemStartGame()
    {
        playerMain = PlayerMain.Instance;
        stateIndext = new StateEquipment[arrowList.Length];
        if (playerMain.PointFire.childCount > 0)
        {
            Destroy(playerMain.PointFire.GetChild(0).gameObject);
        }
        int countArrowHave = 0;
        for (int i = 0; i < arrowList.Length; i++)
        {
            if (PlayerPrefs.GetInt(arrowList[i].nameArrow) == 2)// 0 = notYet, 1 = onlyPurchase, 2 = equiped
            {
                playerMain.Playerso.arrowPrefabs = arrowList[i].arrowPrefabs;
                GameObject arrow =(GameObject) Instantiate(arrowList[i].arrowPrefabs, playerMain.PointFire, false);
                arrow.GetComponent<Arrow>().enabled = false;
                arrow.GetComponent<RoteItself>().enabled = false;
                Vector3 offsetArrow = new Vector3(-0.23f, 0, 0);
                arrow.transform.localPosition += offsetArrow;
                arrow.transform.transform.Rotate(0, -90, 0, Space.Self);
                countArrowHave++;
            }
        }
        if (countArrowHave == 0)
        {
            playerMain.Playerso.arrowPrefabs = arrowList[arrowList.Length - 1].arrowPrefabs;
            GameObject arrow = (GameObject)Instantiate(arrowList[arrowList.Length - 1].arrowPrefabs, playerMain.PointFire, false);// default last weapon
            arrow.GetComponent<Arrow>().enabled = false;
            arrow.GetComponent<RoteItself>().enabled = false;
            Vector3 offsetArrow = new Vector3(-0.23f, 0, 0);
            arrow.transform.localPosition += offsetArrow;
            arrow.transform.transform.Rotate(0, -90, 0, Space.Self);
        }
    }
    private void OnEnable()
    {
        //InitializeVariables();
    }
    public void InitializeArrowDefault()
    {
        if (PlayerPrefs.GetInt(arrowList[arrowList.Length - 1].nameArrow, 0) == 0)
        {
            PlayerPrefs.SetInt(arrowList[arrowList.Length - 1].nameArrow, 1);
        }
    }
    public void InitializeVariables()
    {
        playerMain = PlayerMain.Instance;
        #region Comment to reset  all Arrow to Not Yet to Test Game
            //for (int i = 0; i < arrowList.Length; i++)//Reset all Arrow to Not Yet to Test Game, only turn this canvas in Hierarchy
            //{
            //    PlayerPrefs.SetInt(arrowList[i].nameArrow, 0);// 0 = notYet, 1 = onlyPurchase, 2 = equiped
            //}
        #endregion
        indexArrow = 0;
        exitBtn.onClick.AddListener(Exit);
        beforeBtn.onClick.AddListener(Before);
        nextBtn.onClick.AddListener(Next);
        purchaseBtn.onClick.AddListener(Purchase);
        adsBtn.onClick.AddListener(AdsCount);
        ArrowScriptableObjectChosen = arrowList[0];
        offsetArrow = new Vector3(-0.23f, 0, 0);
        priceArrow = arrowList[0].priceArrow;
        priceTxt.text = arrowList[0].priceArrow.ToString();
        iconArrow.sprite = arrowList[0].iconArrow;
        stateIndext = new StateEquipment[arrowList.Length];
        for (int i = 0; i < arrowList.Length; i++)
        {
            if (PlayerPrefs.GetInt(arrowList[i].nameArrow) == 1)// 0 = notYet, 1 = onlyPurchase, 2 = equiped
            {
                stateIndext[i] = StateEquipment.onlyPurchase;
            }
            else if (PlayerPrefs.GetInt(arrowList[i].nameArrow) == 2)// 0 = notYet, 1 = onlyPurchase, 2 = equiped
            {
                stateIndext[i] = StateEquipment.equiped;
            }
            else
            {
                stateIndext[i] = StateEquipment.notYet;
            }
        }
        isAdsClick = false;
        //
        nameUI = UIName.SkinShop;
        CheckStateShowUi();
    }
}
