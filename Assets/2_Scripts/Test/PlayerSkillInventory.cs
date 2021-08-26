using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerSkillInventory : MonoBehaviour
    {
        [SerializeField] public List<PlayerSkill> activeSkills;
        [SerializeField] public List<PlayerSkill> passiveSkills;
        [SerializeField] public List<PlayerSkill> ultimateSkills;


    }

}
