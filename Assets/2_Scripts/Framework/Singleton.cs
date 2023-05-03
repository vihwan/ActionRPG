using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : class
{
    private static Singleton<T> m_instance = null;
    private static object m_lock = new Object();

    public static Singleton<T> It
    {
        get 
        {
            // lock은 비싼 연산에 속하므로 Double-checked locking을 사용한다
            // 자세한 설명 : https://stackoverflow.com/questions/12316406/thread-safe-c-sharp-singleton-pattern

            lock (m_lock)
            {
                if (m_instance == null)
                {
                    m_instance = new Singleton<T>();
                }
            }

            return m_instance;
        }
    }

    public static bool hasInstance()
    {
        return m_instance != null;
    }
}
