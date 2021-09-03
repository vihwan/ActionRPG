using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class InteractNPC : Interactable
    {
        public List<Item> itemLists;

        PlayerInventory playerInventory;
        PlayerLocomotion playerLocomotion;
        AnimatorHandler animatorHandler;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            //NPC와 대화 및 메뉴 선택
            TalkNPC(playerManager);
        }

        public void TalkNPC(PlayerManager playerManager)
        {
            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponent<AnimatorHandler>();

            //NPC랑 대화하는 동안은 플레이어의 움직임을 멈춰야함.
            playerLocomotion.Rigidbody.velocity = Vector3.zero;
            Debug.Log("NPC와 대화하기");

            //NPC가 가지고 있는 아이템을 보여주는 상점을 활성화시킴.
            GUIManager guiManager = FindObjectOfType<GUIManager>();
            guiManager.shopPanel.SetActiveShopPanel(true);
            guiManager.shopPanel.SetShopPanel(shopNameText);
            guiManager.shopPanel.CreateItemList(itemLists);

            //Interact Object UI SetActive False
            playerManager.InteractableUI.SetActiveInteractUI(false);
            InputHandler inputHandler = playerManager.GetComponent<InputHandler>();
            inputHandler.menuFlag = !inputHandler.menuFlag;
        }
    }
}
