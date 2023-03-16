using UnityEngine;

public abstract class Singleton<T> where T : new()
{
    private static T instance;
    private static object mutex = new object();

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (mutex)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
            }
            return instance;
        }
    }
}

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool global = true;
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = (T)obj.AddComponent(typeof(T));
                    obj.hideFlags = HideFlags.DontSave;
                }
            }
            return instance;
        }

    }

    void Start()
    {
        if (global) DontDestroyOnLoad(this.gameObject);
        this.OnStart();
    }

    protected virtual void OnStart()
    {

    }
}