﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    //플레이어의 인벤토리를 관리하고 출력하는 패널 스크립트
    // 무기, 소비, 가방 등 여러 인벤토리 창을 관리하는 스크립트입니다.
    public class WindowPanel : MonoBehaviour
    {
        [Header("Window GameObject")]
        [SerializeField] internal CharacterWindowUI characterWindow;
        [SerializeField] internal InventoryWindowUI inventoryWindow;

        [Header("Weapon Inventory")]
        private Transform weaponInventorySlotsParent;
        [SerializeField] private WeaponInventorySlot[] weaponInventorySlots;


        public void Init()
        {
            weaponInventorySlotsParent = transform.Find("Inventory Slot Parent");
            weaponInventorySlots = GetComponentsInChildren<WeaponInventorySlot>(true);

            characterWindow = UtilHelper.Find<CharacterWindowUI>(transform, "Character Window");
            if (characterWindow != null)
            {
                characterWindow.Init();
                characterWindow.gameObject.SetActive(false);
            }

            inventoryWindow = UtilHelper.Find<InventoryWindowUI>(transform, "Inventory Window");
            if(inventoryWindow != null)
            {
                inventoryWindow.Init();
                inventoryWindow.gameObject.SetActive(false);
            }
        }

        public void UpdateUI()
        {
            #region Weapon Inventory Slot
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if (i < PlayerInventory.Instance.weaponsInventory.Count)
                {
                    if (weaponInventorySlots.Length < PlayerInventory.Instance.weaponsInventory.Count)
                    {
                        Instantiate(Resources.Load<GameObject>("Prefab/InventorySlots/WeaponInventorySlotPrefab")
                                    , weaponInventorySlotsParent);

                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>(true);
                    }
                    weaponInventorySlots[i].AddItem(PlayerInventory.Instance.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }
            #endregion
        }

        #region Panel Window Controls
        public void OpenCharacterWindowPanel()
        {
            characterWindow.gameObject.SetActive(true);
            characterWindow.OpenStatusPanel();
        }

        public void CloseCharacterWindowPanel()
        {
            characterWindow.gameObject.SetActive(false);
        }

        public void OpenInventoryWindowPanel()
        {
            inventoryWindow.gameObject.SetActive(true);
        }

        public void CloseInventoryWindowPanel()
        {
            inventoryWindow.gameObject.SetActive(false);
        }

        #endregion
    }
}

