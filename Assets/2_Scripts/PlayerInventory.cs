using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace SG
{
    public enum EquipType
    {
        Tops,
        Bottoms,
        Gloves,
        Shoes,
        Accessory,
        SpecialEquip
    }

    //Dictionary의 키를 enum타입으로 사용하면, 평소보다 메모리 소모가 심하기 때문에 가비지 콜렉터 호출이 심해집니다
    //이를 해결하기 위해 EnumComparer 구조체를 생성하고 Dictionary에 사용하면, GC의 호출을 최소화시킬 수 있습니다.
    public struct ItemTypeEnumComparer : IEqualityComparer<ItemType>
    {
        public bool Equals(ItemType x, ItemType y)
        {
            return x == y;
        }

        public int GetHashCode(ItemType obj)
        { // you need to do some thinking here

            return (int)obj;
        }
    }


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
        public List<EquipItem> topsInventory;
        public List<EquipItem> bottomsInventory;
        public List<EquipItem> glovesInventory;
        public List<EquipItem> shoesInventory;
        public List<EquipItem> accessoryInventory;
        public List<EquipItem> specialEquipInventory;

        public Dictionary<ItemType, List<EquipItem>> equipmentsInventory;

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

            //무기 초기화
            if (currentWeapon == null)
            {
                currentWeapon = Resources.Load<WeaponItem>("Scriptable/DragonBlade");
                currentWeapon.isArmed = true;
                weaponsInventory.Insert(0, currentWeapon); //List에서, 해당 위치에 넣는 함수 해당위치에 넣으면 뒷부분은 알아서 밀린다.
            }

            SettingEquipmentInventoryList();

            foreach (KeyValuePair<ItemType, List<EquipItem>> pair in equipmentsInventory)
            {
                EquipItemIsArmedInitailize(pair.Value);
            }

            //장비들 초기화
            foreach (EquipItem item in currentEquipmentSlots)
            {
                item.isArmed = true;
                equipmentsInventory[item.itemType].Insert(0, item);
            }

            weaponSlotManager.LoadWeaponOnSlot(currentWeapon, false);
        }

        private void EquipItemIsArmedInitailize(List<EquipItem> list)
        {
            foreach (EquipItem item in list)
            {
                item.isArmed = false;
            }
        }

        private void SettingEquipmentInventoryList()
        {
            equipmentsInventory = new Dictionary<ItemType, List<EquipItem>>(new ItemTypeEnumComparer());

            equipmentsInventory.Add(ItemType.Tops, topsInventory);
            equipmentsInventory.Add(ItemType.Bottoms, bottomsInventory);
            equipmentsInventory.Add(ItemType.Gloves, glovesInventory);
            equipmentsInventory.Add(ItemType.Shoes, shoesInventory);
            equipmentsInventory.Add(ItemType.Accessory, accessoryInventory);
            equipmentsInventory.Add(ItemType.SpecialEquip, specialEquipInventory);
        }

        //무기를 교체하는 함수
        //현재 무기와 교체하고자 하는 무기를 갱신합니다.
        //이에 따라 플레이어 스테이터스도 조정됩니다.
        public void ChangeCurrentWeapon(WeaponItem weaponItem)
        {
            //무기를 벗으면, 벗은 만큼 수치를 감소
            currentWeapon.isArmed = false;
            playerStats.UpdatePlayerStatus_UnEquip(currentWeapon);

            currentWeapon = weaponItem;
            currentWeapon.isArmed = true;
            playerStats.UpdatePlayerStatus_Equip(currentWeapon);
            playerStats.SetMaxHealthBar();
            weaponSlotManager.LoadWeaponOnSlot(currentWeapon, false);
        }

        //장비를 교체하는 함수
        //바꾸고자 하는 장비의 타입을 가져와 이에 따라 알맞는 장비 슬롯을 찾아 장비를 갱신합니다.
        //이에 따라 플레이어 스테이터스도 조정됩니다.


        public void ChangeCurrentEquipment(EquipItem equipItem)
        {
            /**
             ############# 주의 ###############

            ItemType의 첫번째 원소는 Weapon이므로
            장비 Type은 1부터 시작이다.
             
            따라서, currentEquipmentSlots에 맞게 하기 위해서 -1을 해줄 필요가 있다.
             */

            ItemType temp = equipItem.itemType;

            currentEquipmentSlots[(int)temp].isArmed = false;
            playerStats.UpdatePlayerStatus_UnEquip(currentEquipmentSlots[(int)temp]);

            currentEquipmentSlots[(int)temp] = equipItem;
            currentEquipmentSlots[(int)temp].isArmed = true;
            playerStats.UpdatePlayerStatus_Equip(currentEquipmentSlots[(int)temp]);
            playerStats.SetMaxHealthBar();
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

        public void SortEquipmentInventory(EquipItem[] equipItems, ItemType itemType)
        {
            if (equipmentsInventory[itemType].Contains(equipItems[(int)itemType]))
            {
                equipmentsInventory[itemType].Remove(equipItems[(int)itemType]);
                equipmentsInventory[itemType].Insert(0, equipItems[(int)itemType]);
            }
        }

        public void SortWeaponInventoryListToGANADA()
        {
            SortWeaponInventory(currentWeapon);
            List<WeaponItem> tempList = weaponsInventory.Skip(1).ToList();
            tempList = tempList.OrderBy(weaponItem => weaponItem.itemName).ToList();

            for (int i = 0; i < tempList.Count; i++)
            {
                weaponsInventory[i + 1] = tempList[i];
                //Debug.Log(tempList[i].itemName);
            }
        }

        public void SortWeaponInventoryListToRarity()
        {
            SortWeaponInventory(currentWeapon);
            List<WeaponItem> tempList = weaponsInventory.Skip(1).ToList();
            tempList = tempList.OrderByDescending(weaponItem => weaponItem.rarity).ToList();

            for (int i = 0; i < tempList.Count; i++)
            {
                weaponsInventory[i + 1] = tempList[i];
                //Debug.Log(tempList[i].itemName + "의 레어도 : " + tempList[i].rarity);
            }
        }

        public void SortEquipmentInventoryListToGANADA(ItemType itemType)
        {
            SortEquipmentInventory(currentEquipmentSlots, itemType);
            List<EquipItem> tempList = equipmentsInventory[itemType].Skip(1).ToList();
            tempList = tempList.OrderBy(equipItem => equipItem.itemName).ToList();

            equipmentsInventory[itemType].RemoveRange(1, equipmentsInventory[itemType].Count - 1);
            for (int i = 0; i < tempList.Count; i++)
            {
                equipmentsInventory[itemType].Add(tempList[i]);
                Debug.Log(tempList[i].itemName);
            }
        }

        public void SortEquipmentInventoryListToRarity(ItemType itemType)
        {
            SortEquipmentInventory(currentEquipmentSlots, itemType);
            List<EquipItem> tempList = equipmentsInventory[itemType].Skip(1).ToList();
            tempList = tempList.OrderByDescending(equipItem => equipItem.rarity).ToList();

            equipmentsInventory[itemType].RemoveRange(1, equipmentsInventory[itemType].Count - 1);
            for (int i = 0; i < tempList.Count; i++)
            {
                equipmentsInventory[itemType].Add(tempList[i]);
                Debug.Log(tempList[i].itemName);
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
