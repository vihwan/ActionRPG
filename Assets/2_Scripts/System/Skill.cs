using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class Skill : ScriptableObject
    {
        [Header("Skill Information")]
        [SerializeField] private Sprite skillImage;
        [SerializeField] private string skillName;

        public Sprite SkillImage { get => skillImage; private set => skillImage = value; }
        public string SkillName { get => skillName; private set => skillName = value; }
    }
}

