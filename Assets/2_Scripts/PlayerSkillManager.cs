using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    //플레이어가 현재 장착중인 스킬과 소비 아이템을 관리하는 매니저입니다.
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

        [Header("Player Consumable")]
        public ConsumableItem consumableItem_One;

        [Header("Player Consumable Button")]
        [SerializeField] public ConsumableBtn consumableBtn_1;
        

        private PlayerAttackAnimation playerAttacker;
        private QuickSlotUI quickSlotUI;
        private PlayerInventory playerInventory;

        private void Start()
        {
            playerAttacker = GetComponent<PlayerAttackAnimation>();
            playerInventory = GetComponent<PlayerInventory>();
            quickSlotUI = FindObjectOfType<QuickSlotUI>();
            try
            {
                skillBtn_1 = quickSlotUI.SkillBtn_1;
                skillBtn_2 = quickSlotUI.SkillBtn_2;
                skillBtn_3 = quickSlotUI.SkillBtn_3;
                skillBtn_Ult = quickSlotUI.SkillBtn_Ult;

                consumableBtn_1 = quickSlotUI.ConsumesSlot;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            SetPlayerHUDConsumableSlot(playerInventory.currentConsumable);
        }

        public void SetPlayerHUDSkillSlot(List<SkillSlot_Current> skillList)
        {
            playerSkill_One = skillList[0].PlayerSkill;
            playerSkill_Two = skillList[1].PlayerSkill;
            playerSkill_Three = skillList[2].PlayerSkill;
            playerSkill_Ult = skillList[3].PlayerSkill;

            quickSlotUI.UpdateSkillSlotsUI(1, playerSkill_One);
            quickSlotUI.UpdateSkillSlotsUI(2, playerSkill_Two);
            quickSlotUI.UpdateSkillSlotsUI(3, playerSkill_Three);
            quickSlotUI.UpdateSkillSlotsUI(4, playerSkill_Ult);
        }

        public void SetPlayerHUDConsumableSlot(ConsumableItem consumableItem)
        {
            consumableItem_One = consumableItem;
            quickSlotUI.UpdateQuickSlotUI(consumableItem_One);
        }

        public void UseSkill(int skillNum)
        {
            switch (skillNum)
            {
                case 1:
                    {
                        if (skillBtn_1.Button.enabled == true)
                        {
                            skillBtn_1.OnClick();
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
                            skillBtn_2.OnClick();
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
                            skillBtn_3.OnClick();
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
                            skillBtn_Ult.OnClick();
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
