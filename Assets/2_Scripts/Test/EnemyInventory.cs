using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    //몬스터가 소지하고 있는 아이템을 관리하고, 드랍 확률을 설정하는 클래스입니다.

    [System.Serializable]
    public class LootItem
    {
        public Item item;
        [Range(1, 30)] 
        public int itemAmount;
        public float dropChance;
    }
    public class EnemyInventory : MonoBehaviour
    {
        [Header("Has Experience")]
        [SerializeField] private int exp;

        [Header("Has Item")]
        [SerializeField] private List<LootItem> itemTable = new List<LootItem>();
        [SerializeField] private List<Item> dropItems;

        public int Exp { get => exp;}

        public void Init()
        {

        }     
        public void CreateDropItem()
        {
            //몬스터가 사망하면 가지고 있는 아이템을 드랍합니다.
            dropItems.Clear();
            dropItems = LootTable.RollLoot(itemTable);

            ItemDropManager.Instance.GenerateDropItemBox(dropItems, this.gameObject.transform);
            Debug.Log("아이템 드랍");
        }
    }
}
