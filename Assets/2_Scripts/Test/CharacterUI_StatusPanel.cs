using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SG
{
    //캐릭터 상태를 텍스트 및 슬라이더로 표시해주는 스크립트
    public class CharacterUI_StatusPanel : MonoBehaviour
    {
        [Header("Basic")]
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text expText;
        [SerializeField] private Slider expSlider;

        [Header("Status")]
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text attackText;
        [SerializeField] private TMP_Text defenseText;
        [SerializeField] private TMP_Text criticalText;
        [SerializeField] private TMP_Text criticalDamageText;
        [SerializeField] private TMP_Text staminaText;

        [Header("Need Component")]
        [SerializeField] private PlayerStats playerStats;

        public void Init()
        {
            nameText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Name");
            levelText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Level");
            expText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Exp");
            expSlider = GetComponentInChildren<Slider>();

            hpText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/HP/Text");
            attackText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/ATK/Text");
            defenseText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/DEF/Text");
            criticalText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/CRI/Text");
            criticalDamageText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/CRIDMG/Text");
            staminaText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/STA/Text");

            playerStats = FindObjectOfType<PlayerStats>();
        }

        public void OnEnable()
        {
            SetParameter();
        }

        public void SetParameter()
        {
            nameText.text = playerStats.playerName;
            levelText.text = "Lv. " + playerStats.PlayerLevel;
            expText.text = playerStats.PlayerExp + " / 1000";
            SetMaxExpSlider();
            hpText.text = playerStats.CurrentHealth + " / " + playerStats.MaxHealth;
            attackText.text = playerStats.Attack.ToString();
            defenseText.text = playerStats.Defense.ToString();
            criticalText.text = playerStats.Critical + "%";
            criticalDamageText.text = playerStats.CriticalDamage + "%";
            staminaText.text = playerStats.CurrentStamina + " / " + playerStats.MaxStamina;
        }

        private void SetMaxExpSlider()
        {
            expSlider.maxValue = 1000f;
            expSlider.value = playerStats.PlayerExp;
        }

        public void SetCurrentExpSlider()
        {
            expSlider.value = playerStats.PlayerExp;
        }
    }
}
