using System.Collections;
using UnityEngine;

public class SetLevel1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        PlayerPrefs.SetInt("CurrentLevel", 1);
    }
}
