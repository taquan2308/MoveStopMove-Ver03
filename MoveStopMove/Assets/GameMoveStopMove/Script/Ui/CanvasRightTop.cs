using System;
using TMPro;
using UnityEngine;

public class CanvasRightTop : UICanvas, IInitializeVariables
{
    private PlayerMain playerMain;
    private int goldPlayer;
    [SerializeField] private TextMeshProUGUI goldPlayerTxt;
    public int GoldPlayer { get => goldPlayer; set => goldPlayer = value; }
    private void OnEnable()
    {
        InitializeVariables();
    }
    private void Update()
    {
        goldPlayer = playerMain.Gold;//
        goldPlayerTxt.text = goldPlayer.ToString();
    }
    public void InitializeVariables()
    {
        nameUI = UIName.RightTop;
        playerMain = PlayerMain.Instance;
    }

}
