using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
        [SerializeField] private LevelManager levelManager;

        public void Init()
        {
            //nameText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Name");
            //levelText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Level");
            //expText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Exp");
            //expSlider = GetComponentInChildren<Slider>();

            //hpText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/HP/Text");
            //attackText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/ATK/Text");
            //defenseText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/DEF/Text");
            //criticalText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/CRI/Text");
            //criticalDamageText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/CRIDMG/Text");
            //staminaText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/STA/Text");

            playerStats = FindObjectOfType<PlayerStats>();
            //levelManager = FindObjectOfType<LevelManager>();
        }
        
        public void OnOpenPanel()
        {
            SetParameter();
        }

        public void SetParameter()
        {
            nameText.text = PlayerManager.it.playerName;
            levelText.text = "Lv. " + LevelManager.it.Level;
            expText.text = string.Format("{0} / {1}",
                            LevelManager.it.Experience, LevelManager.it.GetExperienceNextLevel(LevelManager.it.Level));
            SetExpSlider();
            hpText.text = playerStats.CurrentHealth + " / " + playerStats.MaxHealth;
            attackText.text = playerStats.Attack.ToString();
            defenseText.text = playerStats.Defense.ToString();
            criticalText.text = playerStats.Critical + "%";
            criticalDamageText.text = playerStats.CriticalDamage + "%";
            staminaText.text = string.Format("{0:0} / {1:0}", playerStats.CurrentStamina, playerStats.MaxStamina);
        }

        private void SetExpSlider()
        {
            expSlider.value = LevelManager.it.GetExperienceNormalized();
        }
    }
}
