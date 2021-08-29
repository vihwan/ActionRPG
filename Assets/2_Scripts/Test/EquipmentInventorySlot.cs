﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class EquipmentInventorySlot : InventorySlot
    {
        [SerializeField] internal EquipItem item;
        [SerializeField] private bool isArmed;

        private EquipmentInventoryList equipmentInventoryList;
        private CharacterUI_EquipmentPanel equipPanel;
        private void Awake()
        {
            equipPanel = FindObjectOfType<CharacterUI_EquipmentPanel>();

            itemBtn = GetComponentInChildren<Button>();
            if (itemBtn != null)
            {
                itemBtn.onClick.AddListener(() => equipPanel.SetParameterIndividualEquipItem(item));
                itemBtn.onClick.AddListener(SelectSlot);
               // itemBtn.onClick.AddListener(() => CheckSlotIsCurrentEquipment());
            }
            icon = UtilHelper.Find<Image>(itemBtn.transform, "Image");

            equipmentInventoryList = GetComponentInParent<EquipmentInventoryList>();
        }

        public void AddItem(EquipItem equipItem)
        {
            gameObject.SetActive(true);
            item = equipItem;
            icon.sprite = equipItem.itemIcon;
            icon.enabled = true;
            isArmed = equipItem.isArmed;
            ChangeBackgroundColor();
        }

        public void SelectSlot()
        {
            foreach (EquipmentInventorySlot slot in equipmentInventoryList.equipmentInventorySlots)
            {
                slot.isSelect = false;
                slot.ChangeBackgroundColor();
            }
            isSelect = true;
            ChangeBackgroundColor();
        }


        public void ClearInventorySlot()
        {
            item = null;
            icon.enabled = false;
            icon.sprite = null;
            isArmed = false;
            ChangeBackgroundColor();
            gameObject.SetActive(false);
        }
        public void UpdateSlot(EquipItem equipItem)
        {
            isArmed = equipItem.isArmed;
            ChangeBackgroundColor();
            Debug.Log(equipItem.name + "의 isArmed : " + equipItem.isArmed);
        }

        private void CheckSlotIsCurrentEquipment(EquipItem equipItem)
        {
            if (item == equipItem)
            {
                CharacterUI_EquipmentPanel ch = FindObjectOfType<CharacterUI_EquipmentPanel>();
                if (ch != null)
                {
                    if (ch.comparisonPanel.activeSelf == true)
                        ch.CloseComparisonPanel();
                }
            }
        }

        public void ChangeBackgroundColor()
        {
            //장착, 미장착 상태인 무기를 구별하기 위해 버튼 색상을 바꾼다.
            //임시적인 방안이므로, 나중에 다른 방법을 사용할 수 있습니다.
            if (isArmed)
            {
                itemBtn.GetComponent<Image>().color = Color.cyan;
                return;
            }
            else if (isSelect)
                itemBtn.GetComponent<Image>().color = Color.green;
            else if (!isSelect)
                itemBtn.GetComponent<Image>().color = Color.white;
        }
    }
}
