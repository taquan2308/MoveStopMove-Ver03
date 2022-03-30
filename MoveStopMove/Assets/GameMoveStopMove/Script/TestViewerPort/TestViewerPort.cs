using UnityEngine;

public class TestViewerPort : MonoBehaviour
{
    public Transform target;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewPos = cam.WorldToViewportPoint(target.position);
        Debug.Log(viewPos);
        //if (viewPos.x > 0.5F)
        //    print("target is on the right side!");
        //else
        //    print("target is on the left side!");
    }

}
