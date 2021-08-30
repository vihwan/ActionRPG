using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;

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

            //아이템 생성 및 초기화
            LoadWeaponInventoryList();
            LoadEquipmentInventoryList();
            LoadConsumableInventoryList();
            LoadIngredientInventoryList();

            //개별 장비 리스트를 딕셔너리에 추가
            AddEquipmentInventoryListToDictionary();

            weaponSlotManager.LoadWeaponOnSlot(currentWeapon, false);
        }

        //private void OnValidate()
        //{
        //    if (currentWeapon != null)
        //        currentWeapon.isArmed = true;

        //    for (int i = 0; i < currentEquipmentSlots.Length; i++)
        //    {
        //        if (currentEquipmentSlots[i] != null)
        //            currentEquipmentSlots[i].isArmed = true;
        //    }

        //    if (currentConsumable != null)
        //        currentConsumable.isArmed = true;
        //}
        private void LoadWeaponInventoryList()
        {
            WeaponItem[] items;
            items = Resources.LoadAll<WeaponItem>("Scriptable/Weapon");
            for (int i = 0; i < items.Length; i++)
            {
                WeaponItem weaponItem = Instantiate(items[i]);
                weaponsInventory.Add(weaponItem);
                weaponItem.isArmed = false;
            }

            //기본무기를 드래곤블레이드로 - "임시"
            for (int i = 0; i < weaponsInventory.Count; i++)
            {
                if (weaponsInventory[i].itemName == "드래곤 블레이드")
                {
                    currentWeapon = weaponsInventory[i];
                    currentWeapon.isArmed = true;
                }
            }

            if (currentWeapon == null)
                Debug.LogWarning("무기를 찾지 못했다.");
        }
        private void LoadEquipmentInventoryList()
        {
            EquipItem[] items;
            items = Resources.LoadAll<EquipItem>("Scriptable/Equipment/Tops");
            for (int i = 0; i < items.Length; i++)
            {
                EquipItem equipItem = Instantiate(items[i]);
                topsInventory.Add(equipItem);
                equipItem.isArmed = false;
            }

            items = Resources.LoadAll<EquipItem>("Scriptable/Equipment/Bottoms");
            for (int i = 0; i < items.Length; i++)
            {
                EquipItem equipItem = Instantiate(items[i]);
                bottomsInventory.Add(equipItem);
                equipItem.isArmed = false;
            }

            items = Resources.LoadAll<EquipItem>("Scriptable/Equipment/Gloves");
            for (int i = 0; i < items.Length; i++)
            {
                EquipItem equipItem = Instantiate(items[i]);
                glovesInventory.Add(equipItem);
                equipItem.isArmed = false;
            }

            items = Resources.LoadAll<EquipItem>("Scriptable/Equipment/Shoes");
            for (int i = 0; i < items.Length; i++)
            {
                EquipItem equipItem = Instantiate(items[i]);
                shoesInventory.Add(equipItem);
                equipItem.isArmed = false;
            }

            items = Resources.LoadAll<EquipItem>("Scriptable/Equipment/Accessory");
            for (int i = 0; i < items.Length; i++)
            {
                EquipItem equipItem = Instantiate(items[i]);
                accessoryInventory.Add(equipItem);
                equipItem.isArmed = false;
            }

            items = Resources.LoadAll<EquipItem>("Scriptable/Equipment/SpecialEquip");
            for (int i = 0; i < items.Length; i++)
            {
                EquipItem equipItem = Instantiate(items[i]);
                specialEquipInventory.Add(equipItem);
                equipItem.isArmed = false;
            }

            //현재 장비 세팅
            currentEquipmentSlots[0] = topsInventory[0];
            currentEquipmentSlots[1] = bottomsInventory[0];
            currentEquipmentSlots[2] = glovesInventory[0];
            currentEquipmentSlots[3] = shoesInventory[0];
            currentEquipmentSlots[4] = accessoryInventory[0];
            currentEquipmentSlots[5] = specialEquipInventory[0];

            for (int i = 0; i < currentEquipmentSlots.Length; i++)
            {
                currentEquipmentSlots[i].isArmed = true;
            }
        }
        private void LoadConsumableInventoryList()
        {
            ConsumableItem[] items;
            items = Resources.LoadAll<ConsumableItem>("Scriptable/Consumable");

            for (int i = 0; i < items.Length; i++)
            {
                ConsumableItem consumeItem = Instantiate(items[i]);
                consumableInventory.Add(consumeItem);
                consumeItem.isArmed = false;
                consumeItem.quantity = Random.Range(1, 6);
            }

            currentConsumable = consumableInventory[0];
            currentConsumable.isArmed = true;
        }
        private void LoadIngredientInventoryList()
        {
            IngredientItem[] items;
            items = Resources.LoadAll<IngredientItem>("Scriptable/Ingredient");

            for (int i = 0; i < items.Length; i++)
            {
                IngredientItem ingredientItem = Instantiate(items[i]);
                ingredientInventory.Add(ingredientItem);
                ingredientItem.quantity = Random.Range(1, 30);
            }
        }
        private void AddEquipmentInventoryListToDictionary()
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

        //아이템 획득 시 처리하는 함수
        public void SaveGetItemToInventory()
        {

        }

        //아이템 버릴 시(삭제 시) 처리하는 함수
        //객체 파괴와 동시에 Inventory를 정리


        #region Delete Items
        public void SaveDeleteItemToInventory(Item item, int count = 0)
        {
            switch (item.itemType)
            {
                case ItemType.Tops: SaveDeleteItemToInventory(item as EquipItem); break;
                case ItemType.Bottoms: SaveDeleteItemToInventory(item as EquipItem); break;
                case ItemType.Gloves: SaveDeleteItemToInventory(item as EquipItem); break;
                case ItemType.Shoes: SaveDeleteItemToInventory(item as EquipItem); break;
                case ItemType.Accessory: SaveDeleteItemToInventory(item as EquipItem); break;
                case ItemType.SpecialEquip: SaveDeleteItemToInventory(item as EquipItem); break;
                case ItemType.Weapon: SaveDeleteItemToInventory(item as WeaponItem); break;
                case ItemType.Consumable: SaveDeleteItemToInventory(item as ConsumableItem, count); break;
                case ItemType.Ingredient: SaveDeleteItemToInventory(item as IngredientItem, count); break;
                default: break;
            }
        }
        public void SaveDeleteItemToInventory(WeaponItem weaponItem)
        {
            if(GetItemIsArmed(weaponItem))
            {
                Debug.Log("장착중인 아이템은 버릴 수 없다.");
                return;
            }

            for (int i = 0; i < weaponsInventory.Count; i++)
            {
                if (weaponsInventory[i] == weaponItem)
                {
                    
                    Destroy(weaponsInventory[i]);
                    weaponsInventory.RemoveAt(i);
                    // weaponsInventory.RemoveAll(item => item = null);
                    return;
                }
            }
            Debug.Log("아이템 제거 완료");
        }
        public void SaveDeleteItemToInventory(EquipItem equipItem)
        {
            if (GetItemIsArmed(equipItem))
            {
                Debug.Log("장착중인 아이템은 버릴 수 없다.");
                return;
            }

            for (int i = 0; i < equipmentsInventory[equipItem.itemType].Count; i++)
            {
                if (equipmentsInventory[equipItem.itemType][i] == equipItem)
                {
                    //equipmentsInventory[equipItem.itemType].Remove(equipmentsInventory[equipItem.itemType][i]);
                    Destroy(equipmentsInventory[equipItem.itemType][i]);
                    equipmentsInventory[equipItem.itemType].RemoveAt(i);
                }
            }
        }
        public void SaveDeleteItemToInventory(ConsumableItem consumableItem, int count)
        {
            if(GetItemIsArmed(consumableItem))
            {
                Debug.Log("장착중인 아이템은 버릴 수 없다.");
                return;
            }

            for (int i = 0; i < consumableInventory.Count; i++)
            {
                if (consumableInventory[i] == consumableItem)
                {
                    consumableInventory[i].quantity -= count;

                    if (consumableInventory[i].quantity <= 0)
                    {
                        //consumableInventory.Remove(equipmentsInventory[equipItem.itemType][i]);
                        Destroy(consumableInventory[i]);
                        consumableInventory.RemoveAt(i);
                    }

                }
            }
        }
        public void SaveDeleteItemToInventory(IngredientItem ingredientItem, int count)
        {
            for (int i = 0; i < ingredientInventory.Count; i++)
            {
                if (ingredientInventory[i] == ingredientItem)
                {
                    ingredientInventory[i].quantity -= count;

                    if (ingredientInventory[i].quantity <= 0)
                    {
                        //ingredientInventory.Remove(equipmentsInventory[equipItem.itemType][i]);
                        Destroy(ingredientInventory[i]);
                        ingredientInventory.RemoveAt(i);
                    }
                }
            }
        }
        #endregion

        public bool GetItemIsArmed(WeaponItem weaponItem)
        {
            return weaponItem.isArmed;
        }
        public bool GetItemIsArmed(EquipItem equipItem)
        {
            return equipItem.isArmed;
        }
        public bool GetItemIsArmed(ConsumableItem consumableItem)
        {
            return consumableItem.isArmed;
        }

        #region Inventory Sorting Methods
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
            if (weaponsInventory.Count == 0)
                return;

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
            if (weaponsInventory.Count == 0)
                return;

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
            if (equipmentsInventory[itemType].Count == 0)
                return;

            SortEquipmentInventory(currentEquipmentSlots, itemType);
            List<EquipItem> tempList = equipmentsInventory[itemType].Skip(1).ToList();
            tempList = tempList.OrderBy(equipItem => equipItem.itemName).ToList();

            equipmentsInventory[itemType].RemoveRange(1, equipmentsInventory[itemType].Count - 1);
            for (int i = 0; i < tempList.Count; i++)
            {
                equipmentsInventory[itemType].Add(tempList[i]);
            }
        }

        public void SortEquipmentInventoryListToRarity(ItemType itemType)
        {
            if (equipmentsInventory[itemType].Count == 0)
                return;

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
            if (consumableInventory.Count == 0)
                return;

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
            if (consumableInventory.Count == 0)
                return;

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
            if (ingredientInventory.Count == 0)
                return;

            List<IngredientItem> tempList = ingredientInventory.ToList();
            tempList = tempList.OrderBy(ingredientItem => ingredientItem.itemName).ToList();

            for (int i = 0; i < tempList.Count; i++)
            {
                ingredientInventory[i] = tempList[i];
            }
        }

        public void SortIngredientInventoryListToRarity()
        {
            if (ingredientInventory.Count == 0)
                return;

            List<IngredientItem> tempList = ingredientInventory.ToList();
            tempList = tempList.OrderByDescending(ingredientItem => ingredientItem.rarity).ToList();

            for (int i = 0; i < tempList.Count; i++)
            {
                ingredientInventory[i] = tempList[i];
            }
        }
        #endregion
    }
}
