using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    //플레이어의 인벤토리를 관리하고 출력하는 패널 스크립트
    // 무기, 소비, 가방 등 여러 인벤토리 창을 관리하는 스크립트입니다.
    public class WindowPanel : MonoBehaviour
    {
        [SerializeField] public GameObject inventoryWindow;

        [Header("Weapon Inventory")]
        private Transform weaponInventorySlotsParent;
        [SerializeField] private WeaponInventorySlot[] weaponInventorySlots;

        [Header("Need Component")]
        private PlayerInventory playerInventory;

        public void Init()
        {
            weaponInventorySlotsParent = transform.Find("Inventory Slot Parent");
            weaponInventorySlots = GetComponentsInChildren<WeaponInventorySlot>(true);

            inventoryWindow = transform.Find("Inventory Window").gameObject;
            if (inventoryWindow != null)
                inventoryWindow.SetActive(false);

            playerInventory = FindObjectOfType<PlayerInventory>();
        }

        public void UpdateUI()
        {
            #region Weapon Inventory Slot
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if(i < playerInventory.weaponsInventory.Count)
                {
                    if(weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                    {
                        Instantiate(Resources.Load<GameObject>("Prefab/InventorySlots/WeaponInventorySlotPrefab")
                                    , weaponInventorySlotsParent);

                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>(true);
                    }
                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
        }
    }
}

