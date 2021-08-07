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
        [SerializeField] public SkillBtn skillBtn_1;
        [SerializeField] public SkillBtn skillBtn_2;
        [SerializeField] public SkillBtn skillBtn_3;
        [SerializeField] public SkillBtn skillBtn_Ult;

        private PlayerAttackAnimation playerAttacker;
        private QuickSlotUI quickSlotUI;

        private void Start()
        {
            playerAttacker = GetComponent<PlayerAttackAnimation>();
            quickSlotUI = FindObjectOfType<QuickSlotUI>();
            try
            {
                skillBtn_1 = quickSlotUI.SkillBtn_1;
                skillBtn_2 = quickSlotUI.SkillBtn_2;
                skillBtn_3 = quickSlotUI.SkillBtn_3;
                skillBtn_Ult = quickSlotUI.SkillBtn_Ult;
            }
            catch (System.Exception)
            {
                throw;
            }
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

                case 2:
                    {
                        if (skillBtn_2.Button.enabled == true)
                        {
                            skillBtn_2.onClick();
                            playerAttacker.HandleSkillAttack(playerSkill_Two);
                        }
                        else
                        {
                            Debug.Log("쿨타임 입니다.");
                        }
                    }
                    break;

                case 3:
                    {
                        if (skillBtn_3.Button.enabled == true)
                        {
                            skillBtn_3.onClick();
                            playerAttacker.HandleSkillAttack(playerSkill_Three);
                        }
                        else
                        {
                            Debug.Log("쿨타임 입니다.");
                        }
                    }
                    break;

                case 4:
                    {
                        if (skillBtn_Ult.Button.enabled == true)
                        {
                            skillBtn_Ult.onClick();
                            playerAttacker.HandleSkillAttack(playerSkill_Ult);
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
