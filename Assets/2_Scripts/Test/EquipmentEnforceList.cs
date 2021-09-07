using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace SG
{
    public class EquipmentEnforceList : InventoryList
    {
        [SerializeField] private ItemType selectEquipType;


        [SerializeField] private Transform equipmentEnforceSlotsParent;
        [SerializeField] internal EnforceItemSlot[] enforceItemSlots;
        [SerializeField] private TMP_Dropdown itemSort_Dropdown;

        [Header("Prefab")]
        [SerializeField] private EnforceItemSlot enforceItemSlotPrefab;

        [Header("Need Component")]
        private EnforceWindowUI enforceWindowUI;
        public override void Init()
        {
            enforceWindowUI = GetComponentInParent<EnforceWindowUI>();
            equipmentEnforceSlotsParent = transform.Find("Inventory Slot Parent");
            enforceItemSlots = GetComponentsInChildren<EnforceItemSlot>(true);
            itemSort_Dropdown = GetComponentInChildren<TMP_Dropdown>(true);
            if (itemSort_Dropdown != null)
            {
                InitDropdown();
                itemSort_Dropdown.onValueChanged.AddListener(OnClickSortInventoryList);
            }
            enforceItemSlotPrefab = Resources.Load<EnforceItemSlot>("Prefabs/InventorySlots/EnforceItemSlotPrefab");
        }

        public override void OnEnable()
        {
            SortInventoryListToGANADA();
            itemSort_Dropdown.value = 0;
            itemSort_Dropdown.RefreshShownValue();
        }

        public void SetEquipItemTypeToView(ItemType selectType)
        {
            selectEquipType = selectType;
        }


        private void InitDropdown()
        {
            itemSort_Dropdown.ClearOptions();

            List<string> options = new List<string>();
            options.Add("가나다순");
            options.Add("레어도순");

            itemSort_Dropdown.AddOptions(options);
            itemSort_Dropdown.value = 0;
            itemSort_Dropdown.RefreshShownValue();
        }

        private void UpdateUI(ItemType itemType)
        {
            //인벤토리 슬롯이 부족한 경우, 인벤토리 슬롯을 새로 생성하여 추가한다.
            if (enforceItemSlots.Length < PlayerInventory.Instance.equipmentsInventory[itemType].Count)
            {
                int dex = PlayerInventory.Instance.equipmentsInventory[itemType].Count - enforceItemSlots.Length;
                for (int i = 0; i < dex; i++)
                {
                    Instantiate(enforceItemSlotPrefab, equipmentEnforceSlotsParent);
                }
                enforceItemSlots = equipmentEnforceSlotsParent.GetComponentsInChildren<EnforceItemSlot>(true);
            }
            else
            { //인벤토리 슬롯이 너무 많아서 잉여분이 생긴다면 파괴시키는 대신 비활성화를 해주는 것이 좋겠다.
                int diff = enforceItemSlots.Length - PlayerInventory.Instance.equipmentsInventory[itemType].Count;
                for (int i = 0; i < diff; i++)
                {
                    enforceItemSlots[enforceItemSlots.Length - 1 - i].ClearEnforceItemSlot();
                }
            }

            int count = 0;
            foreach (EquipItem item in PlayerInventory.Instance.equipmentsInventory[itemType])
            {
                enforceItemSlots[count].SetEnforceItemSlot(item);
                enforceItemSlots[count].AddBtnListener(() => OnClickEnforceItemSlot(item));
                count++;
            }

            SetAllSlotsDeselect();
        }

        private void OnClickEnforceItemSlot(EquipItem equipItem)
        {
            enforceWindowUI.enforceUI_RightPanel.gameObject.SetActive(true);
            enforceWindowUI.enforceUI_RightPanel.SetRightPanel(equipItem);
        }
        public void SetAllSlotsDeselect()
        {
            foreach (EnforceItemSlot slot in enforceItemSlots)
            {
                slot.isSelect = false;
                slot.ChangeBackgroundColor();
            }
        }

        internal override void OnClickSortInventoryList(int value)
        {
            switch (value)
            {
                case 0:
                    SortInventoryListToGANADA();
                    break;
                case 1:
                    SortInventoryListToRarity();
                    break;
            }

            //equipPanel.SetIndividualEquipItemInformation(playerInventory.currentWeapon);
            //if (equipPanel.comparisonPanel.activeSelf == true)
            //    equipPanel.comparisonPanel.SetActive(false);
        }

        private void SortInventoryListToGANADA()
        {
            PlayerInventory.Instance.SortEquipmentInventoryListToGANADA(selectEquipType);
            UpdateUI(selectEquipType);
        }

        private void SortInventoryListToRarity()
        {
            PlayerInventory.Instance.SortEquipmentInventoryListToRarity(selectEquipType);
            UpdateUI(selectEquipType);
        }

    }
}
