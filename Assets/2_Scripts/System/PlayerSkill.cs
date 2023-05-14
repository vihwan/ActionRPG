﻿using System.Collections;
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
        [SerializeField] private PlayerSkillType skillType = PlayerSkillType.Active;
        [SerializeField] private int level = 0;
        [SerializeField] private int needMana = 0;
        [SerializeField] private int coolTime = 0;
        [TextArea, SerializeField] private string explainText = string.Empty;
        public DamageMap[] damageDic;

        [Header("Skill Animation")]
        [SerializeField]
        private string m_skillAnimationName = string.Empty;
        public string SkillAnimationName => m_skillAnimationName;

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

