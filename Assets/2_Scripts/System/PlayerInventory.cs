using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;

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
        public static PlayerInventory Instance;

        [Header("Current Gold")]
        [SerializeField] private int currentGold;

        [Header("Current Equipping")]
        public WeaponItem currentWeapon;
        public WeaponItem unarmedWeapon;
        public EquipItem[] currentEquipmentSlots = new EquipItem[6];
        public ConsumableItem currentConsumableItem;         //소비템칸 하나

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

        //Delegate
        public delegate void UpdateGoldText();
        private event UpdateGoldText updateGoldText;
        public int CurrentGold
        {
            get => currentGold;
            private set
            {
                currentGold = value;
                if (currentGold < 0)
                    currentGold = 0;
            }
        }

        public void Init()
        {
            if(Instance == null)
                Instance = this;

            playerStats = GetComponent<PlayerStats>();
            if (playerStats == null)
                Debug.LogWarning("PlayerStats가 참조되지 않았습니다.");

            //아이템 생성 및 초기화 (임시)
            LoadWeaponInventoryList();
            LoadEquipmentInventoryList();
            LoadConsumableInventoryList();
            LoadIngredientInventoryList();

            //개별 장비 리스트를 딕셔너리에 추가
            AddEquipmentInventoryListToDictionary();

            //Default Weapon Set
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            if (weaponSlotManager != null)
                weaponSlotManager.Init();

            weaponSlotManager.LoadWeaponOnSlot(currentWeapon);
        }

        #region delegate Func
        public void AddUpdateGoldText(UpdateGoldText listener) { updateGoldText += listener; }
        public void InvokeUpdateGoldText() { updateGoldText?.Invoke(); }

        #endregion
        private void LoadWeaponInventoryList()
        {
            for (int i = 0; i < Database.it.ItemDataBase.weaponItems.Count; i++)
            {
                WeaponItem weaponItem = Instantiate(Database.it.GetItemByID(ItemType.Weapon, i)) as WeaponItem;
                weaponItem.quantity = 1;
                weaponItem.isArmed = false;
                weaponsInventory.Add(weaponItem);
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
            for (int i = 0; i < Database.it.ItemDataBase.topsItems.Count; i++)
            {
                EquipItem equipItem = Instantiate(Database.it.GetItemByID(ItemType.Tops, i)) as EquipItem;
                topsInventory.Add(equipItem);
                equipItem.isArmed = false;
            }

            for (int i = 0; i < Database.it.ItemDataBase.bottomsItems.Count; i++)
            {
                EquipItem equipItem = Instantiate(Database.it.GetItemByID(ItemType.Bottoms, i)) as EquipItem;
                bottomsInventory.Add(equipItem);
                equipItem.isArmed = false;
            }

            for (int i = 0; i < Database.it.ItemDataBase.glovesItems.Count; i++)
            {
                EquipItem equipItem = Instantiate(Database.it.GetItemByID(ItemType.Gloves, i)) as EquipItem;
                glovesInventory.Add(equipItem);
                equipItem.isArmed = false;
            }

            for (int i = 0; i < Database.it.ItemDataBase.shoesItems.Count; i++)
            {
                EquipItem equipItem = Instantiate(Database.it.GetItemByID(ItemType.Shoes, i)) as EquipItem;
                shoesInventory.Add(equipItem);
                equipItem.isArmed = false;
            }

            for (int i = 0; i < Database.it.ItemDataBase.accessoryItems.Count; i++)
            {
                EquipItem equipItem = Instantiate(Database.it.GetItemByID(ItemType.Accessory, i)) as EquipItem;
                accessoryInventory.Add(equipItem);
                equipItem.isArmed = false;
            }

            for (int i = 0; i < Database.it.ItemDataBase.specialEquipItems.Count; i++)
            {
                EquipItem equipItem = Instantiate(Database.it.GetItemByID(ItemType.SpecialEquip, i)) as EquipItem;
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
            for (int i = 0; i < 3; i++)
            {
                ConsumableItem consumeItem = Instantiate(Database.it.GetItemByID(ItemType.Consumable, i)) as ConsumableItem;
                consumableInventory.Add(consumeItem);
                consumeItem.isArmed = false;
                consumeItem.quantity = Random.Range(1, 6);
            }

            currentConsumableItem = consumableInventory[0];
            currentConsumableItem.isArmed = true;
        }
        private void LoadIngredientInventoryList()
        {
            for (int i = 0; i < 3; i++)
            {
                IngredientItem ingredientItem = Instantiate(Database.it.GetItemByID(ItemType.Ingredient, i)) as IngredientItem;
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
            playerStats.SetMaxStatusBar();
            weaponSlotManager.LoadWeaponOnSlot(currentWeapon);
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
            playerStats.SetMaxStatusBar();
        }

        //장착중인 소비 아이템을 교체하는 함수
        public void ChangeCurrentConsumable(ConsumableItem consumableItem)
        {
            currentConsumableItem.isArmed = false;
            currentConsumableItem = consumableItem;
            currentConsumableItem.isArmed = true;
        }

        //아이템 획득 시 처리하는 함수
        public void SaveGetItemToInventory(Item item, int count = 1)
        {
            switch (item.itemType)
            {
                case ItemType.Tops: GetItem(item as EquipItem, count); break;
                case ItemType.Bottoms: goto case ItemType.Tops;
                case ItemType.Gloves: goto case ItemType.Tops;
                case ItemType.Shoes: goto case ItemType.Tops;
                case ItemType.Accessory: goto case ItemType.Tops;
                case ItemType.SpecialEquip: goto case ItemType.Tops;
                case ItemType.Weapon: GetItem(item as WeaponItem, count); break;
                case ItemType.Consumable: GetItem(item as ConsumableItem, count); break;
                case ItemType.Ingredient: GetItem(item as IngredientItem, count); break;
            }

            //아이템 팝업 출력
            PopUpGenerator.Instance.GetMessageGetItemObject(item, count);
            //퀵슬롯 현재 장착 소비 아이템 갱신
            GUIManager.it.quickSlotUI.ConsumesSlot.SetActiveBtn(currentConsumableItem);
        }
        private void GetItem(WeaponItem weaponItem, int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                WeaponItem newItem = Instantiate(weaponItem);
                weaponsInventory.Add(newItem);
            }      
        }
        private void GetItem(EquipItem equipItem, int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                EquipItem newItem = Instantiate(equipItem);
                switch (equipItem.itemType)
                {
                    case ItemType.Tops: topsInventory.Add(newItem); break;
                    case ItemType.Bottoms: bottomsInventory.Add(newItem); break;
                    case ItemType.Gloves: glovesInventory.Add(newItem); break;
                    case ItemType.Shoes: shoesInventory.Add(newItem); break;
                    case ItemType.Accessory: accessoryInventory.Add(newItem); break;
                    case ItemType.SpecialEquip: specialEquipInventory.Add(newItem); break;
                }
            }
        }
        private void GetItem(ConsumableItem consumableItem, int count)
        {
            //탐색을 돌려서 만약에 같은 이름의 아이템이 이미 있다면, 갯수만 늘리고 종료
            for (int i = 0; i < consumableInventory.Count; i++)
            {
                if (consumableInventory[i].itemName == consumableItem.itemName)
                {
                    consumableInventory[i].quantity += count;
                    return;
                }
            }

            //탐색을 돌려도 없을 경우, 새로 아이템을 생성하고 인벤토리에 추가.
            ConsumableItem newItem = Instantiate(consumableItem);
            newItem.quantity = count;
            consumableInventory.Add(newItem);
        }
        private void GetItem(IngredientItem ingredientItem, int count)
        {
            //탐색을 돌려서 만약에 같은 이름의 아이템이 이미 있다면, 갯수만 늘리고 종료
            for (int i = 0; i < ingredientInventory.Count; i++)
            {
                if (ingredientInventory[i].itemName == ingredientItem.itemName)
                {
                    ingredientInventory[i].quantity += count;
                    return;
                }
            }

            //탐색을 돌려도 없을 경우, 새로 아이템을 생성하고 인벤토리에 추가.
            IngredientItem newItem = Instantiate(ingredientItem);
            newItem.quantity = count;
            ingredientInventory.Add(newItem);
        }

        public int GetHaveItem(ConsumableItem consumableItem)
        {
            //탐색을 돌려서 만약에 같은 이름의 아이템이 이미 있다면, 갯수만 늘리고 종료
            for (int i = 0; i < consumableInventory.Count; i++)
            {
                if (consumableInventory[i].itemName == consumableItem.itemName)
                {
                    return consumableInventory[i].quantity;
                }
            }

            return 0;
        }

        public int GetHaveItem(IngredientItem ingredientItem)
        {
            //탐색을 돌려서 만약에 같은 이름의 아이템이 이미 있다면, 갯수만 늘리고 종료
            for (int i = 0; i < ingredientInventory.Count; i++)
            {
                if (ingredientInventory[i].itemName == ingredientItem.itemName)
                {
                    return ingredientInventory[i].quantity;
                }
            }
            return 0;
        }

        //골드 획득
        public void GetGold(int money)
        {
            CurrentGold += money;
            PopUpGenerator.Instance.GetMessageGetGold(money);
        }

        public void UseGold(int money)
        {
            CurrentGold -= money;
        }

        public bool HaveGold(int money)
        {
            if (CurrentGold >= money)
                return true;
            else
            {
                return false;
            }
        }

        //소유하고 있는 아이템의 스테이터스를 관리하는 함수들
        #region Status Manage

        //무기, 장비 함수만 적용
        public void SetItemEnforceStatusItem(Item item, int increaseCount)
        {
            switch (item.itemType)
            {
                case ItemType.Tops: SetItemEnforceStatusEquip(item as EquipItem, increaseCount); break;
                case ItemType.Bottoms: goto case ItemType.Tops;
                case ItemType.Gloves: goto case ItemType.Tops;
                case ItemType.Shoes: goto case ItemType.Tops;
                case ItemType.Accessory: goto case ItemType.Tops;
                case ItemType.SpecialEquip: goto case ItemType.Tops;
                case ItemType.Weapon: SetItemEnforceStatusWeapon(item as WeaponItem, increaseCount); break;
                default: break;
            }
        }

        public void SetItemEnforceStatusWeapon(WeaponItem weaponItem, int increaseCount)
        {
            for (int i = 0; i < weaponsInventory.Count; i++)
            {
                if(weaponsInventory[i] == weaponItem)
                {
                    //능력치 증가, 강화단계 상승
                    weaponsInventory[i].IncreaseAttribute(increaseCount);
                    weaponsInventory[i].enforceLevel += 1;
                    return;
                }
            }
        }

        public void SetItemEnforceStatusEquip(EquipItem equipItem, int increaseCount)
        {
            for (int i = 0; i < equipmentsInventory[equipItem.itemType].Count; i++)
            {
                if (equipmentsInventory[equipItem.itemType][i] == equipItem)
                {
                    equipmentsInventory[equipItem.itemType][i].IncreaseAttribute(increaseCount);
                    equipmentsInventory[equipItem.itemType][i].enforceLevel += 1;
                }
            }
        }

        #endregion

        //아이템 버릴 시(삭제 시) 처리하는 함수
        //객체 파괴와 동시에 Inventory를 정리
        #region Delete Items
        public void SaveDeleteItemToInventory(Item item)
        {
            switch (item.itemType)
            {
                case ItemType.Tops:       SaveDeleteItemToInventoryEquip(item as EquipItem); break;
                case ItemType.Bottoms:      goto case ItemType.Tops;
                case ItemType.Gloves:       goto case ItemType.Tops;
                case ItemType.Shoes:        goto case ItemType.Tops;
                case ItemType.Accessory:    goto case ItemType.Tops;
                case ItemType.SpecialEquip: goto case ItemType.Tops;
                case ItemType.Weapon:     SaveDeleteItemToInventoryWeapon(item as WeaponItem); break;
                default: break;
            }
        }
        public void SaveDeleteItemToInventoryConsIngred(Item item, out bool allDelete , int count = 1)
        {
            allDelete = false;
            switch (item.itemType)
            {
                case ItemType.Consumable: allDelete = SaveDeleteItemToInventoryConsumable(item as ConsumableItem, count); break;
                case ItemType.Ingredient: allDelete = SaveDeleteItemToInventoryIngredient(item as IngredientItem, count); break;
                default: break;
            }
        }
        public void SaveDeleteItemToInventoryWeapon(WeaponItem weaponItem)
        {


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
        public void SaveDeleteItemToInventoryEquip(EquipItem equipItem)
        {
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
        public bool SaveDeleteItemToInventoryConsumable(ConsumableItem consumableItem, int count)
        {

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
                        return true;
                    }
                }
            }

            return false;
        }
        public bool SaveDeleteItemToInventoryIngredient(IngredientItem ingredientItem, int count)
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
                        return true;
                    }
                }
            }
            return false;
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

            SortConsumableInventory(currentConsumableItem);
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

            SortConsumableInventory(currentConsumableItem);
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
