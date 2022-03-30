using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gaga : MonoBehaviour
{
    public RectTransform parent;
    public RectTransform image;
    public Camera cameradfd;
    //public Canvas canvas;
    //public Transform player;
    public Transform enemy;
    public float height;
    public float with;
    //
    //public Transform center;
    //public Transform pointTestCenter;
    public RectTransform pointTestCenterLook;
    // Start is called before the first frame update
    void Start()
    {
        height = 1920;
        with = 1080;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 anchoPos;
        Vector2 centerPos = new Vector2(with / 2, height / 2);
        Vector3 enemySeen;
        enemySeen = cameradfd.WorldToScreenPoint(enemy.position);
        Vector2 enemySeen2D = new Vector2(enemySeen.x, enemySeen.y);
        Vector2 enemychuanhoa = new Vector2(0,0);
        float a1 = (centerPos.x * enemySeen.y - centerPos.y * enemySeen.x);
        float a2 = 50;
        float a3 = with - 50;
        float a4 = height - 50;
        float a5 = enemySeen.x - centerPos.x;
        float a6 = enemySeen.y - centerPos.y;
        if (enemySeen.x >= 0 && enemySeen.x <= with && enemySeen.y >= 0 && enemySeen.y <= height)
        {
            image.gameObject.SetActive(false);
            return;
        }
        if (image.gameObject.activeSelf == false)
        {
            image.gameObject.SetActive(true);
        }
        Vector2 point11h = new Vector2(a2, a4);
        Vector2 point2h = new Vector2(a3, a4);
        Vector2 point5h = new Vector2(a3, a2);
        Vector2 point7h = new Vector2(a2, a2);
        Vector2 CenterTo11h = centerPos - point11h;
        Vector2 CenterTo2h = centerPos - point2h;
        Vector2 CenterTo5h = centerPos - point5h;
        Vector2 CenterTo7h = centerPos - point7h;
        Vector2 CenterToChuanHoa = centerPos - enemySeen2D;
        //
        int key = 1;
        if(Vector2.Angle(CenterToChuanHoa, CenterTo11h) <= Vector2.Angle(CenterTo11h, CenterTo2h) && Vector2.Angle(CenterToChuanHoa, CenterTo2h) <= Vector2.Angle(CenterTo11h, CenterTo2h))
        {
            key = 1;
        }else if (Vector2.Angle(CenterToChuanHoa, CenterTo2h) <= Vector2.Angle(CenterTo2h, CenterTo5h) && Vector2.Angle(CenterToChuanHoa, CenterTo5h) <= Vector2.Angle(CenterTo2h, CenterTo5h) && CenterToChuanHoa.x <0)
        {
            key = 2;
        }
        else if (Vector2.Angle(CenterToChuanHoa, CenterTo5h) <= Vector2.Angle(CenterTo5h, CenterTo7h) && Vector2.Angle(CenterToChuanHoa, CenterTo7h) <= Vector2.Angle(CenterTo5h, CenterTo7h))
        {
            key = 3;
        }
        else if (Vector2.Angle(CenterToChuanHoa, CenterTo7h) <= Vector2.Angle(CenterTo7h, CenterTo11h) && Vector2.Angle(CenterToChuanHoa, CenterTo11h) <= Vector2.Angle(CenterTo7h, CenterTo11h))
        {
            key = 4;
        }
        //
        if (key == 1)
        {
            enemychuanhoa = new Vector2((a1 + a4 * a5) / a6, a4);
        } else if (key == 2)
        {
            enemychuanhoa = new Vector2(a3, (-a1 + a3 * a6) / a5);
        } else if (key == 3)
        {
            enemychuanhoa = new Vector2((a1 + a2 * a5) / a6, a2);
        }
        else if (key == 4)
        {
            enemychuanhoa = new Vector2(a2, (-a1 + a2 * a6) / a5);
        }
        //
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, enemychuanhoa, cameradfd, out anchoPos);
        image.anchoredPosition = anchoPos;

        ////image.LookAt(centerPos);
        Vector3 dirCenterToIcon = pointTestCenterLook.position - image.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dirCenterToIcon);
        ////Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 0).eulerAngles;
        //image.transform.rotation = Quaternion.RotateTowards(image.transform.rotation, lookRotation, 20);
        image.transform.rotation = lookRotation;

        Debug.DrawLine(pointTestCenterLook.position, enemy.position);
    }
}
