using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    //무기 인벤토리 리스트. 플레이어가 가지고 있는 무기 인벤토리를 불러와 prefab을 생성하고 출력합니다.
    public class WeaponInventoryList : InventoryList
    {
        private Transform weaponInventorySlotsParent;
        [SerializeField] internal WeaponInventorySlot[] weaponInventorySlots;
        private PlayerInventory playerInventory;
        public void Init()
        {
            weaponInventorySlotsParent = transform.Find("Inventory Slot Parent");
            weaponInventorySlots = GetComponentsInChildren<WeaponInventorySlot>(true);
            playerInventory = FindObjectOfType<PlayerInventory>();
        }

        public void OnEnable()
        {
            UpdateUI();
        }

        public void DeSelectAllSlots()
        {
            foreach (WeaponInventorySlot slot in weaponInventorySlots)
            {
                slot.isSelect = false;
                slot.ChangeBackgroundColor();
            }
        }

        // 무기 아이템 아이콘을 불러와 보여준다. 현재 장착중인 아이템이 가장 먼저 실행.
        public void UpdateUI()
        {

            if(weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
            {
                int dex = playerInventory.weaponsInventory.Count - weaponInventorySlots.Length;
                for (int i = 0; i < dex; i++)
                {
                    Instantiate(Resources.Load<WeaponInventorySlot>("Prefabs/InventorySlots/WeaponInventorySlotPrefab")
                                    , weaponInventorySlotsParent);
                }
                weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>(true);
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
        }
    }

}
