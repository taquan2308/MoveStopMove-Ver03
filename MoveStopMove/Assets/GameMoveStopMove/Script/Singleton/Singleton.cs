using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour

{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    _instance = new GameObject("[" + typeof(T).Name + "]").AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (this != Instance)
        {
            GameObject obj = this.gameObject;
            Destroy(this);
            Destroy(obj);
            return;
        }
    }

    public static bool HasInstance
    {
        get
        { 
            return Instance != null;
        }
    }
}
