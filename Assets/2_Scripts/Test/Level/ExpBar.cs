using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class ExpBar : MonoBehaviour
    {
        public Slider slider;
        public TMP_Text expText;
        public void Init()
        {
            slider = GetComponent<Slider>();
            if (slider == null)
                Debug.Log("Slider Null");

            expText = GetComponentInChildren<TMP_Text>(true);
        }
    }
}
