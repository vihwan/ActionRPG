using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class ItemDataBase : MonoBehaviour
    {
        [SerializeField] public List<WeaponItem> weaponItems = new List<WeaponItem>();
        [SerializeField] public List<EquipItem> topsItems = new List<EquipItem>();
        [SerializeField] public List<EquipItem> bottomsItems = new List<EquipItem>();
        [SerializeField] public List<EquipItem> glovesItems = new List<EquipItem>();
        [SerializeField] public List<EquipItem> shoesItems = new List<EquipItem>();
        [SerializeField] public List<EquipItem> accessoryItems = new List<EquipItem>();
        [SerializeField] public List<EquipItem> specialEquipItems = new List<EquipItem>();
        [SerializeField] public List<ConsumableItem> consumableItems = new List<ConsumableItem>();
        [SerializeField] public List<IngredientItem> ingredientItems = new List<IngredientItem>();

        public void Init()
        {      
            LoadWeapon();
            LoadIngredient();
            LoadEquipment();
            LoadConsumable();
        }
        private void LoadWeapon()
        {
            WeaponItem[] items;
            int idCount = 0;
            items = Resources.LoadAll<WeaponItem>("Scriptable/Weapon");
            for (int i = 0; i < items.Length; i++)
            {
                items[i].itemId = idCount;
                weaponItems.Add(items[i]);
                idCount++;
            }
        }

        private void LoadEquipment()
        {
            EquipItem[] items;
            int idCount = 0;
            items = Resources.LoadAll<EquipItem>("Scriptable/Equipment/Tops");
            for (int i = 0; i < items.Length; i++)
            {
                items[i].itemId = idCount;
                topsItems.Add(items[i]);
                idCount++;
            }

            idCount = 0;
            items = Resources.LoadAll<EquipItem>("Scriptable/Equipment/Bottoms");
            for (int i = 0; i < items.Length; i++)
            {
                items[i].itemId = idCount;
                bottomsItems.Add(items[i]);
                idCount++;
            }

            idCount = 0;
            items = Resources.LoadAll<EquipItem>("Scriptable/Equipment/Gloves");
            for (int i = 0; i < items.Length; i++)
            {
                items[i].itemId = idCount;
                glovesItems.Add(items[i]);
                idCount++;
            }

            idCount = 0;
            items = Resources.LoadAll<EquipItem>("Scriptable/Equipment/Shoes");
            for (int i = 0; i < items.Length; i++)
            {
                items[i].itemId = idCount;
                shoesItems.Add(items[i]);
                idCount++;
            }

            idCount = 0;
            items = Resources.LoadAll<EquipItem>("Scriptable/Equipment/Accessory");
            for (int i = 0; i < items.Length; i++)
            {
                items[i].itemId = idCount;
                accessoryItems.Add(items[i]);
                idCount++;
            }

            idCount = 0;
            items = Resources.LoadAll<EquipItem>("Scriptable/Equipment/SpecialEquip");
            for (int i = 0; i < items.Length; i++)
            {
                items[i].itemId = idCount;
                specialEquipItems.Add(items[i]);
                idCount++;
            }
        }

        private void LoadConsumable()
        {
            int idCount = 0;
            ConsumableItem[] items;
            items = Resources.LoadAll<ConsumableItem>("Scriptable/Consumable");

            for (int i = 0; i < items.Length; i++)
            {
                items[i].itemId = idCount;
                consumableItems.Add(items[i]);
                idCount++;
            }
        }

        private void LoadIngredient()
        {
            int idCount = 0;
            IngredientItem[] items;
            items = Resources.LoadAll<IngredientItem>("Scriptable/Ingredient");

            for (int i = 0; i < items.Length; i++)
            {
                items[i].itemId = idCount;
                ingredientItems.Add(items[i]);
                idCount++;
            }
        }
    }
}
