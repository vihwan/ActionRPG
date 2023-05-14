using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    //스킬 아이콘 및 퀵슬롯 UI들을 관리하는 스크립트
    public class QuickSlotUI : MonoBehaviour
    {
        private PlayerSkillManager skillManager;

        public SkillBtn SkillBtn_1 { get; private set; }
        public SkillBtn SkillBtn_2 { get; private set; }
        public SkillBtn SkillBtn_3 { get; private set; }
        public SkillBtn SkillBtn_Ult { get; private set; }
        public ConsumableBtn ConsumesSlot { get; private set; }

        public void Init()
        {
            SkillBtn_1 = UtilHelper.Find<SkillBtn>(transform, "Skill_1");
            if (SkillBtn_1 != null)
                SkillBtn_1.Init();

            SkillBtn_2 = UtilHelper.Find<SkillBtn>(transform, "Skill_2");
            if (SkillBtn_2 != null)
                SkillBtn_2.Init();

            SkillBtn_3 = UtilHelper.Find<SkillBtn>(transform, "Skill_3");
            if (SkillBtn_3 != null)
                SkillBtn_3.Init();

            SkillBtn_Ult = UtilHelper.Find<SkillBtn>(transform, "Skill_Ult");
            if (SkillBtn_Ult != null)
                SkillBtn_Ult.Init();

            ConsumesSlot = UtilHelper.Find<ConsumableBtn>(transform, "ConsumesSlot");
            if (ConsumesSlot != null)
                ConsumesSlot.Init();

            skillManager = FindObjectOfType<PlayerSkillManager>();
            if (skillManager != null)
            {
                UpdateSkillSlotsUI(1, skillManager.playerSkill_One);
                UpdateSkillSlotsUI(2, skillManager.playerSkill_Two);
                UpdateSkillSlotsUI(3, skillManager.playerSkill_Three);
                UpdateSkillSlotsUI(4, skillManager.playerSkill_Ult);
                UpdateConsumeSlotUI(skillManager.ConsumableItem_One);
            }
        }

        //스킬 슬롯을 갱신하는 함수
        public void UpdateSkillSlotsUI(int skillSlotNum, PlayerSkill playerSkill)
        {
            switch (skillSlotNum)
            {
                case 1:
                    SkillBtn_1.SetActiveBtn(playerSkill);
                    break;

                case 2:
                    SkillBtn_2.SetActiveBtn(playerSkill);
                    break;

                case 3:
                    SkillBtn_3.SetActiveBtn(playerSkill);
                    break;

                case 4:
                    SkillBtn_Ult.SetActiveBtn(playerSkill);
                    break;
            }
        }

        //퀵슬롯 아이콘(ex. 소비템)을 갱신하는 함수
        public void UpdateConsumeSlotUI(ConsumableItem consumableItem)
        {
            ConsumesSlot.SetActiveBtn(consumableItem);
        }
    }
}
