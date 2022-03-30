using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Transform mainCameraTrans;
    [SerializeField] private Transform sub01CameraTrans;
    [SerializeField] private GameObject mainCameraGameObj;
    [SerializeField] private GameObject sub01CameraGameObj;
    public Transform MainCameraTrans { get => mainCameraTrans;}
    public Transform Sub01CameraTrans { get => sub01CameraTrans;}
    public GameObject MainCameraGameObj { get => mainCameraGameObj; }
    public GameObject Sub01CameraGameObj { get => sub01CameraGameObj; }
}
