﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Items/Equip Item")]
    public class EquipItem : Item
    {
        public GameObject modelPrefab;
        public bool isArmed;

        [Header("Equipment Status")]
        public string kind;
        public int currentDurability;
        public int maxDurability;
        [Range(0, 5)]
        public int enforceLevel = 0; //강화 수치

        public List<ItemAttribute> itemAttributes = new List<ItemAttribute>()
        {
            new ItemAttribute(){attribute = Attribute.Hp, value = 0 },
            new ItemAttribute(){attribute = Attribute.Attack, value = 0},
            new ItemAttribute(){attribute = Attribute.Defense, value = 0},
            new ItemAttribute(){attribute = Attribute.Critical, value = 0},
            new ItemAttribute(){attribute = Attribute.CriticalDamage, value = 0},
            new ItemAttribute(){attribute = Attribute.Stamina, value = 0}
        };



        //public SetItem setItem;

        private void OnValidate()
        {
            if (quantity != 1) quantity = 1;
        }
    }
}