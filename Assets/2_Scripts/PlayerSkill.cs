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

    [CreateAssetMenu(menuName = "Skill/Player Skill")]
    public class PlayerSkill : Skill
    {
        [Header("Skill Parameter")]
        public PlayerSkillType skillType;
        public int level;
        public int needMana;
        public int coolTime;
        [TextArea]
        public string explainText;
        public int damage;

        [Header("Skill Animation")]
        public string skillAnimationName;
    }
}

