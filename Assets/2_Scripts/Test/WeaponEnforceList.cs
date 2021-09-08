using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace SG
{
    public class WeaponEnforceList : InventoryList
    {
        [SerializeField] private Transform weaponEnforceSlotsParent;
        [SerializeField] internal EnforceItemSlot[] enforceItemSlots;
        [SerializeField] private TMP_Dropdown itemSort_Dropdown;

        [Header("Prefab")]
        [SerializeField] private EnforceItemSlot enforceItemSlotPrefab;

        [Header("Current Select Slot")]
        [SerializeField] private EnforceItemSlot currentSelectSlot;

        [Header("Need Component")]
        private EnforceWindowUI enforceWindowUI;

        public EnforceItemSlot CurrentSelectSlot { get => currentSelectSlot; private set => currentSelectSlot = value; }

        public override void Init()
        {
            weaponEnforceSlotsParent = transform.Find("Inventory Slot Parent");
            enforceItemSlots = GetComponentsInChildren<EnforceItemSlot>(true);
            itemSort_Dropdown = GetComponentInChildren<TMP_Dropdown>(true);
            if (itemSort_Dropdown != null)
            {
                InitDropdown();
                itemSort_Dropdown.onValueChanged.AddListener(OnClickSortInventoryList);
            }
            enforceWindowUI = GetComponentInParent<EnforceWindowUI>();
            enforceItemSlotPrefab = Resources.Load<EnforceItemSlot>("Prefabs/InventorySlots/EnforceItemSlotPrefab");
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

        public override void OnEnable()
        {
            SortInventoryListToGANADA();
            itemSort_Dropdown.value = 0;
            itemSort_Dropdown.RefreshShownValue();
        }

        public void UpdateUI()
        {
            //인벤토리 슬롯이 부족한 경우, 인벤토리 슬롯을 새로 생성하여 추가한다.
            if (enforceItemSlots.Length < PlayerInventory.Instance.weaponsInventory.Count)
            {
                int diff = PlayerInventory.Instance.weaponsInventory.Count - enforceItemSlots.Length;
                for (int i = 0; i < diff; i++)
                {
                    Instantiate(enforceItemSlotPrefab, weaponEnforceSlotsParent);
                }
                enforceItemSlots = weaponEnforceSlotsParent.GetComponentsInChildren<EnforceItemSlot>(true);
            }
            else
            { //인벤토리 슬롯이 너무 많아서 잉여분이 생긴다면 파괴시키는 대신 비활성화를 해주는 것이 좋겠다.
                int diff = enforceItemSlots.Length - PlayerInventory.Instance.weaponsInventory.Count;
                for (int i = 0; i < diff; i++)
                {
                    enforceItemSlots[enforceItemSlots.Length - 1 - i].ClearEnforceItemSlot();
                }
            }

            //위의 생성된 슬롯들을 토대로 인벤토리에서 가져와 아이템을 세팅
            int count = 0;
            foreach (WeaponItem item in PlayerInventory.Instance.weaponsInventory)
            {
                EnforceItemSlot slot = enforceItemSlots[count];
                slot.SetEnforceItemSlot(item);
                slot.SetBtnListener(() => OnClickEnforceItemSlot(slot, item));
                count++;
            }

            SetAllSlotsDeselect();
        }
        private void OnClickEnforceItemSlot(EnforceItemSlot slot, WeaponItem weaponItem)
        {
            CurrentSelectSlot = slot;
            SetAllSlotsDeselect();
            slot.SetIsSelectSlot(true);
            enforceWindowUI.enforceUI_RightPanel.gameObject.SetActive(true);
            enforceWindowUI.enforceUI_RightPanel.SetRightPanel(weaponItem);
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

            //weaponPanel.SetParameter(PlayerInventory.Instance.currentWeapon);
            //if (weaponPanel.comparisonPanel.activeSelf == true)
            //    weaponPanel.comparisonPanel.SetActive(false);
        }

        private void SortInventoryListToGANADA()
        {
            PlayerInventory.Instance.SortWeaponInventoryListToGANADA();
            UpdateUI();
        }

        private void SortInventoryListToRarity()
        {
            PlayerInventory.Instance.SortWeaponInventoryListToRarity();
            UpdateUI();
        }
    }
}
