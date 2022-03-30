using UnityEngine;

public class CameraControll : MonoBehaviour, IInitializeVariables
{
    private Vector3 cameraPosOffset;
    [SerializeField] private PlayerMain playerMain;
    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (playerMain != null)
        {
            CameraManager.Instance.MainCameraTrans.position = playerMain.gameObject.transform.position + cameraPosOffset;
        }
    }
    public void InitializeVariables()
    {
        cameraPosOffset = new Vector3(15f, 15f, 0);
        playerMain = PlayerMain.Instance;
    }
}
