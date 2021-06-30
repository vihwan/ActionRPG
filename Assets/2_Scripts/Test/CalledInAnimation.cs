using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalledInAnimation : MonoBehaviour
{
    public void CallFunc(string functionName)
    {
        transform.parent.SendMessage(functionName, SendMessageOptions.DontRequireReceiver);
    }
}
