using UnityEngine;
using UnityEngine.UI;
public class CanvasLive : UICanvas, IInitializeVariables
{
    private void OnEnable()
    {
        InitializeVariables();
    }
    public void InitializeVariables()
    {
        //
        nameUI = UIName.Live;
    }
}
