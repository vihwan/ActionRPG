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

        private Image skillIcon_1;
        private Image skillIcon_2;
        private Image skillIcon_3;
        private Image skillIcon_Ult;

        private PlayerSkillManager skillManager;

        public SkillBtn SkillBtn_1 { get => skillBtn_1; private set => skillBtn_1 = value; }
        public SkillBtn SkillBtn_2 { get => skillBtn_2; private set => skillBtn_2 = value; }
        public SkillBtn SkillBtn_3 { get => skillBtn_3; private set => skillBtn_3 = value; }
        public SkillBtn SkillBtn_Ult { get => skillBtn_Ult; private set => skillBtn_Ult = value; }

        public void Init()
        {
            SkillBtn_1 = UtilHelper.Find<SkillBtn>(transform, "Skill_1");
            if (SkillBtn_1 != null)
                skillIcon_1 = UtilHelper.Find<Image>(SkillBtn_1.transform, "Mask/Icon");

            SkillBtn_2 = UtilHelper.Find<SkillBtn>(transform, "Skill_2");
            if (SkillBtn_2 != null)
                skillIcon_2 = UtilHelper.Find<Image>(SkillBtn_2.transform, "Mask/Icon");

            SkillBtn_3 = UtilHelper.Find<SkillBtn>(transform, "Skill_3");
            if (SkillBtn_3 != null)
                skillIcon_3 = UtilHelper.Find<Image>(SkillBtn_3.transform, "Mask/Icon");

            SkillBtn_Ult = UtilHelper.Find<SkillBtn>(transform, "Skill_Ult");
            if (SkillBtn_Ult != null)
                skillIcon_Ult = UtilHelper.Find<Image>(SkillBtn_Ult.transform, "Mask/Icon");

            consumesSlot = UtilHelper.Find<SkillBtn>(transform, "ConsumesSlot");

            skillManager = FindObjectOfType<PlayerSkillManager>();
            if(skillManager != null)
            {
                UpdateSkillSlotsUI(1, skillManager.playerSkill_One);
                UpdateSkillSlotsUI(2, skillManager.playerSkill_Two);
                UpdateSkillSlotsUI(3, skillManager.playerSkill_Three);
                UpdateSkillSlotsUI(4, skillManager.playerSkill_Ult);
            }
        }

        //스킬 슬롯을 갱신하는 함수
        public void UpdateSkillSlotsUI(int skillSlotNum, PlayerSkill skill)
        {
            switch (skillSlotNum)
            {
                case 1:
                    skillIcon_1.sprite = skill.skillImage;
                    skillIcon_1.enabled = true;
                    skillBtn_1.SetCoolTime(skill.coolTime);
                    break;
                case 2:
                    skillIcon_2.sprite = skill.skillImage;
                    skillIcon_2.enabled = true;
                    skillBtn_2.SetCoolTime(skill.coolTime);
                    break;
                case 3:
                    skillIcon_3.sprite = skill.skillImage;
                    skillIcon_3.enabled = true;
                    skillBtn_3.SetCoolTime(skill.coolTime);
                    break;
                case 4:
                    skillIcon_Ult.sprite = skill.skillImage;
                    skillIcon_Ult.enabled = true;
                    skillBtn_Ult.SetCoolTime(skill.coolTime);
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
