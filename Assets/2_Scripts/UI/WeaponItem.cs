using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isArmed;

        [Header("Weapon Status")]
        public string kind;
        public int currentDurability;
        public int maxDurability;
        [Range(1, 5)]
        public int rarity = 1;
        public List<ItemAttribute> itemAttributes = new List<ItemAttribute>()
        {
            new ItemAttribute(){attribute = Attribute.Hp, value = 0 },
            new ItemAttribute(){attribute = Attribute.Attack, value = 0},
            new ItemAttribute(){attribute = Attribute.Defense, value = 0},
            new ItemAttribute(){attribute = Attribute.Critical, value = 0},
            new ItemAttribute(){attribute = Attribute.CriticalDamage, value = 0},
            new ItemAttribute(){attribute = Attribute.Stamina, value = 0}
        };


        //나중에 무기에 따라 다르게 설정되도록 프로퍼티로 세팅하는 것이 좋겠다.
        [Header("Idle Animations")]
        public string right_Hand_Idle = "Right_Arm_Idle_01";
        public string left_Hand_Idle = "Left_Arm_Idle_01";

        [Header("Attack Animations")]
        public string OneHanded_LightAttack1 = "Light_Attack_1";
        public string OneHanded_LightAttack2 = "Light_Attack_2";
        public string OneHanded_LightAttack3 = "Light_Attack_3";
        public string OneHanded_HeavyAttack1 = "Heavy_Attack_1";


        private void OnValidate()
        {
            if(quantity != 1) quantity = 1;
        }
    }
}

