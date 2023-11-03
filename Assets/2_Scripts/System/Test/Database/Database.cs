using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Extension;

namespace SG
{
    public class Database : MonoBehaviourSingleton<Database>
    {
        [SerializeField] private ItemDataBase itemDataBase;
        [SerializeField] private PrefabDataBase prefabDatabase;

        public ItemDataBase ItemDataBase => itemDataBase;
        public PrefabDataBase PrefabDataBase => prefabDatabase;

        public void Init()
        {
            itemDataBase = this.gameObject.GetComponentForce<ItemDataBase>();
            itemDataBase.Init();

            prefabDatabase = this.gameObject.GetComponentForce<PrefabDataBase>();
            prefabDatabase.Init();
        }

        public Item GetItemByID(ItemType itemType, int id)
        {
            switch (itemType)
            {
                case ItemType.Tops:
                    return itemDataBase.topsItems.FirstOrDefault(i => i.itemId == id);
                case ItemType.Bottoms:
                    return itemDataBase.bottomsItems.FirstOrDefault(i => i.itemId == id);
                case ItemType.Gloves:
                    return itemDataBase.glovesItems.FirstOrDefault(i => i.itemId == id);
                case ItemType.Shoes:
                    return itemDataBase.shoesItems.FirstOrDefault(i => i.itemId == id);
                case ItemType.Accessory:
                    return itemDataBase.accessoryItems.FirstOrDefault(i => i.itemId == id);
                case ItemType.SpecialEquip:
                    return itemDataBase.specialEquipItems.FirstOrDefault(i => i.itemId == id);
                case ItemType.Weapon:
                    return itemDataBase.weaponItems.FirstOrDefault(i => i.itemId == id);
                case ItemType.Consumable:
                    return itemDataBase.consumableItems.FirstOrDefault(i => i.itemId == id);
                case ItemType.Ingredient:
                    return itemDataBase.ingredientItems.FirstOrDefault(i => i.itemId == id);
                default:
                    return null;
            }
        }

        public Item GetItemByName(ItemType itemType, string name)
        {
            switch (itemType)
            {
                case ItemType.Tops:
                    return itemDataBase.topsItems.FirstOrDefault(i => i.itemName == name);
                case ItemType.Bottoms:
                    return itemDataBase.bottomsItems.FirstOrDefault(i => i.itemName == name);
                case ItemType.Gloves:
                    return itemDataBase.glovesItems.FirstOrDefault(i => i.itemName == name);
                case ItemType.Shoes:
                    return itemDataBase.shoesItems.FirstOrDefault(i => i.itemName == name);
                case ItemType.Accessory:
                    return itemDataBase.accessoryItems.FirstOrDefault(i => i.itemName == name);
                case ItemType.SpecialEquip:
                    return itemDataBase.specialEquipItems.FirstOrDefault(i => i.itemName == name);
                case ItemType.Weapon:
                    return itemDataBase.weaponItems.FirstOrDefault(i => i.itemName == name);
                case ItemType.Consumable:
                    return itemDataBase.consumableItems.FirstOrDefault(i => i.itemName == name);
                case ItemType.Ingredient:
                    return itemDataBase.ingredientItems.FirstOrDefault(i => i.itemName == name);
                default:
                    return null;
            }
        }
    }
}

