using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerSkillManager : MonoBehaviour
    {
        [Header("Player Skill")]
        public PlayerSkill playerSkill_One;
        public PlayerSkill playerSkill_Two;
        public PlayerSkill playerSkill_Three;
        public PlayerSkill playerSkill_Ult;

        [Header("Player Skill Button")]
        public SkillBtn skillBtn_1;

        private PlayerAttackAnimation playerAttacker;

        private void Start()
        {
            playerAttacker = GetComponent<PlayerAttackAnimation>();
        }

        public void UseSkill(int skillNum)
        {
            switch (skillNum)
            {
                case 1:
                    {
                        if (skillBtn_1.Button.enabled == true)
                        {
                            skillBtn_1.onClick();
                            playerAttacker.HandleSkillAttack(playerSkill_One);
                        }
                        else
                        {
                            Debug.Log("쿨타임 입니다.");
                        }            
                    }
                    break;
            }
        }
    }
}
