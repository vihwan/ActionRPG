using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SG
{
    public class EquipmentInventoryList : InventoryList
    {
        [SerializeField] private ItemType selectEquipType;

        private Transform equipmentInventorySlotsParent;
        [SerializeField] internal EquipmentInventorySlot[] equipmentInventorySlots;
        private TMP_Dropdown itemSort_Dropdown;

        [Header("Need Component")]
        internal PlayerInventory playerInventory;
        internal CharacterUI_EquipmentPanel equipPanel;

        public override void Init()
        {
            equipmentInventorySlotsParent = transform.Find("Inventory Slot Parent");
            equipmentInventorySlots = GetComponentsInChildren<EquipmentInventorySlot>(true);
            itemSort_Dropdown = GetComponentInChildren<TMP_Dropdown>(true);
            if (itemSort_Dropdown != null)
            {
                InitDropdown();
                itemSort_Dropdown.onValueChanged.AddListener(SortInventoryList);
            }

            playerInventory = FindObjectOfType<PlayerInventory>();
            equipPanel = FindObjectOfType<CharacterUI_EquipmentPanel>();
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

        internal override void SortInventoryList(int value)
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
            if (equipPanel.comparisonPanel.activeSelf == true)
                equipPanel.comparisonPanel.SetActive(false);
        }

        private void SortInventoryListToGANADA()
        {
            playerInventory.SortEquipmentInventoryListToGANADA(selectEquipType);
            UpdateUI(selectEquipType);
        }

        private void SortInventoryListToRarity()
        {
            playerInventory.SortEquipmentInventoryListToRarity(selectEquipType);
            UpdateUI(selectEquipType);
        }

        public void UpdateSlots()
        {
            foreach (EquipmentInventorySlot slot in equipmentInventorySlots)
            {
                foreach (EquipItem item in playerInventory.equipmentsInventory[selectEquipType])
                {
                    slot.UpdateSlot(item);
                }
            }

            Debug.Log("장비 아이콘 색상 갱신 완료");
        }

        private void UpdateUI(ItemType itemType)
        {
            //인벤토리 슬롯이 부족한 경우, 인벤토리 슬롯을 새로 생성하여 추가한다.
            if (equipmentInventorySlots.Length < playerInventory.equipmentsInventory[itemType].Count)
            {
                int dex = playerInventory.equipmentsInventory[itemType].Count - equipmentInventorySlots.Length;
                for (int i = 0; i < dex; i++)
                {
                    Instantiate(Resources.Load<EquipmentInventorySlot>("Prefabs/InventorySlots/EquipmentInventorySlotPrefab")
                                    , equipmentInventorySlotsParent);
                }
                equipmentInventorySlots = equipmentInventorySlotsParent.GetComponentsInChildren<EquipmentInventorySlot>(true);
            }

            int count = 0;
            foreach (EquipItem item in playerInventory.equipmentsInventory[itemType])
            {
                equipmentInventorySlots[count].AddItem(item);
                count++;
            }

            DeSelectAllSlots();
        }
        public void DeSelectAllSlots()
        {
            foreach (EquipmentInventorySlot slot in equipmentInventorySlots)
            {
                slot.isSelect = false;
                slot.ChangeBackgroundColor();
            }
        }
    }
}
