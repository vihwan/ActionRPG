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

        [Header("One Handed Attack Animations")]
        public string OneHanded_LightAttack1;
        public string OneHanded_LightAttack2;
        public string OneHanded_LightAttack3;
        public string OneHanded_HeavyAttack1;
    }
}

