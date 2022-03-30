using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    string ten;
    enum listString { a,b,c,d}
    void Start()
    {
        ten = checkString(listString.a);// ten = "aaa"
    }
    string checkString(listString _enumString)
    {
        switch (_enumString)
        {
            case listString.a:
                ten = "aaa";
                break;
            case listString.b:
                ten = "bbb";
                break;
            case listString.c:
                ten = "ccc";
                break;
            case listString.d:
                ten = "ddd";
                break;
            default:
                break;
        }
        return ten;
    }
}
