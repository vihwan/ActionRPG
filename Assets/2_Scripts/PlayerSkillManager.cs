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
        private PlayerStats playerStats;

        public void Init()
        {
            playerAttacker = GetComponent<PlayerAttackAnimation>();
            playerStats = GetComponent<PlayerStats>();
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

            SetPlayerHUDConsumableSlot(PlayerInventory.Instance.currentConsumableItem);
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
            quickSlotUI.UpdateConsumeSlotUI(consumableItem_One);
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

        public void UseConsumeItem()
        {
            //플레이어 체력 회복
            //플레이어 체력이 전부 회복되어있을 경우 아이템 사용 불가.
            //UI갱신 - 체력바
            //UI갱신 - 퀵슬롯

            if (consumableBtn_1.isAct == true)
            {
                if (playerStats.GetCurrentHealthEqualsMaxHealth())
                {
                    Debug.Log("회복할 체력이 없습니다.");
                    return;
                }
                //버튼 쿨타임 활성화
                consumableBtn_1.OnClick();
                //플레이어 체력 갱신
                playerStats.PlusStatsByComsumableItem(consumableItem_One);

                //퀵슬롯 정보 갱신
                consumableItem_One.quantity -= 1;
                if (consumableItem_One.quantity == 0) 
                {
                    //만약 해당 소비 아이템을 전부 사용했을 경우
                    //인벤토리에 해당 아이템을 제거하고 null로 퀵슬롯 UI를 갱신
                    bool allDelete = false;
                    PlayerInventory.Instance.SaveDeleteItemToInventoryConsIngred(consumableItem_One, out allDelete);
                    if (allDelete.Equals(true))
                        Debug.Log("인벤토리에서 성공적으로 제거되었습니다.");

                    consumableItem_One = null;
                }
                quickSlotUI.UpdateConsumeSlotUI(consumableItem_One);
            }
            else
            {
                Debug.Log("아이템 사용 불가");
            }
        }
    }
}
