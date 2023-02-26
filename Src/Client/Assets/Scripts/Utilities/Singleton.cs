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

public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
{
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

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
        OnAwake();
    }

    protected virtual void OnAwake()
    {

    }
    }