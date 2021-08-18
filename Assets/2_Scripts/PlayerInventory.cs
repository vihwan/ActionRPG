using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [System.Serializable]
    public class DictionaryOfEquipment : SerializableDictionary<ItemType, EquipItem> { }


    //플레이어가 소지하고 있는 아이템을 저장하고 관리하는 인벤토리 스크립트 입니다.
    public class PlayerInventory : MonoBehaviour
    {
        [Header("Current Equipping")]
        public WeaponItem currentWeapon;
        // public WeaponItem leftWeapon;
        public WeaponItem unarmedWeapon;
        public EquipItem[] currentEquipmentSlots = new EquipItem[6];

        public WeaponItem[] weaponInRightHandSlots = new WeaponItem[1];
        public WeaponItem[] weaponInLeftHandSlots = new WeaponItem[1];

        //public int currentRightWeaponIndex = -1;
        //public int currentLeftWeaponIndex = -1;

        [Header("Inventory")]
        public List<WeaponItem> weaponsInventory;
        public DictionaryOfEquipment equipmentsInventory = new DictionaryOfEquipment();

        //Need Component
        private WeaponSlotManager weaponSlotManager;

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            currentWeapon = weaponInRightHandSlots[0];
            // leftWeapon = weaponInLeftHandSlots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(currentWeapon, false);
            // weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);

            equipmentsInventory.Add(ItemType.Tops, new EquipItem());
        }

        //[System.Obsolete]
        //        public void ChangeRightWeapon()
        //        {
        //            currentRightWeaponIndex++;

        //            if (currentRightWeaponIndex == 0 && weaponInRightHandSlots[0] != null)
        //            {
        //                rightWeapon = weaponInRightHandSlots[currentRightWeaponIndex];
        //                weaponSlotManager.LoadWeaponOnSlot(weaponInRightHandSlots[currentRightWeaponIndex], false);
        //            }
        //            else if (currentRightWeaponIndex == 0 && weaponInRightHandSlots[0] == null)
        //            {
        //                currentRightWeaponIndex++;
        //            }
        //            else if (currentRightWeaponIndex == 1 && weaponInRightHandSlots[1] != null)
        //            {
        //                rightWeapon = weaponInRightHandSlots[currentRightWeaponIndex];
        //                weaponSlotManager.LoadWeaponOnSlot(weaponInRightHandSlots[currentRightWeaponIndex], false);
        //            }
        //            else
        //            {
        //                currentRightWeaponIndex++;
        //            }

        //            if( currentRightWeaponIndex > weaponInRightHandSlots.Length - 1)
        //            {
        //                currentRightWeaponIndex = -1;
        //                rightWeapon = unarmedWeapon;
        //                weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
        //            }
        //        }
    }
}
