using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouseHoverSettingBtn : MonoBehaviour
{
    private void OnMouseEnter()
    {
        Debug.Log("fdsaffs");
        UIManager.Instance.CloseUI(UIName.Joystick);
    }
    private void OnMouseExit()
    {
        Debug.Log("fdsaffs");
        UIManager.Instance.OpenUI(UIName.Joystick);
    }
}
