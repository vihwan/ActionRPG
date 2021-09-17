using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class ManaBar : MonoBehaviour
    {
        private Slider slider;
        [SerializeField] private TMP_Text manaText;
        public void Init()
        {
            slider = GetComponent<Slider>();
            if (slider == null)
                Debug.Log("Slider Null");

            manaText = GetComponentInChildren<TMP_Text>(true);
        }

        public void SetMaxMana(int maxMana)
        {
            slider.maxValue = maxMana;
            slider.value = maxMana;
        }

        public void SetCurrentMana(int currentMana)
        {
            slider.value = currentMana;
        }

        public void SetManaText(int currentMana, int maxMana)
        {
            manaText.text = string.Format("{0} / {1}", currentMana, maxMana);
        }
    }
}