using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Items/Consumable Item")]
    public class ConsumableItem : Item
    {
        [Header("Consumable Status")]
        public int coolTime; //재사용 쿨타임
        public bool isArmed;

        public List<ConsumableAttribute> consumableAttributes = new List<ConsumableAttribute>()
        {
            new ConsumableAttribute(){consumableAttribute = ConsumeAttribute.Hp, value = 0},
            new ConsumableAttribute(){consumableAttribute = ConsumeAttribute.Mp, value = 0},
            new ConsumableAttribute(){consumableAttribute = ConsumeAttribute.Stamina, value = 0},
            new ConsumableAttribute(){consumableAttribute = ConsumeAttribute.Attack, value = 0},
            new ConsumableAttribute(){consumableAttribute = ConsumeAttribute.Defense, value = 0},
            new ConsumableAttribute(){consumableAttribute = ConsumeAttribute.Critical, value = 0},
            new ConsumableAttribute(){consumableAttribute = ConsumeAttribute.CriticalDamage, value = 0},
            new ConsumableAttribute(){consumableAttribute = ConsumeAttribute.Speed, value = 0},
            new ConsumableAttribute(){consumableAttribute = ConsumeAttribute.SkillDamage, value = 0},
        };
        //소비 아이템은 우선 체력, 마나 회복 용으로 만들고
        //이후에 따로 지속 시간이 있는 여러 스테이터스 버프용 속성 아이템도 만들어보자.

        private void OnValidate()
        {
            if (itemType != ItemType.Consumable)
                itemType = ItemType.Consumable;

            if (coolTime < 0)
                coolTime = 0;

            if (quantity < 0)
                quantity = 0;
        }
    }
}
