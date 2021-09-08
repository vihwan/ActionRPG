using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public static class LootTable
    {
        private static List<Item> dropItemList = new List<Item>();
        public static List<Item> RollLoot(List<LootItem> lootItems)
        {
            dropItemList.Clear();

            //아이템의 드롭 여부를 정하고, 드롭된다면 드롭될 아이템의 최종 갯수도 정합니다.
            foreach (LootItem loot in lootItems)
            {
                float roll_CreateItem = Random.Range(0f, 100f);
                if (roll_CreateItem <= loot.dropChance)
                {
                    int roll_ItemAmount = Random.Range(1, loot.itemMaxAmount + 1);
                    loot.item.quantity = roll_ItemAmount;

                    dropItemList.Add(loot.item);
                }
            }
            return dropItemList;
        }
    }
}

