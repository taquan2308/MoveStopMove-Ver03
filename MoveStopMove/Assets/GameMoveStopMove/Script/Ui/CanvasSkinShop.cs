using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasSkinShop : UICanvas, IInitializeVariables
{
    //Button
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button hornBtn;
    [SerializeField] private Button shortBtn;
    [SerializeField] private Button armBtn;
    [SerializeField] private Button skinBtn;
    //
    [SerializeField] private HornSO[] arrayHornSO;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject itemOfContentPrefab;
    [SerializeField] private Button purchaseBtn;
    [SerializeField] private Button adsBtn;
    //Price
    [SerializeField] private TextMeshProUGUI priceTxt;
    [SerializeField] private Image goldImage;
    [SerializeField] private TextMeshProUGUI adsCountTxt;
    //
    [SerializeField] private TextMeshProUGUI equipedTxt;
    [SerializeField] private TextMeshProUGUI selectTxt;
    private GameObject lockImage;
    [SerializeField] private Material defaultMaterial;
    private bool isAdsClick;
    //UI
    [HideInInspector] public PlayerMain playerMain;
    // Load weapon to UI
    private List<int> indexEquipedList;
    private int indexHorn;
    [HideInInspector] public int indexEquiped;
    [HideInInspector] public int priceHorn;
    [HideInInspector] public HornSO hornScriptableObjectChosen;
    // State Purcchase button
    private enum StateEquipment { onlyPurchase, equiped, notYet }
    private StateEquipment[] stateIndext;
    //Color Button
    private Color choseColor;
    private Color normalColor;
    private Button[] arrButtonGroup;
    [SerializeField] private Image[] arrImageButtonGroup;
    private List<Button> listBtnItem;
    //
    public List<Button> ListBtnItem { get => listBtnItem; set => listBtnItem = value; }
    public int IndexHorn { get => indexHorn; set => indexHorn = value; }
    public GameObject LockImage { get => lockImage; set => lockImage = value; }
    public HornSO[] ArrayHornSO { get => arrayHornSO; set => arrayHornSO = value; }
    void OnEnable()
    {
        InitializeVariables();
        Horn();
    }
    public void Exit()
    {
        UIManager.Instance.OpenUI(UIName.CenterBoot);
        gameObject.SetActive(false);
        CameraManager.Instance.MainCameraTrans.gameObject.SetActive(true);
        CameraManager.Instance.Sub01CameraTrans.gameObject.SetActive(false);
    }
    public void SetColorBtn(Button _nameBtn)
    {
        for (int i = 0; i < arrButtonGroup.Length; i++)
        {
            if(arrButtonGroup[i] != _nameBtn)
            {
                arrImageButtonGroup[i].color = normalColor;
            }
            else
            {
                arrImageButtonGroup[i].color = choseColor;
            }
        }
    }
    public void Horn()
    {
        SetColorBtn(hornBtn);
        LoadItemOnGroup("Horn");
    }
    public void Short()
    {
        SetColorBtn(shortBtn);
        LoadItemOnGroup("Short");
    }
    public void Arm()
    {
        SetColorBtn(armBtn);
        LoadItemOnGroup("Arm");
    }
    public void Skin()
    {
        SetColorBtn(skinBtn);
        LoadItemOnGroup("Skin");
    }
    public void LoadItemOnGroup(string _nameGroup)
    {
        listBtnItem.Clear();
        if (content.childCount > 0)//
        {
            RectTransform[] items = content.gameObject.GetComponentsInChildren<RectTransform>();
            //Ignor first component of Content 
            for (int i = 1; i < items.Length; i++)
            {
                Destroy(items[i].gameObject);
            }
        }
        bool isFirstItemInGroup = true;
        
        for (int i = 0; i < arrayHornSO.Length; i++)
        {
            
            GameObject item = Instantiate(itemOfContentPrefab, content, false);
            item.GetComponentsInChildren<Image>()[1].sprite = arrayHornSO[i].iconHorn;
            item.GetComponent<Item>().HornSOThisItem = arrayHornSO[i];
            item.GetComponent<Item>().IndexHorn = i;
            //check state because order process start and OnEnable,
            //unlock
            if (stateIndext.Length > 0)
            {
                if (stateIndext[i] == StateEquipment.equiped || stateIndext[i] == StateEquipment.onlyPurchase)
                {
                    item.GetComponent<Item>().LockImage.SetActive(false);
                }
            }
            
            //destroy button of other group
            listBtnItem.Add(item.GetComponent<Button>());
            if (arrayHornSO[i].nameGroup != _nameGroup)
            {
                Destroy(item);
            }else if (isFirstItemInGroup)
            {
                isFirstItemInGroup = false;
                item.GetComponent<Item>().ClickBtn();

                var colors1 = item.GetComponent<Button>().colors;
                colors1.normalColor = choseColor;
                item.GetComponent<Button>().colors = colors1;
            }

        }
    }
    public void SetPriceTxt(int _price)
    {
        priceHorn = _price;
        priceTxt.text = priceHorn.ToString();
    }
    
    public void CheckStateShowUi()
    {
        if (hornScriptableObjectChosen != null)
        {
            priceHorn = hornScriptableObjectChosen.priceHorn;
            priceTxt.text = priceHorn.ToString();
            switch (stateIndext[indexHorn])
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
    }
    public void AdsCount()
    {
        if (stateIndext[indexHorn] == StateEquipment.notYet && !isAdsClick)
        {
            DestroySpawnEqip();
            adsCountTxt.text = "1/1";
            lockImage.SetActive(false);
        }
        isAdsClick = true;
    }
    public void Purchase()
    {
        if (playerMain.Gold >= hornScriptableObjectChosen.priceHorn)
        {
            DestroySpawnEqip();
            lockImage.SetActive(false);
        }
    }
    public void DestroySpawnEqip()
    {
        SpawnSetInforHorn();
        stateIndext[indexHorn] = StateEquipment.equiped;
        DeEquiped();
        PlayerPrefs.SetInt(hornScriptableObjectChosen.nameItem, 2);// 0 = notYet, 1 = onlyPurchase, 2 = equiped
        SetEquipedVisible();
    }
  

    public void SpawnSetInforHorn()
    {
        PlayerPrefs.SetInt(hornScriptableObjectChosen.nameItem, 1);
        //Horn
        if(hornScriptableObjectChosen.nameGroup == "Horn")
        {
            if (PlayerPrefs.GetInt("stateFullSet", 0) == 1)
            {
                RemoveSetSkin();
            }
            DeleteChildOfParent(playerMain.HeadTras);
            DeleteChildOfParent(playerMain.BladeWearTras);
            DeleteChildOfParent(playerMain.BeardTras);

            if (hornScriptableObjectChosen.prefabsHead != null)
            {
                Instantiate(hornScriptableObjectChosen.prefabsHead, playerMain.HeadTras, false);
            }
            if(hornScriptableObjectChosen.prefabsBeard != null)
            {
                Instantiate(hornScriptableObjectChosen.prefabsBeard, playerMain.BeardTras, false);
            }
            if (stateIndext[indexHorn] == StateEquipment.notYet && !isAdsClick)
            {
                playerMain.Gold -= hornScriptableObjectChosen.priceHorn;
            }
        }
        //Short
        if(hornScriptableObjectChosen.nameGroup == "Short")
        {
            if (PlayerPrefs.GetInt("stateFullSet", 0) == 1)
            {
                RemoveSetSkin();
            }
            //set material
            var mats = new Material[playerMain.MaterialPantWears.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = playerMain.MaterialPantWears[i];
            }
            mats[0] = hornScriptableObjectChosen.materialPan;
            //Must call player.materialGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats, ( do not change if set : player.materialWears = mats;)
            playerMain.MaterialPantGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats;
            //
            if (stateIndext[indexHorn] == StateEquipment.notYet && !isAdsClick)
            {
                playerMain.Gold -= hornScriptableObjectChosen.priceHorn;
            }
        }
        //Arm
        if(hornScriptableObjectChosen.nameGroup == "Arm")
        {
            if (PlayerPrefs.GetInt("stateFullSet",0) == 1)
            {
                RemoveSetSkin();
            }
            DeleteChildOfParent(playerMain.ShieldWearTras);
            PlayerPrefs.SetInt(hornScriptableObjectChosen.nameItem, 2);// 0 = notYet, 1 = onlyPurchase, 2 = equiped
            Instantiate(hornScriptableObjectChosen.prefabsShield, playerMain.ShieldWearTras, false);
            if (stateIndext[indexHorn] == StateEquipment.notYet && !isAdsClick)
            {
                playerMain.Gold -= hornScriptableObjectChosen.priceHorn;
            }
        }
        //Skin
        if(hornScriptableObjectChosen.nameGroup == "Skin")
        {
            PlayerPrefs.SetInt("stateFullSet", 1);
            DeleteChildOfParent(playerMain.HeadTras);
            DeleteChildOfParent(playerMain.BladeWearTras);
            DeleteChildOfParent(playerMain.BeardTras);
            DeleteChildOfParent(playerMain.TailWearTras);
            DeleteChildOfParent(playerMain.ShieldWearTras);
            if(hornScriptableObjectChosen.materialFullSet != null)
            {
                var mats = new Material[playerMain.MaterialSkinWears.Length];
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i] = playerMain.MaterialSkinWears[i];
                }
                mats[0] = hornScriptableObjectChosen.materialFullSet;
                //Must call player.materialGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats, ( do not change if set : player.materialWears = mats;)
                playerMain.MaterialSkinGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats;
            }
            if(hornScriptableObjectChosen.prefabsWing != null)
            {
                    
                Instantiate(hornScriptableObjectChosen.prefabsWing, playerMain.BladeWearTras, false);
            }
            if (hornScriptableObjectChosen.prefabsHead != null)
            {
                Instantiate(hornScriptableObjectChosen.prefabsHead, playerMain.HeadTras, false);
            }
            if (hornScriptableObjectChosen.prefabsTail != null)
            {
                Instantiate(hornScriptableObjectChosen.prefabsTail, playerMain.TailWearTras, false);
            }
            if (hornScriptableObjectChosen.prefabsShield != null)
            {
                Instantiate(hornScriptableObjectChosen.prefabsShield, playerMain.ShieldWearTras, false);
            }
            //
            if (stateIndext[indexHorn] == StateEquipment.notYet && !isAdsClick)
            {
                playerMain.Gold -= hornScriptableObjectChosen.priceHorn;
            }
        }
    }
    public void DeEquiped()
    {
        for (int i = 0; i < stateIndext.Length; i++)
        {
            if (i != indexHorn && stateIndext[i] == StateEquipment.equiped)
            {
                stateIndext[i] = StateEquipment.onlyPurchase;
                foreach (var item in arrayHornSO)
                {
                    if (item.nameItem != hornScriptableObjectChosen.nameItem && PlayerPrefs.GetInt(item.nameItem) == 2)
                    {
                        PlayerPrefs.SetInt(item.nameItem, 1);// 0 = notYet, 1 = onlyPurchase, 2 = equiped
                    }
                }
            }
        }
    }
    public void EquipItemReward(HornSO _hornSOReward)
    {
        playerMain = PlayerMain.Instance;
        stateIndext = new StateEquipment[arrayHornSO.Length];
        RemoveSetSkin();
        DeleteChildOfParent(playerMain.BeardTras);
        PlayerPrefs.SetInt(_hornSOReward.nameItem, 2);
        for (int i = 0; i < arrayHornSO.Length; i++)
        {
            if (arrayHornSO[i].nameItem != _hornSOReward.nameItem && PlayerPrefs.GetInt(arrayHornSO[i].nameItem) == 2)
            {
                PlayerPrefs.SetInt(arrayHornSO[i].nameItem, 1);// 0 = notYet, 1 = onlyPurchase, 2 = equiped
            }
        }
        if (_hornSOReward.prefabsArrow != null)
        {
            Instantiate(_hornSOReward.prefabsArrow, playerMain.PointFire, false);
        }
        if (_hornSOReward.prefabsHead != null)
        {
            Instantiate(_hornSOReward.prefabsHead, playerMain.HeadTras, false);
        }
        if (_hornSOReward.prefabsBeard != null)
        {
            Instantiate(_hornSOReward.prefabsBeard, playerMain.BeardTras, false);
        }
        if (_hornSOReward.prefabsPan != null)
        {
            Instantiate(_hornSOReward.prefabsPan, playerMain.UnderWearTras, false);
        }
        if (_hornSOReward.prefabsShield != null)
        {
            Instantiate(_hornSOReward.prefabsShield, playerMain.ShieldWearTras, false);
        }
        if (_hornSOReward.prefabsTail != null)
        {
            Instantiate(_hornSOReward.prefabsTail, playerMain.TailWearTras, false);
        }
        if (_hornSOReward.prefabsWing != null)
        {
            Instantiate(_hornSOReward.prefabsWing, playerMain.BladeWearTras, false);
        }
        if (_hornSOReward.materialFullSet != null)
        {
            var mats = new Material[playerMain.MaterialSkinWears.Length];
            for (int j = 0; j < mats.Length; j++)
            {
                mats[j] = playerMain.MaterialSkinWears[j];
            }
            mats[0] = _hornSOReward.materialFullSet;
            //Must call player.materialGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats, ( do not change if set : player.materialWears = mats;)
            playerMain.MaterialSkinGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats;
        }
        if (_hornSOReward.materialPan != null)
        {
            var mats = new Material[playerMain.MaterialSkinWears.Length];
            for (int j = 0; j < mats.Length; j++)
            {
                mats[j] = playerMain.MaterialSkinWears[j];
            }
            mats[0] = _hornSOReward.materialPan;
            //Must call player.materialGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats, ( do not change if set : player.materialWears = mats;)
            playerMain.MaterialPantGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats;
        }
    }
    public void EquipItemStartGame()
    {
        playerMain = PlayerMain.Instance;
        stateIndext = new StateEquipment[arrayHornSO.Length];
        DeleteChildOfParent(playerMain.HeadTras);
        DeleteChildOfParent(playerMain.BeardTras);
        DeleteChildOfParent(playerMain.UnderWearTras);
        DeleteChildOfParent(playerMain.BladeWearTras);
        DeleteChildOfParent(playerMain.ShieldWearTras);
        DeleteChildOfParent(playerMain.TailWearTras);
        for (int i = 0; i < arrayHornSO.Length; i++)
        {
            if (PlayerPrefs.GetInt(arrayHornSO[i].nameItem) == 2)// 0 = notYet, 1 = onlyPurchase, 2 = equiped
            {
                if (arrayHornSO[i].prefabsArrow != null)
                {
                    Instantiate(arrayHornSO[i].prefabsArrow, playerMain.PointFire, false);
                }
                if (arrayHornSO[i].prefabsHead != null)
                {
                    Instantiate(arrayHornSO[i].prefabsHead, playerMain.HeadTras, false);
                }
                if (arrayHornSO[i].prefabsBeard != null)
                {
                    Instantiate(arrayHornSO[i].prefabsBeard, playerMain.BeardTras, false);
                }
                if (arrayHornSO[i].prefabsPan != null)
                {
                    Instantiate(arrayHornSO[i].prefabsPan, playerMain.UnderWearTras, false);
                }
                if (arrayHornSO[i].prefabsShield != null)
                {
                    Instantiate(arrayHornSO[i].prefabsShield, playerMain.ShieldWearTras, false);
                }
                if (arrayHornSO[i].prefabsTail != null)
                {
                    Instantiate(arrayHornSO[i].prefabsTail, playerMain.TailWearTras, false);
                }
                if (arrayHornSO[i].prefabsWing != null)
                {
                    Instantiate(arrayHornSO[i].prefabsWing, playerMain.BladeWearTras, false);
                }
                if (arrayHornSO[i].materialFullSet != null)
                {
                    var mats = new Material[playerMain.MaterialSkinWears.Length];
                    for (int j = 0; j < mats.Length; j++)
                    {
                        mats[j] = playerMain.MaterialSkinWears[j];
                    }
                    mats[0] = arrayHornSO[i].materialFullSet;
                    //Must call player.materialGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats, ( do not change if set : player.materialWears = mats;)
                    playerMain.MaterialSkinGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats;
                }
                if (arrayHornSO[i].materialPan != null)
                {
                    var mats = new Material[playerMain.MaterialSkinWears.Length];
                    for (int j = 0; j < mats.Length; j++)
                    {
                        mats[j] = playerMain.MaterialSkinWears[j];
                    }
                    mats[0] = arrayHornSO[i].materialPan;
                    //Must call player.materialGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats, ( do not change if set : player.materialWears = mats;)
                    playerMain.MaterialPantGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats;
                }
            }
        }
    }
    //Set type UI visible
    public void SetPriceVisible()
    {
        priceTxt.gameObject.SetActive(true);
        goldImage.gameObject.SetActive(true);
        equipedTxt.gameObject.SetActive(false);
        selectTxt.gameObject.SetActive(false);
    }
    public void SetEquipedVisible()
    {
        priceTxt.gameObject.SetActive(false);
        goldImage.gameObject.SetActive(false);
        equipedTxt.gameObject.SetActive(true);
        selectTxt.gameObject.SetActive(false);
    }
    public void SetSelectVisible()
    {
        priceTxt.gameObject.SetActive(false);
        goldImage.gameObject.SetActive(false);
        equipedTxt.gameObject.SetActive(false);
        selectTxt.gameObject.SetActive(true);
    }
    //
    public void DeleteChildOfParent(Transform _transformParentToFormat)
    {
        if (_transformParentToFormat.childCount > 0)
        {
            Transform[] items = _transformParentToFormat.gameObject.GetComponentsInChildren<Transform>();
            //Ignor first component of Content
            for (int i = 1; i < items.Length; i++)
            {
                Destroy(items[i].gameObject);
            }
        }
    }
    public void SetDefaultMaterialSkin()
    {
        var mats = new Material[playerMain.MaterialSkinWears.Length];
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = playerMain.MaterialSkinWears[i];
        }
        mats[0] = defaultMaterial;
        //Must call player.materialGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats, ( do not change if set : player.materialWears = mats;)
        playerMain.MaterialSkinGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats;
    }
    public void SetDefaultMaterialPant()
    {
        var mats = new Material[playerMain.MaterialPantWears.Length];
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = playerMain.MaterialPantWears[i];
        }
        mats[0] = defaultMaterial;
        //Must call player.materialGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats, ( do not change if set : player.materialWears = mats;)
        playerMain.MaterialPantGameObject.GetComponent<SkinnedMeshRenderer>().materials = mats;
    }
    public void RemoveSetSkin()
    {
        SetDefaultMaterialSkin();
        SetDefaultMaterialPant();
        DeleteChildOfParent(playerMain.HeadTras);
        DeleteChildOfParent(playerMain.UnderWearTras);
        DeleteChildOfParent(playerMain.BladeWearTras);
        DeleteChildOfParent(playerMain.ShieldWearTras);
        DeleteChildOfParent(playerMain.TailWearTras);
        for (int i = 0; i < arrayHornSO.Length; i++)
        {
            if (arrayHornSO[i].nameGroup == "Skin" && PlayerPrefs.GetInt(arrayHornSO[i].nameItem) == 2)
            {
                PlayerPrefs.SetInt(arrayHornSO[i].nameItem, 1);// 0 = notYet, 1 = onlyPurchase, 2 = equiped
            }
        }
        PlayerPrefs.SetInt("stateFullSet", 0);
    }
    public void InitializeVariables()
    {
        nameUI = UIName.SkinShop;
        stateIndext = new StateEquipment[arrayHornSO.Length];
        #region Comment to reset  all Arrow to Not Yet to Test Game
            //for (int i = 0; i < arrayHornSO.Length; i++)//Reset all Horn to Not Yet to Test Game, only turn this canvas in Hierarchy
            //{
            //    PlayerPrefs.SetInt(arrayHornSO[i].nameItem, 0);// 0 = notYet, 1 = onlyPurchase, 2 = equiped

            //}
        #endregion
        for (int i = 0; i < arrayHornSO.Length; i++)
        {
            if (PlayerPrefs.GetInt(arrayHornSO[i].nameItem) == 1)// 0 = notYet, 1 = onlyPurchase, 2 = equiped
            {
                stateIndext[i] = StateEquipment.onlyPurchase;
            }
            else if (PlayerPrefs.GetInt(arrayHornSO[i].nameItem) == 2)// 0 = notYet, 1 = onlyPurchase, 2 = equiped
            {
                stateIndext[i] = StateEquipment.equiped;
            }
            else
            {
                stateIndext[i] = StateEquipment.notYet;
            }
        }
        listBtnItem = new List<Button>();
        choseColor = new Color(1f, 1f, 1f);
        normalColor = new Color(0.8f, 0.8f, 0.8f);
        //
        playerMain = PlayerMain.Instance;
        exitBtn.onClick.AddListener(Exit);
        hornBtn.onClick.AddListener(Horn);
        shortBtn.onClick.AddListener(Short);
        armBtn.onClick.AddListener(Arm);
        skinBtn.onClick.AddListener(Skin);
        purchaseBtn.onClick.AddListener(Purchase);
        adsBtn.onClick.AddListener(AdsCount);
        //
        isAdsClick = false;
        //
        hornScriptableObjectChosen = arrayHornSO[0];
        //
        priceHorn = arrayHornSO[0].priceHorn;
        priceTxt.text = arrayHornSO[0].priceHorn.ToString();
        //
        arrButtonGroup = new Button[] { hornBtn, shortBtn, armBtn, skinBtn };
        //
        nameUI = UIName.SkinShop;
    }
}


