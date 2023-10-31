using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
{
    private static T m_instance;
    private static bool m_shuttingDown = false;
    private static object m_lock = new object();
    public static T it
    {
        get
        {
            if (m_shuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed. Returning null.");
                return null;
            }

            lock (m_lock)
            {
                if (m_instance == null)
                {
                    // Search for existing instance.
                    m_instance = (T)FindObjectOfType(typeof(T));

                    // Create new instance if one doesn't already exist.
                    if (m_instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        m_instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        // Make instance persistent.
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return m_instance;
            }
        }
    }

    public static bool hasInstance()
    {
        return m_instance != null;
    }

    protected virtual void Init()
    {
        this.transform.SetParent(SG.GameManager.it.transform);
    }


    private void OnDestroy()
    {
        m_shuttingDown = true;
    }

    private void OnApplicationQuit()
    {
        m_shuttingDown = true;
    }
}