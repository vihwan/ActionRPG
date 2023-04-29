using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SG
{
    public class InteractTreasureChest : Interactable
    {
        [Header("Status")]
        private int chestId;
        private bool isOpen;

        [Header("Chest Item List")]
        [SerializeField] private List<Item> itemsInChest = new List<Item> ();
        //임시로 아이템을 에디터에서 채워넣었다.

        private Animator anim;

        public override void Start()
        {
            anim = GetComponentInChildren<Animator>(true);
            //임시로 아이템을 에디터에서 채워넣었다.
            //클론아이템들이 아니다. 주의
            if (itemsInChest.Count > 0)
            {
                foreach (Item item in itemsInChest)
                {
                    if (item.GetType().Equals(typeof(ConsumableItem)) || item.GetType().Equals(typeof(IngredientItem)))
                        item.quantity = Random.Range(1, 6);
                    else
                        item.quantity = 1;
                }
            }
        }
        public override void Interact(PlayerManager playerManager)
        {
            if (canInteract.Equals(false))
                return;

            canInteract = false;
            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            GetItem(playerManager);
            playerManager.OpenInteractionToVelocityZero();
        }
        private void GetItem(PlayerManager playerManager)
        {
            isOpen = true;
            anim.SetBool("Open", isOpen);
            anim.Play("Close to Open");

            GUIManager.instance.lootWindow.SetLootWindow(this.gameObject, itemsInChest);
            GUIManager.instance.lootWindow.OpenLootWindow();
            //Interact Object UI SetActive False
            playerManager.InteractableUI.SetActiveInteractUI(false);

            itemsInChest = null;
        }
    }

}
