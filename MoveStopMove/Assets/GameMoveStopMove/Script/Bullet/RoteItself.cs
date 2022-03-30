using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoteItself : MonoBehaviour, IInitializeVariables
{
    private Arrow arrow2;
    private bool isRoteArrow;
    private Transform objectRoteTrans;
    private int speedRote;
    public Arrow Arrow2 { get => arrow2; set => arrow2 = value; }
    public bool IsRoteArrow { get => isRoteArrow; set => isRoteArrow = value; }
    public Transform ObjectRoteTrans { get => objectRoteTrans; set => objectRoteTrans = value; }
    public int SpeedRote { get => speedRote; set => speedRote = value; }
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRoteArrow)
        {
            RoteAround();
        }
    }
    public void RoteAround()
    {
        objectRoteTrans.RotateAround(objectRoteTrans.position, Vector3.up, speedRote * Time.deltaTime);
    }

    public void InitializeVariables()
    {
        arrow2 = GetComponent<Arrow>();
        isRoteArrow = arrow2.ArrowSO2.isRoteArrow;
        speedRote = arrow2.ArrowSO2.speedRote;
        objectRoteTrans = gameObject.GetComponentInChildren<Transform>();
    }
}
