using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace SG
{
    public class SkillSlot_Current : MonoBehaviour
    {
        [SerializeField] private PlayerSkill playerSkill;
        [SerializeField] public PlayerSkillType playerSkillType;
        [SerializeField] private Image skillIcon;
        [SerializeField] private Button slotBtn;
        [SerializeField] private Image slotBtnImage;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] internal bool isSelect = false;

        [SerializeField] private Sprite frameNormal;
        [SerializeField] private Sprite frameGold;

        [SerializeField] private CharacterUI_SkillPanel skillPanel;

        public PlayerSkill PlayerSkill { get => playerSkill; set => playerSkill = value; }

        public void Init()
        {

            slotBtn = GetComponentInChildren<Button>();
            if (slotBtn != null)
                slotBtn.onClick.AddListener(OpenSkillInformation);

            slotBtnImage = slotBtn.GetComponent<Image>();
            skillIcon = UtilHelper.Find<Image>(slotBtn.transform, "Image");
            nameText = GetComponentInChildren<TMP_Text>();

            frameNormal = Resources.Load<Sprite>("Sprites/Item/Frame/frame_normal");
            frameGold = Resources.Load<Sprite>("Sprites/Item/Frame/frame_select");

            skillPanel = GetComponentInParent<CharacterUI_SkillPanel>();

            AddEvent(this.gameObject, EventTriggerType.PointerEnter, delegate { OnEnter(this); });
            AddEvent(this.gameObject, EventTriggerType.PointerExit, delegate { OnExit(this); });

            //임시적으로 스킬칸을 전부 비움.
            ClearSkill();
        }
        public void AddSkill(PlayerSkill playerSkill)
        {
            this.playerSkill = playerSkill;
            skillIcon.color = new Color(1, 1, 1, 1);
            skillIcon.sprite = playerSkill.SkillImage;
            nameText.text = string.Format("{0}", playerSkill.SkillName);
        }

        public void ClearSkill()
        {
            this.playerSkill = null;
            skillIcon.sprite = null;
            skillIcon.color = new Color(1, 1, 1, 0);
            nameText.text = null;
        }

        public void OpenSkillInformation()
        {
            skillPanel.DeselectAllSkillSlots();
            isSelect = true;
            ChangeFrameColor(isSelect);
            skillPanel.SetParameterSkillPanel(playerSkill);
        }

        private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = obj.GetComponent<EventTrigger>();
            var eventTrigger = new EventTrigger.Entry();
            eventTrigger.eventID = type;
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }

        private void OnExit(SkillSlot_Current skillSlot_Current)
        {
            skillPanel.enteredCurrentSkillSlot = null;
        }

        private void OnEnter(SkillSlot_Current skillSlot_Current)
        {
            skillPanel.enteredCurrentSkillSlot = skillSlot_Current;
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
