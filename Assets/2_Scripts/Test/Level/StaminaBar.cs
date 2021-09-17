using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class StaminaBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text staminaText;
        public void Init()
        {
            slider = GetComponent<Slider>();
            if (slider == null)
                Debug.Log("Slider Null");
            
            staminaText = GetComponentInChildren<TMP_Text>(true);
        }

        public void SetMaxStamina(int maxStamina)
        {
            slider.maxValue = maxStamina;
            slider.value = maxStamina;
        }

        public void SetCurrentStamina(int currentStamian)
        {
            slider.value = currentStamian;
        }

        public void SetStaminaText(int currentStamina, int maxStamina)
        {
            staminaText.text = string.Format("{0} / {1}", currentStamina, maxStamina);
        }
    }
}
