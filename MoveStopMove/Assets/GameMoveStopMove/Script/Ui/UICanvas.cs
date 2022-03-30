using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICanvas : MonoBehaviour
{
    public UIName nameUI;
    
    public virtual void OnInit()
    {
        
    }
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
