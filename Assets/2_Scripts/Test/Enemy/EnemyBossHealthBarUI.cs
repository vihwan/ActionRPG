using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class EnemyBossHealthBarUI : MonoBehaviour
    {
        public TMP_Text bossNameText;
        public Slider hpSlider;

        public void Init()
        {
            hpSlider = UtilHelper.Find<Slider>(transform, "HealthBar");
            bossNameText = UtilHelper.Find<TMP_Text>(hpSlider.transform, "BossNameText");

            SetActiveEnemyBossHealthBar(false);
        }

        public void SetActiveEnemyBossHealthBar(bool state)
        {
            hpSlider.gameObject.SetActive(state);
        }

        public void SetBossMaxHealth(int maxHealth)
        {
            hpSlider.maxValue = maxHealth;
            hpSlider.value = maxHealth;
        }

        public void SetBossCurrentHealth(int currentHealth)
        {
            hpSlider.value = currentHealth;
        }
    }
}
