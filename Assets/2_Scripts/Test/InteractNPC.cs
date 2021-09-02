using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class InteractNPC : Interactable
    {
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

            //Interact Object UI SetActive False
            playerManager.InteractableUI.SetActiveInteractUI(false);
        }
    }
}
