using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    public static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();

                if (_instance == null)
                {
                    Debug.LogError($"[MonoSingleton] No instance of {typeof(T).Name} found in the scene.");
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning($"[MonoSingleton] Duplicate instance of {typeof(T).Name} found. Destroying this one.");
            Destroy(gameObject);
            return;
        }

        _instance = (T)this;
        DontDestroyOnLoad(gameObject); // Optional: Remove if not needed across scenes

        Init();
    }

    /// <summary>
    /// Any logic that needs to be completed at start-up (Awake)
    /// </summary>
    protected virtual void Init()
    {

    }
}
