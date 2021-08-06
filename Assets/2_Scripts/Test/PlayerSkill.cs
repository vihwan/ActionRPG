using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    [CreateAssetMenu(menuName = "Skill/Player Skill")]
    public class PlayerSkill : Skill
    {
        [Header("Skill Animation")]
        public string skillAnimationName;
    }
}

