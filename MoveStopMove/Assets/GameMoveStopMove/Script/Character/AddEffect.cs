using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEffect : MonoBehaviour
{
    public float t;
    private void Awake()
    {
        t = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        if ((Time.time - t) > 0.2f)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        t = Time.time;
    }
}
