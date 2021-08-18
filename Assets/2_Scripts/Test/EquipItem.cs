using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [CreateAssetMenu(menuName = "Items/Equip Item")]
    public class EquipItem : Item
    {
        public GameObject modelPrefab;

        [Header("Equipment Status")]
        public ItemType itemType;
        public string kind;
        public int currentDurability;
        public int maxDurability;
        [Range(1, 5)]
        public int rarity;
        public ItemAttribute[] itemAttributes;

        //public SetItem setItem;
    }
}
