using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Text;

namespace SG
{
    public class CharacterUI_EquipmentPanel : MonoBehaviour
    {
        [Header("Equipment Slot")]
        [SerializeField] private EquipSlot[] equipSlots;

        [Header("Basic")]
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private TMP_Text equipEffectText;
        [SerializeField] private SetTemplate[] setTemplates;

        [Header("Need Component")]
        [SerializeField] internal PlayerInventory playerInventory;
        private CharacterWindowUI characterWindowUI; //상위 계층 

        public StringBuilder sb = new StringBuilder();
        public void Init()
        {
            equipSlots = GetComponentsInChildren<EquipSlot>(true);
            if(equipSlots != null)
            {
                
            }

            mainPanel = transform.Find("UI Background Main").gameObject;
            if(mainPanel != null)
            {
                equipEffectText = UtilHelper.Find<TMP_Text>(mainPanel.transform, "EquipEffect/Status/Text");
            }

            setTemplates = GetComponentsInChildren<SetTemplate>(true);

            #region Component Initialize
            playerInventory = FindObjectOfType<PlayerInventory>();
            if (playerInventory == null)
                Debug.LogWarning("playerInventory를 찾지 못했습니다.");

            characterWindowUI = GetComponentInParent<CharacterWindowUI>();
            #endregion
        }

        private void OnEnable()
        {
            SetParameterMain(playerInventory.currentEquipmentSlots);
        }

        //장비 전체 효과를 설정하는 함수
        //플레이어가 착용하고 있는 각 장비들의 스테이터스를 전부 더해 보여주어야한다.
        private void SetParameterMain(EquipItem[] currentEquipmentSlots)
        {
            int wholeHp = 0;
            int wholeAttack = 0;
            int wholeDefense = 0;
            int wholeCritical = 0;
            int wholeCriticalDMG = 0;
            int wholeStamina = 0;

            foreach (EquipItem item in currentEquipmentSlots)
            {
                wholeHp += item.itemAttributes[(int)Attribute.Hp].value;
                wholeAttack += item.itemAttributes[(int)Attribute.Attack].value;
                wholeDefense += item.itemAttributes[(int)Attribute.Defense].value;
                wholeCritical += item.itemAttributes[(int)Attribute.Critical].value;
                wholeCriticalDMG += item.itemAttributes[(int)Attribute.CriticalDamage].value;
                wholeStamina += item.itemAttributes[(int)Attribute.Stamina].value;
            }

            SetItemStatusText(equipEffectText, wholeHp, wholeAttack, wholeDefense, wholeCritical, wholeCriticalDMG, wholeStamina);
            CreateSetItemTemplate(currentEquipmentSlots);
        }


        //착용하고 있는 아이템들을 서로 비교하여, 일정 갯수 이상 일 때 세트 아이템 효과를 출력하는 템플릿을 생성하는 함수
        private void CreateSetItemTemplate(EquipItem[] currentEquipmentSlots)
        {
            int setItem_Count = 0;
            foreach (EquipItem item in currentEquipmentSlots)
            {
                
            }
        }

        private void AddStatusText(int value, string statName, bool isPercent = false)
        {
            if (value != 0)
            {
                if (sb.Length > 0)
                    sb.AppendLine();

                sb.Append(statName);
                switch (statName.Length)
                {
                    case 2:
                        sb.Append("                      ");
                        break;

                    case 3:
                        sb.Append("                   ");
                        break;

                    case 4:
                        sb.Append("                ");
                        break;

                    case 6:
                        sb.Append("           ");
                        break;
                }


                if (value > 0)
                {
                    sb.Append("+");
                    sb.Append(" ");
                }

                if (isPercent)
                {
                    sb.Append(value);
                    sb.Append("%");
                }
                else
                {
                    sb.Append(value);
                }
            }
        }
        private void SetItemStatusText(TMP_Text tMP_Text, int hp, int attack, int defense, int critical, int criticalDMG, int stamina)
        {
            sb.Length = 0;
            AddStatusText(hp, "체력");
            AddStatusText(attack, "공격력");
            AddStatusText(defense, "방어력");
            AddStatusText(critical, "치명타", isPercent: true);
            AddStatusText(criticalDMG, "치명타 배율", isPercent: true);
            AddStatusText(stamina, "스태미나");

            tMP_Text.text = sb.ToString();
        }
    }
}
