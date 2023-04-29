using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class DropItemBox : Interactable
    {
        [Header("Box Item List")]
        [SerializeField] public List<Item> itemsInDropBox;
        public override void Interact(PlayerManager playerManager)
        {
            GetItem(playerManager);
        }
        private void GetItem(PlayerManager playerManager)
        {
            //루트 아이템 윈도우 생성

            GUIManager.instance.lootWindow.SetLootWindow(this.gameObject, itemsInDropBox);
            GUIManager.instance.lootWindow.OpenLootWindow();

            //Interact Object UI SetActive False
            playerManager.InteractableUI.SetActiveInteractUI(false);
        }
        internal void SetItem(List<Item> items)
        {
            this.itemsInDropBox = items;
            if (this.itemsInDropBox == null) Debug.Log("아이템 박스 리스트가 비어있다.");
        }
    }
}
