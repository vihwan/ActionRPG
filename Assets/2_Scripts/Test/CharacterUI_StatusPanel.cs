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
        [SerializeField] private PlayerInventory playerInventory;

        //Property
        public TMP_Text LevelText { get => levelText; private set => levelText = value; }
        public TMP_Text ExpText { get => expText; private set => expText = value; }
        public Slider ExpSlider { get => expSlider; private set => expSlider = value; }
        public TMP_Text HpText { get => hpText; private set => hpText = value; }
        public TMP_Text AttackText { get => attackText; private set => attackText = value; }
        public TMP_Text DefenseText { get => defenseText; private set => defenseText = value; }
        public TMP_Text CriticalText { get => criticalText; private set => criticalText = value; }
        public TMP_Text CriticalDamageText { get => criticalDamageText; private set => criticalDamageText = value; }
        public TMP_Text StaminaText { get => staminaText; private set => staminaText = value; }

        public void Init()
        {
            nameText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Name");
            LevelText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Level");
            ExpText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Exp");
            ExpSlider = GetComponentInChildren<Slider>();

            HpText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/HP/Text");
            AttackText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/ATK/Text");
            DefenseText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/DEF/Text");
            CriticalText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/CRI/Text");
            CriticalDamageText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/CRIDMG/Text");
            StaminaText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/STA/Text");

            playerStats = FindObjectOfType<PlayerStats>();
            playerInventory = FindObjectOfType<PlayerInventory>();
        }

        public void SetParameter()
        {
            nameText.text = playerStats.playerName;
            levelText.text = "Lv. " + playerStats.playerLevel;
            expText.text = playerStats.playerExp + " / 1000";
            SetMaxExpSlider();
            hpText.text = playerStats.currentHealth + " / " + playerStats.maxHealth;
            attackText.text = (playerStats.attack + playerInventory.rightWeapon.attack).ToString();
            defenseText.text = playerStats.defense.ToString();
            criticalText.text = playerStats.critical + "%";
            criticalDamageText.text = playerStats.criticalDamage * 100 + "%";
            staminaText.text = playerStats.stamina.ToString();
        }

        private void SetMaxExpSlider()
        {
            expSlider.maxValue = 1000f;
            expSlider.value = playerStats.playerExp;
        }

        public void SetCurrentExpSlider()
        {
            expSlider.value = playerStats.playerExp;
        }
    }
}
