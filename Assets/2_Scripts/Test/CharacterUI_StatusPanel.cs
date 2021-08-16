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


        //Property
        public TMP_Text LevelText { get => levelText; set => levelText = value; }
        public TMP_Text ExpText { get => expText; set => expText = value; }
        public Slider ExpSlider { get => expSlider; set => expSlider = value; }
        public TMP_Text HpText { get => hpText; set => hpText = value; }
        public TMP_Text AttackText { get => attackText; set => attackText = value; }
        public TMP_Text DefenseText { get => defenseText; set => defenseText = value; }
        public TMP_Text CriticalText { get => criticalText; set => criticalText = value; }
        public TMP_Text CriticalDamageText { get => criticalDamageText; set => criticalDamageText = value; }
        public TMP_Text StaminaText { get => staminaText; set => staminaText = value; }

        public void Init()
        {
            nameText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Name");
            LevelText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Level");
            ExpText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Exp");
            ExpSlider = GetComponentInChildren<Slider>();

            HpText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/HP");
            AttackText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/ATK");
            DefenseText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/DEF");
            CriticalText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/CRI");
            CriticalDamageText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/CRIDMG");
            StaminaText = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/STA");
        }
    }
}
