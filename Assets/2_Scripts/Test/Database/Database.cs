using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace SG
{
    public class Database : MonoBehaviour
    {
        public static Database Instance;
        public ItemDataBase itemDataBase;
        public PrefabDataBase prefabDatabase;

        public void Init()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
                Destroy(this.gameObject);

            itemDataBase = GetComponentInChildren<ItemDataBase>(true);
            if (itemDataBase != null)
                itemDataBase.Init();

            prefabDatabase = GetComponentInChildren<PrefabDataBase>(true);
            if (prefabDatabase != null)
                prefabDatabase.Init();
        }

        public static Item GetItemByID(ItemType itemType, int id)
        {
            switch (itemType)
            {
                case ItemType.Tops:
                    return Instance.itemDataBase.topsItems.FirstOrDefault(i => i.itemId == id);
                case ItemType.Bottoms:
                    return Instance.itemDataBase.bottomsItems.FirstOrDefault(i => i.itemId == id);
                case ItemType.Gloves:
                    return Instance.itemDataBase.glovesItems.FirstOrDefault(i => i.itemId == id);
                case ItemType.Shoes:
                    return Instance.itemDataBase.shoesItems.FirstOrDefault(i => i.itemId == id);
                case ItemType.Accessory:
                    return Instance.itemDataBase.accessoryItems.FirstOrDefault(i => i.itemId == id);
                case ItemType.SpecialEquip:
                    return Instance.itemDataBase.specialEquipItems.FirstOrDefault(i => i.itemId == id);
                case ItemType.Weapon:
                    return Instance.itemDataBase.weaponItems.FirstOrDefault(i => i.itemId == id);
                case ItemType.Consumable:
                    return Instance.itemDataBase.consumableItems.FirstOrDefault(i => i.itemId == id);
                case ItemType.Ingredient:
                    return Instance.itemDataBase.ingredientItems.FirstOrDefault(i => i.itemId == id);
                default:
                    return null;
            }
        }

        public static Item GetItemByName(ItemType itemType, string name)
        {
            switch (itemType)
            {
                case ItemType.Tops:
                    return Instance.itemDataBase.topsItems.FirstOrDefault(i => i.itemName == name);
                case ItemType.Bottoms:
                    return Instance.itemDataBase.bottomsItems.FirstOrDefault(i => i.itemName == name);
                case ItemType.Gloves:
                    return Instance.itemDataBase.glovesItems.FirstOrDefault(i => i.itemName == name);
                case ItemType.Shoes:
                    return Instance.itemDataBase.shoesItems.FirstOrDefault(i => i.itemName == name);
                case ItemType.Accessory:
                    return Instance.itemDataBase.accessoryItems.FirstOrDefault(i => i.itemName == name);
                case ItemType.SpecialEquip:
                    return Instance.itemDataBase.specialEquipItems.FirstOrDefault(i => i.itemName == name);
                case ItemType.Weapon:
                    return Instance.itemDataBase.weaponItems.FirstOrDefault(i => i.itemName == name);
                case ItemType.Consumable:
                    return Instance.itemDataBase.consumableItems.FirstOrDefault(i => i.itemName == name);
                case ItemType.Ingredient:
                    return Instance.itemDataBase.ingredientItems.FirstOrDefault(i => i.itemName == name);
                default:
                    return null;
            }
        }
    }
}

