using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class AddExp : MonoBehaviour, IInitializeVariables
{
    private float speedGo;
    private TextMeshProUGUI txtExp;
    private Transform canvasExpTrans;
    public TextMeshProUGUI TxtExp { get => txtExp; set => txtExp = value; }
    public Transform CanvasExpTrans { get => canvasExpTrans; set => canvasExpTrans = value; }
    private float t;
    
    private void Awake()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speedGo);
        if((Time.time - t) > 1)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        InitializeVariables();
    }

    public void InitializeVariables()
    {
        speedGo = 2;
        //Canvas Exp
        txtExp = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        canvasExpTrans = gameObject.GetComponentInChildren<Transform>();
        t = Time.time;
    }
}
