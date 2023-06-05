using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace SG
{
    public class CharacterUI_SkillPanel : MonoBehaviour
    {
        [Header("Skill Slots")]
        [SerializeField] private SkillSlot[] skillSlots;
        [SerializeField] internal List<SkillSlot_Current> currentSkillList;
        [SerializeField] internal SkillSlot_Current enteredCurrentSkillSlot;

        [Header("Basic")]
        [SerializeField] private TMP_Text skillName;
        [SerializeField] private TMP_Text skillLevel;
        [SerializeField] private TMP_Text skillType;
        [SerializeField] private TMP_Text skillNeedMana;
        [SerializeField] private TMP_Text skillCoolTime;
        [SerializeField] private TMP_Text skillExplain;
        [SerializeField] private Image skillIcon;
        [SerializeField] private GameObject currentState;
        [SerializeField] private Button skillLevelUpBtn;

        [Header("Skill List Transform")]
        [SerializeField] private Transform activeSkillSlotParentTransform;
        [SerializeField] private Transform passiveSkillSlotParentTransform;
        [SerializeField] private Transform ultimateSkillSlotParentTransform;

        [Header("Skill Slot Prefab")]
        [SerializeField] private SkillSlot skillSlotPrefab;


        [Header("Need Component")]
        private PlayerSkillInventory playerSkillInventory;
        private CharacterWindowUI characterWindowUI; //상위 계층 
        private InputHandler inputHandler;

        public MouseItem mouseItem = new MouseItem();
        public StringBuilder sb = new StringBuilder();


        public void Init()
        {
            Transform t = transform.Find("UI Background").transform;
            skillName = UtilHelper.Find<TMP_Text>(t, "Name");
            skillLevel = UtilHelper.Find<TMP_Text>(t, "Level");
            skillType = UtilHelper.Find<TMP_Text>(t, "Type");
            skillNeedMana = UtilHelper.Find<TMP_Text>(t, "NeedMana");
            skillCoolTime = UtilHelper.Find<TMP_Text>(t, "CoolTime");
            skillExplain = UtilHelper.Find<TMP_Text>(t, "Explain/ExplainText");
            skillIcon = UtilHelper.Find<Image>(t, "IconImage");
            skillLevelUpBtn = UtilHelper.Find<Button>(t, "LevelUpBtn");
            if (skillLevelUpBtn != null)
                skillLevelUpBtn.onClick.AddListener(null);
            currentState = t.Find("CurrentState").gameObject;


            t = transform.Find("Skill List Scroll View/Viewport/Content").transform;
            activeSkillSlotParentTransform = UtilHelper.Find<Transform>(t, "ActiveList/Skill Slot Parent");
            passiveSkillSlotParentTransform = UtilHelper.Find<Transform>(t, "PassiveList/Skill Slot Parent");
            ultimateSkillSlotParentTransform = UtilHelper.Find<Transform>(t, "UltimateList/Skill Slot Parent");


            skillSlotPrefab = Resources.Load<SkillSlot>("Prefabs/SkillSlots/SkillSlot");

            playerSkillInventory = FindObjectOfType<PlayerSkillInventory>();
            characterWindowUI = GetComponentInParent<CharacterWindowUI>();
            inputHandler = FindObjectOfType<InputHandler>();

            InstantiateSkillLists();
            skillSlots = GetComponentsInChildren<SkillSlot>();

            for (int i = 0; i < currentSkillList.Count; i++)
            {
                currentSkillList[i].Init();
            }
        }
        public void OnOpenPanel()
        {
            //스킬 정보창이 디폴트 상태라면, 맨 첫번째 스킬 슬롯의 정보를 보여줍니다.
            if (skillName.text.Equals("스킬 이름"))
            {
                skillSlots[0].isSelect = true;
                skillSlots[0].ChangeFrameColor(skillSlots[0].isSelect);
                SetParameterSkillPanel(skillSlots[0].PlayerSkill);
            }

            if(characterWindowUI.backBtn.gameObject.activeSelf.Equals(true))
            {
                characterWindowUI.backBtn.gameObject.SetActive(false);
            }
        }
        private void InstantiateSkillLists()
        {
            for (int i = 0; i < playerSkillInventory.activeSkills.Count; i++)
            {
                var slot = Instantiate(skillSlotPrefab, activeSkillSlotParentTransform);
                slot.Init();
                slot.AddSkill(playerSkillInventory.activeSkills[i]);
                AddEvent(slot.gameObject, EventTriggerType.BeginDrag, delegate { OnDragStart(slot); });
                AddEvent(slot.gameObject, EventTriggerType.EndDrag, delegate { OnDragEnd(slot); });
                AddEvent(slot.gameObject, EventTriggerType.Drag, delegate { OnDrag(slot); });
            }

            for (int i = 0; i < playerSkillInventory.passiveSkills.Count; i++)
            {
                SkillSlot slot = Instantiate(skillSlotPrefab, passiveSkillSlotParentTransform);
                slot.Init();
                slot.AddSkill(playerSkillInventory.passiveSkills[i]);
            }

            for (int i = 0; i < playerSkillInventory.ultimateSkills.Count; i++)
            {
                SkillSlot slot = Instantiate(skillSlotPrefab, ultimateSkillSlotParentTransform);
                slot.Init();
                slot.AddSkill(playerSkillInventory.ultimateSkills[i]);
                AddEvent(slot.gameObject, EventTriggerType.BeginDrag, delegate { OnDragStart(slot); });
                AddEvent(slot.gameObject, EventTriggerType.EndDrag, delegate { OnDragEnd(slot); });
                AddEvent(slot.gameObject, EventTriggerType.Drag, delegate { OnDrag(slot); });
            }
        }

        private void OnDragStart(SkillSlot slot)
        {
            var mouseObject = new GameObject();
            var rt = mouseObject.AddComponent<RectTransform>();
            rt.sizeDelta = slot.SkillIcon.rectTransform.sizeDelta;
            mouseObject.transform.SetParent(transform.parent);

            var img = mouseObject.AddComponent<Image>();
            img.sprite = slot.SkillIcon.sprite;
            img.raycastTarget = false;

            mouseItem.obj = mouseObject;
            mouseItem.slot = slot;
        }
        private void OnDrag(SkillSlot slot)
        {
            if (mouseItem.obj != null)
            {
                // mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
                mouseItem.obj.GetComponent<RectTransform>().position = Mouse.current.position.ReadValue();
                //Input대신 InputSystem을 사용하고 있으므로, InputSystem의 Mouse의 현재위치를 사용.
            }
        }

        private void OnDragEnd(SkillSlot slot)
        {
            if (mouseItem.slot)
            {
                TrySetCurrentSkillSlot(mouseItem.slot);
            }

            Destroy(mouseItem.obj);
            mouseItem.slot = null;
        }

        private void TrySetCurrentSkillSlot(SkillSlot slot)
        {
            if (enteredCurrentSkillSlot != null)
            {
                if (enteredCurrentSkillSlot.playerSkillType == slot.PlayerSkill.SkillType)
                {
                    enteredCurrentSkillSlot.AddSkill(slot.PlayerSkill);
                }
                else
                {
                    Debug.Log("타입이 맞지 않아 장착할 수 없습니다.");
                    return;
                }
            }
            else
                return;

            //이미 다른 스킬 칸에 장착하고자 하는 스킬이 존재한다면 그 스킬칸을 비운다.
            for (int i = 0; i < currentSkillList.Count; i++)
            {
                if (currentSkillList[i].PlayerSkill != null)
                {
                    if (currentSkillList[i] == enteredCurrentSkillSlot)
                        continue;

                    if (currentSkillList[i].PlayerSkill.Equals(slot.PlayerSkill))
                    {
                        currentSkillList[i].ClearSkill();
                        break;
                    }
                }
            }
        }

        private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = obj.GetComponent<EventTrigger>();
            var eventTrigger = new EventTrigger.Entry();
            eventTrigger.eventID = type;
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }

        public void SetParameterSkillPanel(PlayerSkill playerSkill)
        {
            skillName.text = playerSkill.SkillName;
            skillLevel.text = "Lv." + playerSkill.Level;
            skillType.text = playerSkill.SkillType.ToString();
            skillNeedMana.text = "필요 마나 : " + playerSkill.NeedMana;
            skillCoolTime.text = "쿨타임 : " + playerSkill.CoolTime;
            skillIcon.sprite = playerSkill.SkillImage;

            SetSkillExplain(playerSkill);

            if (playerSkill.SkillType == PlayerSkillType.Passive)
            {
                skillNeedMana.text = null;
                skillCoolTime.text = null;
            }

            if (skillLevelUpBtn.gameObject.activeSelf == true)
            {
                skillLevelUpBtn.onClick.RemoveAllListeners();
                skillLevelUpBtn.onClick.AddListener(() => SkillLevelUp(playerSkill));
            }
        }

        internal void DeselectAllSkillSlots()
        {
            for (int i = 0; i < skillSlots.Length; i++)
            {
                skillSlots[i].isSelect = false;
                skillSlots[i].ChangeFrameColor(skillSlots[i].isSelect);
            }

            for (int i = 0; i < currentSkillList.Count; i++)
            {
                currentSkillList[i].isSelect = false;
                currentSkillList[i].ChangeFrameColor(currentSkillList[i].isSelect);
            }
        }

        private void SkillLevelUp(PlayerSkill playerSkill)
        {
            Debug.Log("미구현 기능");
        }

        private void SetSkillExplain(PlayerSkill playerSkill)
        {
            sb.Length = 0;
            sb.Append(playerSkill.ExplainText);

            skillExplain.text = sb.ToString();
        }
    }
}
