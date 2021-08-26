using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

namespace SG
{
    public class CharacterUI_SkillPanel : MonoBehaviour
    {
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

        [Header("Current Skill Slot")]
        [SerializeField] private List<SkillSlot> currentSkillList;

        [Header("Skill List Transform")]
        [SerializeField] private Transform activeSkillSlotParentTransform;
        [SerializeField] private Transform passiveSkillSlotParentTransform;
        [SerializeField] private Transform ultimateSkillSlotParentTransform;

        [Header("Skill Slot Prefab")]
        [SerializeField] private SkillSlot skillSlot;

        [Header("Need Component")]
        private PlayerSkillManager playerSkillManager;
        private PlayerSkillInventory playerSkillInventory;
        private CharacterWindowUI characterWindowUI; //상위 계층 

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


            skillSlot = Resources.Load<SkillSlot>("Prefabs/SkillSlots/SkillSlot");

            playerSkillManager = FindObjectOfType<PlayerSkillManager>();
            playerSkillInventory = FindObjectOfType<PlayerSkillInventory>();
            characterWindowUI = GetComponentInParent<CharacterWindowUI>();

            InstantiateSkillLists();
        }
        private void OnEnable()
        {

        }
        private void InstantiateSkillLists()
        {
            for (int i = 0; i < playerSkillInventory.activeSkills.Count; i++)
            {
                SkillSlot slot = Instantiate(skillSlot, activeSkillSlotParentTransform);
                slot.Init();
            }

            for (int i = 0; i < playerSkillInventory.passiveSkills.Count; i++)
            {
                SkillSlot slot = Instantiate(skillSlot, passiveSkillSlotParentTransform);
                slot.Init();
            }

            for (int i = 0; i < playerSkillInventory.ultimateSkills.Count; i++)
            {
                SkillSlot slot = Instantiate(skillSlot, ultimateSkillSlotParentTransform);
                slot.Init();
            }
        }

        public void SetParameterSkillPanel(PlayerSkill playerSkill)
        {
            skillName.text = playerSkill.skillName;
            skillLevel.text = "Lv." + playerSkill.level;
            skillType.text = playerSkill.skillType.ToString();
            skillNeedMana.text = "필요 마나 : " + playerSkill.needMana;
            skillCoolTime.text = "쿨타임 : " + playerSkill.coolTime;
            skillIcon.sprite = playerSkill.skillImage;

            SetSkillExplain(playerSkill);

            if (skillLevelUpBtn.gameObject.activeSelf == true)
            {
                skillLevelUpBtn.onClick.RemoveAllListeners();
                skillLevelUpBtn.onClick.AddListener(() => SkillLevelUp(playerSkill));
            }
        }

        private void SkillLevelUp(PlayerSkill playerSkill)
        {
          
        }

        private void SetSkillExplain(PlayerSkill playerSkill)
        {
            
        }
    }
}
