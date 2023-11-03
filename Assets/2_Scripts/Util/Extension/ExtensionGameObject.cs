using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extension
{
    public static partial class Extension
    {

        public static void SetParentEx(this GameObject gameObject, Transform Parent)
        {
            if (gameObject == null)
                return;

            gameObject.transform.SetParent(Parent);
        }

        public static T GetComponentForce<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
                component = gameObject.AddComponent<T>();

            return component;
        }

        public static void SetActiveEx(this GameObject gameObject, bool IsActive)
        {
            if (gameObject == null)
                return;

            gameObject.SetActive(IsActive);
        }
    }
}

