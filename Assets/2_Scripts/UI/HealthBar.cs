using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class HealthBar : MonoBehaviour
    {
        private Slider slider;
        [SerializeField] private TMP_Text healthText;
        public void Init()
        {
            slider = GetComponent<Slider>();
            if (slider == null)
                Debug.Log("Slider Null");

            healthText = GetComponentInChildren<TMP_Text>(true);
        }

        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        public void SetCurrentHealth(int currentHealth)
        {
            slider.value = currentHealth;
        }

        public void SetHealthText(int currentHealth, int maxHealth)
        {
            healthText.text = string.Format("{0} / {1}", currentHealth, maxHealth);
        }
    }

}

