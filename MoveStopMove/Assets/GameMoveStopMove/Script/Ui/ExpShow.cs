using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExpShow : MonoBehaviour,IInitializeVariables
{
    private PlayerMain playerMain;
    private Transform playerTrans;
    private EnemyMain enemyMain;
    private Transform enemyTrans;
    private Vector3 offsetExpCanvas;
    private TextMeshProUGUI txtCore;
    private bool isDeactive;
    public PlayerMain PlayerMain { get => playerMain; set => playerMain = value; }
    public EnemyMain EnemyMain { get => enemyMain; set => enemyMain = value; }
    public Transform PlayerTrans { get => playerTrans; set => playerTrans = value; }
    public Transform EnemyTrans { get => enemyTrans; set => enemyTrans = value; }
    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerMain != null)
        {
            txtCore.text = PlayerMain.Experience.ToString();
            transform.position = playerTrans.position + offsetExpCanvas;
        }
        else if (enemyMain != null)
        {
            txtCore.text = enemyMain.Experience.ToString();
        }
        if (isDeactive)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void InitializeVariables()
    {
        // y = 2.5 start
        offsetExpCanvas = new Vector3(0, 4, 0);
        isDeactive = false;
        txtCore = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }
}
