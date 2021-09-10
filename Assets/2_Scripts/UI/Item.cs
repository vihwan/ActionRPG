using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public enum ItemType
    {
        Tops,   //상의
        Bottoms,//하의
        Gloves, //장갑
        Shoes,  //신발
        Accessory, //악세사리
        SpecialEquip, //특수장비
        Weapon, //무기
        Consumable, //소비 아이템
        Ingredient //재료 아이템
    }

    //아이템마다 가지는 고유 속성(수치)
    public enum Attribute
    {
        Hp,
        Attack,
        Defense,
        Critical,
        CriticalDamage,
        Stamina,
    }

    //소비 아이템 고유 속성
    public enum ConsumeAttribute
    {
        Hp,
        Mp,
        Stamina,
        Attack,
        Defense,
        Critical,
        CriticalDamage,
        Speed,
        SkillDamage
    }

    [System.Serializable]
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        public Sprite itemIcon;
        public string itemName;
        public ItemType itemType;
        public string kind;
        public int quantity;
        [TextArea]
        public string itemDescription;
        public int price;
        [Range(1, 5)] public int rarity = 1;
    }

    [System.Serializable]
    public class ItemAttribute
    {
        public Attribute attribute;
        public int value;
    }

    [System.Serializable]
    public class ConsumableAttribute
    {
        public ConsumeAttribute consumableAttribute;
        public int value;
        public int time = 0; 
    }
}
