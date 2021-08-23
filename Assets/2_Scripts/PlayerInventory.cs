using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SG
{
    [System.Serializable]
    public class DictionaryOfEquipment : SerializableDictionary<ItemType, EquipItem> { }


    //플레이어가 소지하고 있는 아이템을 저장하고 관리하는 인벤토리 스크립트 입니다.
    public class PlayerInventory : MonoBehaviour
    {
        [Header("Current Equipping")]
        public WeaponItem currentWeapon;
        public WeaponItem unarmedWeapon;
        public EquipItem[] currentEquipmentSlots = new EquipItem[6];
        //소비템칸 하나

        [Header("Inventory")]
        public List<WeaponItem> weaponsInventory;
        public DictionaryOfEquipment equipmentsInventory = new DictionaryOfEquipment();

        //Need Component
        private WeaponSlotManager weaponSlotManager;
        private PlayerStats playerStats;

        private void Awake()
        {
            //Default Weapon Set
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            playerStats = GetComponent<PlayerStats>();
            if (playerStats == null)
                Debug.LogWarning("PlayerStats가 참조되지 않았습니다.");

            //무기 인벤토리의 무기들 isArmed를 false로 초기화
            foreach (WeaponItem item in weaponsInventory)
            {
                item.isArmed = false;
            }

            if (currentWeapon == null)
            {
                currentWeapon = Resources.Load<WeaponItem>("Scriptable/DragonBlade");
                currentWeapon.isArmed = true;
                weaponsInventory.Insert(0, currentWeapon); //List에서, 해당 위치에 넣는 함수 해당위치에 넣으면 뒷부분은 알아서 밀린다.
            }
            weaponSlotManager.LoadWeaponOnSlot(currentWeapon, false);
        }

        public void ChangeCurrentWeapon(WeaponItem weaponItem)
        {
            //무기를 벗으면, 벗은 만큼 수치를 감소
            currentWeapon.isArmed = false;
            playerStats.UpdatePlayerStatus_UnEquip(currentWeapon);

            currentWeapon = weaponItem;
            currentWeapon.isArmed = true;
            playerStats.UpdatePlayerStatus_Equip(currentWeapon);
            weaponSlotManager.LoadWeaponOnSlot(currentWeapon, false);
        }


        //LeftPanel_WeaponInventory가 닫힐 때, 실행하는 것이 좋다.
        public void SortWeaponInventory(WeaponItem weaponItem)
        {
            if (weaponsInventory.Contains(weaponItem))
            {
                weaponsInventory.Remove(weaponItem);
                weaponsInventory.Insert(0, weaponItem);
            }
        }

        public void SortInventoryListToGANADA()
        {
            SortWeaponInventory(currentWeapon);
            List<WeaponItem> tempList = weaponsInventory.Skip(1).ToList();
            tempList = tempList.OrderBy(weaponItem => weaponItem.itemName).ToList();

            for (int i = 0; i < tempList.Count; i++)
            {
                weaponsInventory[i + 1] = tempList[i];
                Debug.Log(tempList[i].itemName);
            }
        }

        public void SortInventoryListToRarity()
        {
            SortWeaponInventory(currentWeapon);
            List<WeaponItem> tempList = weaponsInventory.Skip(1).ToList();
            tempList = tempList.OrderByDescending(weaponItem => weaponItem.rarity).ToList();

            for (int i = 0; i < tempList.Count; i++)
            {
                weaponsInventory[i + 1] = tempList[i];
                Debug.Log(tempList[i].itemName + "의 레어도 : " + tempList[i].rarity);
            }
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
