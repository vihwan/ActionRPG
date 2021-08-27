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
        [SerializeField] private Image slotBtnImage;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] internal bool isSelect = false;

        [SerializeField] private Sprite frameNormal;
        [SerializeField] private Sprite frameGold;

        [SerializeField] private CharacterUI_SkillPanel skillPanel;

        public PlayerSkill PlayerSkill { get => playerSkill; private set => playerSkill = value; }
        public Image SkillIcon { get => skillIcon; set => skillIcon = value; }

        public void Init()
        {
            slotBtn = GetComponentInChildren<Button>();
            if (slotBtn != null)
                slotBtn.onClick.AddListener(OpenSkillInformation);

            slotBtnImage = slotBtn.GetComponent<Image>();

            skillIcon = UtilHelper.Find<Image>(slotBtn.transform, "Image");
            levelText = GetComponentInChildren<TMP_Text>();

            frameNormal = Resources.Load<Sprite>("Sprites/Item/Frame/frame_normal");
            frameGold = Resources.Load<Sprite>("Sprites/Item/Frame/frame_select");

            skillPanel = GetComponentInParent<CharacterUI_SkillPanel>();
        }

        public void AddSkill(PlayerSkill playerSkill)
        {
            this.playerSkill = playerSkill;
            this.skillIcon.sprite = playerSkill.SkillImage;
            this.levelText.text = playerSkill.Level.ToString();
        }

        public void OpenSkillInformation()
        {
            skillPanel.DeselectAllSkillSlots();
            isSelect = true;
            ChangeFrameColor(isSelect);

            skillPanel.SetParameterSkillPanel(playerSkill);
        }

        public void NotAcquireSkill()
        {

        }
        public void ChangeFrameColor(bool state)
        {
            //선택, 미선택 상태인 장비 슬롯을 구별하기 위해 프레임 스프라이트를 바꾼다.
            //임시적인 방안이므로, 나중에 다른 방법을 사용할 수 있습니다.
            if (state == true)
            {
                slotBtnImage.sprite = frameGold;
            }
            else
            {
                slotBtnImage.sprite = frameNormal;
            }
        }
    }

}
