using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{

    public enum ItemType
    {
        Weapon, //무기
        Tops,   //상의
        Bottoms,//하의
        Gloves, //장갑
        Shoes,  //신발
        Accessory, //악세사리
        SpecialEquip, //특수장비
        Consumable
    }
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        public Sprite itemIcon;
        public string itemName;
        [TextArea]
        public string itemDescription;
    }
}
