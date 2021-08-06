using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    //스킬 아이콘 및 퀵슬롯 UI들을 관리하는 스크립트
    public class QuickSlotUI : MonoBehaviour
    {
        private Image Icon_Skill_1;
        private Image Icon_Skill_2;
        private Image Icon_Skill_3;
        private Image Icon_Skill_Ult;
        private Image Icon_QuickSlot;

        public void Init()
        {
            Icon_Skill_1 = UtilHelper.Find<Image>(transform, "Skill_1/Mask/Icon");
            Icon_Skill_2 = UtilHelper.Find<Image>(transform, "Skill_2/Icon");
            Icon_Skill_3 = UtilHelper.Find<Image>(transform, "Skill_3/Icon");
            Icon_Skill_Ult = UtilHelper.Find<Image>(transform, "Skill_Ult/Icon");
            Icon_QuickSlot = UtilHelper.Find<Image>(transform, "QuickSlot/Icon");
        }

        //스킬 슬롯을 갱신하는 함수
        public void UpdateSkillSlotsUI(int skillSlotNum, PlayerSkill skill)
        {
            switch (skillSlotNum)
            {
                case 1:
                    Icon_Skill_1.sprite = skill.skillImage;
                    Icon_Skill_1.enabled = true;
                    break;
                case 2:
                    Icon_Skill_2.sprite = skill.skillImage;
                    Icon_Skill_2.enabled = true;
                    break;
                case 3:
                    Icon_Skill_3.sprite = skill.skillImage;
                    Icon_Skill_3.enabled = true;
                    break;
                case 4:
                    Icon_Skill_Ult.sprite = skill.skillImage;
                    Icon_Skill_Ult.enabled = true;
                    break;
            }
        }

        //퀵슬롯 아이콘(ex. 소비템)을 갱신하는 함수
        public void UpdateQuickSlotUI()
        {

        }
    }

}
