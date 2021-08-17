using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Weapon Status")]
        public string kind;
        public int attack;
        public int currentDurability;
        public int maxDurability;
        [Range(1,5)] 
        public int rarity;
        

        [Header("Idle Animations")]
        public string right_Hand_Idle;
        public string left_Hand_Idle;

        [Header("Attack Animations")]
        public string OneHanded_LightAttack1;
        public string OneHanded_LightAttack2;
        public string OneHanded_LightAttack3;
        public string OneHanded_HeavyAttack1;
    }
}

