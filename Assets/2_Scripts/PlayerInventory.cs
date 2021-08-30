using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace SG
{
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
        public ConsumableItem currentConsumable;
        //소비템칸 하나

        [Header("Inventory")]
        public List<WeaponItem> weaponsInventory;
        public List<EquipItem> topsInventory;
        public List<EquipItem> bottomsInventory;
        public List<EquipItem> glovesInventory;
        public List<EquipItem> shoesInventory;
        public List<EquipItem> accessoryInventory;
        public List<EquipItem> specialEquipInventory;
        public List<ConsumableItem> consumableInventory;
        public List<IngredientItem> ingredientInventory;

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
            SettingConsumableInventoryList();
            SettingIngredientInventoryList();


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

        private void SettingConsumableInventoryList()
        {
            ConsumableItem[] items;
            items = Resources.LoadAll<ConsumableItem>("Scriptable/Consumable");

            for (int i = 0; i < items.Length; i++)
            {
                consumableInventory.Add(items[i]);
                items[i].isArmed = false;
            }

            currentConsumable = consumableInventory[0];
            currentConsumable.isArmed = true;
        }

        private void SettingIngredientInventoryList()
        {
            IngredientItem[] items;
            items = Resources.LoadAll<IngredientItem>("Scriptable/Ingredient");

            for (int i = 0; i < items.Length; i++)
            {
                ingredientInventory.Add(items[i]);
            }
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
            ItemType temp = equipItem.itemType;

            currentEquipmentSlots[(int)temp].isArmed = false;
            playerStats.UpdatePlayerStatus_UnEquip(currentEquipmentSlots[(int)temp]);

            currentEquipmentSlots[(int)temp] = equipItem;
            currentEquipmentSlots[(int)temp].isArmed = true;
            playerStats.UpdatePlayerStatus_Equip(currentEquipmentSlots[(int)temp]);
            playerStats.SetMaxHealthBar();
        }

        //장착중인 소비 아이템을 교체하는 함수
        public void ChangeCurrentConsumable(ConsumableItem consumableItem)
        {
            currentConsumable.isArmed = false;
            currentConsumable = consumableItem;
            currentConsumable.isArmed = true;
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

        public void SortConsumableInventory(ConsumableItem consumableItem)
        {
            if (consumableInventory.Contains(consumableItem))
            {
                consumableInventory.Remove(consumableItem);
                consumableInventory.Insert(0, consumableItem);
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

        public void SortConsumableInventoryListToGANADA()
        {
            SortConsumableInventory(currentConsumable);
            List<ConsumableItem> tempList = consumableInventory.Skip(1).ToList();
            //List<ConsumableItem> tempList = consumableInventory.ToList();
            tempList = tempList.OrderBy(consumableItem => consumableItem.itemName).ToList();

            for (int i = 0; i < tempList.Count; i++)
            {
                //consumableInventory[i] = tempList[i];
                consumableInventory[i + 1] = tempList[i];
            }
        }

        public void SortConsumableInventoryListToRarity()
        {
            SortConsumableInventory(currentConsumable);
            List<ConsumableItem> tempList = consumableInventory.Skip(1).ToList();
            //List<ConsumableItem> tempList = consumableInventory.ToList();
            tempList = tempList.OrderByDescending(consumableItem => consumableItem.rarity).ToList();

            for (int i = 0; i < tempList.Count; i++)
            {
                //consumableInventory[i] = tempList[i];
                consumableInventory[i + 1] = tempList[i];
            }
        }

        public void SortIngredientInventoryListToGANADA()
        {
            List<IngredientItem> tempList = ingredientInventory.ToList();
            tempList = tempList.OrderBy(ingredientItem => ingredientItem.itemName).ToList();

            for (int i = 0; i < tempList.Count; i++)
            {
                ingredientInventory[i] = tempList[i];
            }
        }

        public void SortIngredientInventoryListToRarity()
        {
            List<IngredientItem> tempList = ingredientInventory.ToList();
            tempList = tempList.OrderByDescending(ingredientItem => ingredientItem.rarity).ToList();

            for (int i = 0; i < tempList.Count; i++)
            {
                ingredientInventory[i] = tempList[i];
            }
        }
    }
}
