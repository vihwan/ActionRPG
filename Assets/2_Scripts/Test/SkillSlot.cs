using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SG
{
    public class SkillSlot : MonoBehaviour
    {
        [SerializeField] private PlayerSkill playerSkill;
        [SerializeField] private Image skillIcon;
        [SerializeField] private Button slotBtn;
        [SerializeField] private TMP_Text levelText;

        public void Init()
        {
            slotBtn = GetComponentInChildren<Button>();
            if (slotBtn != null)
                slotBtn.onClick.AddListener(OpenSkillInformation);

            skillIcon = UtilHelper.Find<Image>(slotBtn.transform, "Image");
            levelText = GetComponentInChildren<TMP_Text>();
        }

        public void AddSkill(PlayerSkill playerSkill)
        {
            this.playerSkill = playerSkill;
            this.skillIcon.sprite = playerSkill.skillImage;
            this.levelText.text = playerSkill.level.ToString();
        }

        public void OpenSkillInformation()
        {
           //스킬 상세 정보 창을 엽니다.
           //OpenSkillInformationPanel(this.playerSkill);
        }

        public void NotAcquireSkill()
        {

        }

        public void ChangeBackgroundColor()
        {
            //버튼 테두리 색상을 변경합니다.
            //스킬 아이콘을 클릭 했을 시, 클릭되었다는 것을 확인하기 위해 동작하는 함수
        }

    }

}
