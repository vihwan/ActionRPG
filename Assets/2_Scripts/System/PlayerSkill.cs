using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public enum PlayerSkillType
    {
        Active,  //액티브
        Passive, //패시브
        Ultimate //궁극기
    }

    [System.Serializable]
    public class DamageMap
    {
        public string damageName;
        public int damage;
    }

    [CreateAssetMenu(menuName = "Skill/Player Skill")]
    public class PlayerSkill : Skill
    {
        [Header("Skill Parameter")]
        [SerializeField] private PlayerSkillType skillType;
        [SerializeField] private int level;
        [SerializeField] private int needMana;
        [SerializeField] private int coolTime;
        [TextArea, SerializeField] private string explainText;
        public DamageMap[] damageDic;

        [Header("Skill Animation")]
        public string skillAnimationName;

        public PlayerSkillType SkillType { get => skillType; private set => skillType = value; }
        public int Level { get => level; private set => level = value; }
        public int NeedMana { get => needMana; private set => needMana = value; }
        public int CoolTime { get => coolTime; private set => coolTime = value; }
        public string ExplainText { get => explainText; private set => explainText = value; }

        [ExecuteInEditMode]
        private void OnValidate()
        {
            if (level <= 0)
                level = 1;

            if (needMana < 0)
                needMana = 0;

            if (coolTime < 0)
                coolTime = 0;
        }
    }
}

