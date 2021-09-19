using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class LootItemPanel : MonoBehaviour
    {
        [SerializeField] public Item item;
        [SerializeField] public TMP_Text itemName;
        [SerializeField] public Image itemIcon; 

        public void SetLootItemPanel(Item item)
        {
            this.item = item;
            if(this.item != null)
            {
                itemName.text = item.itemName + " × " + item.quantity;      
                itemIcon.sprite = item.itemIcon;
            }

            SetItemNameColorToRarity(item);
        }
        private void SetItemNameColorToRarity(Item item)
        {
            switch (item.rarity)
            {
                case 1: itemName.color = Color.black; break;
                case 2: itemName.color = Color.grey; break;
                case 3: itemName.color = Color.yellow; break;
                case 4: itemName.color = Color.cyan; break;
                case 5: itemName.color = Color.red; break;
                default: break;
            }
        }
    }
}
