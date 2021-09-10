using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilHelper
{
    //T 타입은 컴포넌트이거나 컴포넌트를 상속받은 클래스만 지정할 수 있도록 한다.
    //특정 경로에 있는 컴포넌트를 찾을 때 사용하는 함수입니다.
    public static T Find<T>(Transform t, string path) where T : Component
    {
        Transform findObj = t.Find(path);

        if (findObj != null)
        {
            return findObj.GetComponent<T>();
        }
        else
        {
            Debug.LogWarning("Cannot Find Component : " + path);
            return null;
        }
    }

    public static bool HasComponent<T>(this GameObject obj) where T : Component
    {
        return obj.GetComponent<T>() != null;
    }

}
