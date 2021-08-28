using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SG
{
    //무기 인벤토리 리스트. 플레이어가 가지고 있는 무기 인벤토리를 불러와 prefab을 생성하고 출력합니다.
    public class WeaponInventoryList : InventoryList
    {
        private Transform weaponInventorySlotsParent;
        [SerializeField] internal WeaponInventorySlot[] weaponInventorySlots;
        private TMP_Dropdown itemSort_Dropdown;

        [Header("Need Component")]
        internal PlayerInventory playerInventory;
        internal CharacterUI_WeaponPanel weaponPanel;
        public override void Init()
        {
            weaponInventorySlotsParent = transform.Find("Inventory Slot Parent");
            weaponInventorySlots = GetComponentsInChildren<WeaponInventorySlot>(true);
            itemSort_Dropdown = GetComponentInChildren<TMP_Dropdown>(true);
            if (itemSort_Dropdown != null)
            {
                InitDropdown();
                itemSort_Dropdown.onValueChanged.AddListener(OnClickSortInventoryList);
            }

            playerInventory = FindObjectOfType<PlayerInventory>();
            weaponPanel = FindObjectOfType<CharacterUI_WeaponPanel>();
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

        public void DeSelectAllSlots()
        {
            foreach (WeaponInventorySlot slot in weaponInventorySlots)
            {
                slot.isSelect = false;
                slot.ChangeBackgroundColor();
            }
        }

        public void UpdateSlots()
        {
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                weaponInventorySlots[i].UpdateSlot(playerInventory.weaponsInventory[i]);
            }

            Debug.Log("무기 아이콘 색상 갱신 완료");
        }

        // 무기 아이템 아이콘을 불러와 보여준다. 현재 장착중인 아이템이 가장 먼저 실행.
        public void UpdateUI()
        {
            //인벤토리 슬롯이 부족한 경우, 인벤토리 슬롯을 새로 생성하여 추가한다.
            if(weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
            {
                int diff = playerInventory.weaponsInventory.Count - weaponInventorySlots.Length;
                for (int i = 0; i < diff; i++)
                {
                    Instantiate(Resources.Load<WeaponInventorySlot>("Prefabs/InventorySlots/WeaponInventorySlotPrefab")
                                    , weaponInventorySlotsParent);
                }
                weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>(true);
            }       
            else
            { //인벤토리 슬롯이 너무 많아서 잉여분이 생긴다면 파괴시키는 대신 비활성화를 해주는 것이 좋겠다.
                int diff = weaponInventorySlots.Length - playerInventory.weaponsInventory.Count;
                for (int i = diff; i > 0; i--)
                {
                    weaponInventorySlots[diff].ClearInventorySlot();
                }
            }
           

            for (int i = 0; i < playerInventory.weaponsInventory.Count; i++)
            {
                if (playerInventory.weaponsInventory[i].isArmed)
                {
                    weaponInventorySlots[0].AddItem(playerInventory.weaponsInventory[i]);
                    continue;
                }
                else
                {
                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }
            }

            DeSelectAllSlots();
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

            weaponPanel.SetParameter(playerInventory.currentWeapon);
            if (weaponPanel.comparisonPanel.activeSelf == true)
                weaponPanel.comparisonPanel.SetActive(false);
        }

        private void SortInventoryListToGANADA()
        {
            playerInventory.SortWeaponInventoryListToGANADA();
            UpdateUI();
        }

        private void SortInventoryListToRarity()
        {
            playerInventory.SortWeaponInventoryListToRarity();
            UpdateUI();
        }
    }
}
