using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class EnemyHealthBarUI : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private float timeUntilBarIsHidden = 0f;

        public void Init()
        {
            slider = GetComponentInChildren<Slider>();
        }

        public void SetHealth(int health)
        {
            slider.value = health;
            timeUntilBarIsHidden = 3f;
        }

        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        private void Update()
        {
            timeUntilBarIsHidden = timeUntilBarIsHidden - Time.deltaTime;

            if (slider?.gameObject)
            {
                slider.gameObject.transform.LookAt(Camera.main.transform.position);
                slider.gameObject.transform.Rotate(0f,180f,0f);

                if (timeUntilBarIsHidden <= 0)
                {
                    timeUntilBarIsHidden = 0f;
                    slider.gameObject.SetActive(false);
                }
                else
                {
                    if (slider.gameObject.activeInHierarchy.Equals(false))
                    {
                        slider.gameObject.SetActive(true);
                    }
                }

                if (slider.value <= 0)
                {
                    slider.interactable = false;
                    slider.gameObject.SetActive(false);
                }
            }
        }
    }
}
