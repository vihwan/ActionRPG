using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class DropItem : Interactable
    {
        [SerializeField] private Item item;
        public override void Interact(PlayerManager playerManager)
        {
            GetItem(playerManager);
        }
        private void GetItem(PlayerManager playerManager)
        {
            PlayerInventory.Instance.SaveGetItemToInventory(item, item.quantity);
            Destroy(this.gameObject);
        }
        internal void SetItem(Item item)
        {
            this.item = item;
            if(this.item != null)
            {
                interactName = item.itemName;
                interactIcon = item.itemIcon;
                canInteract = true;
            }
        }
    }
}
