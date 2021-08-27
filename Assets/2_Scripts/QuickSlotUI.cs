using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    //스킬 아이콘 및 퀵슬롯 UI들을 관리하는 스크립트
    public class QuickSlotUI : MonoBehaviour
    {
        private SkillBtn skillBtn_1;
        private SkillBtn skillBtn_2;
        private SkillBtn skillBtn_3;
        private SkillBtn skillBtn_Ult;
        private SkillBtn consumesSlot;

        private PlayerSkillManager skillManager;

        public SkillBtn SkillBtn_1 { get => skillBtn_1; private set => skillBtn_1 = value; }
        public SkillBtn SkillBtn_2 { get => skillBtn_2; private set => skillBtn_2 = value; }
        public SkillBtn SkillBtn_3 { get => skillBtn_3; private set => skillBtn_3 = value; }
        public SkillBtn SkillBtn_Ult { get => skillBtn_Ult; private set => skillBtn_Ult = value; }

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

            consumesSlot = UtilHelper.Find<SkillBtn>(transform, "ConsumesSlot");

            skillManager = FindObjectOfType<PlayerSkillManager>();
            if (skillManager != null)
            {
                UpdateSkillSlotsUI(1, skillManager.playerSkill_One);
                UpdateSkillSlotsUI(2, skillManager.playerSkill_Two);
                UpdateSkillSlotsUI(3, skillManager.playerSkill_Three);
                UpdateSkillSlotsUI(4, skillManager.playerSkill_Ult);
            }
        }

        //스킬 슬롯을 갱신하는 함수
        public void UpdateSkillSlotsUI(int skillSlotNum, PlayerSkill playerSkill)
        {
            switch (skillSlotNum)
            {
                case 1:
                    skillBtn_1.SetActiveBtn(playerSkill);
                    break;

                case 2:
                    skillBtn_2.SetActiveBtn(playerSkill);
                    break;

                case 3:
                    skillBtn_3.SetActiveBtn(playerSkill);
                    break;

                case 4:
                    skillBtn_Ult.SetActiveBtn(playerSkill);
                    break;
            }
        }

        //퀵슬롯 아이콘(ex. 소비템)을 갱신하는 함수
        public void UpdateQuickSlotUI()
        {

        }

        /*        public void UpdateWeaponQuickSlotsUI(bool isLeft, WeaponItem weaponItem)
                {

                    if (isLeft == false)
                    {
                        if(weaponItem.itemIcon != null)
                        {
                            rightWeaponIcon.sprite = weaponItem.itemIcon;
                            rightWeaponIcon.enabled = true;
                        }
                        else
                        {

                        }
                    }
                }*/
    }

}
