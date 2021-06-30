using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DoublePressKeyDetection
{

    public KeyCode Key { get; private set; }

    public bool NonePressed { get; private set; }

    /// <summary> 한 번 눌러서 유지한 상태 </summary>
    public bool SinglePressed { get; private set; }

    /// <summary> 두 번 눌러서 유지한 상태 </summary>
    public bool DoublePressed { get; private set; }

    private bool doublePressDetected;
    private float doublePressThreshold;
    private float lastKeyDownTime;

    //Contructor
    public DoublePressKeyDetection(KeyCode key, float threshold = 0.3f)
    {
        this.Key = key;
        NonePressed = true;
        SinglePressed = false;
        DoublePressed = false;
        doublePressDetected = false;
        doublePressThreshold = threshold;
        lastKeyDownTime = 0f;
    }

    public void ChangeKey(KeyCode key)
    {
        this.Key = key;
    }
    public void ChangeThreshold(float seconds)
    {
        doublePressThreshold = seconds > 0f ? seconds : 0f;
    }

    /// <summary> MonoBehaviour.Update()에서 호출 : 키 정보 업데이트 </summary>
    public void UpdateCheck()
    {
        if (Input.GetKeyDown(Key))
        {
            NonePressed = false;

            doublePressDetected =
                (Time.time - lastKeyDownTime < doublePressThreshold);

            lastKeyDownTime = Time.time;
        }

        if (Input.GetKey(Key))
        {
            NonePressed = false;

            if (doublePressDetected)
                DoublePressed = true;
            else
                SinglePressed = true;
        }
        else
        {
            doublePressDetected = false;
            DoublePressed = false;
            SinglePressed = false;
            NonePressed = true;
        }
    }

    /// <summary> MonoBehaviour.Update()에서 호출 : 키 입력에 따른 동작 </summary>
    public void UpdateAction(Action singlePressAction, Action doublePressAction)
    {
        if (SinglePressed) singlePressAction?.Invoke();
        if (DoublePressed) doublePressAction?.Invoke();  
    }
}
