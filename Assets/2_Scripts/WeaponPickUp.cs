using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class WeaponPickUp : Interactable
    {
        public WeaponItem weapon;

        public override void Interact(PlayerManager playerManager)
        {
            //PickUp The Weapon and Add It
            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory;
            PlayerLocomotion playerLocomotion;
            AnimatorHandler animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();

            //물건을 줍는 동안 플레이어의 움직임은 멈춰야함
            playerLocomotion.Rigidbody.velocity = Vector3.zero;
            animatorHandler.PlayTargetAnimation("Pick Up Item", true);
            playerInventory.weaponsInventory.Add(weapon);

            //Interact Object UI SetActive False
            playerManager.InteractableUI.SetActiveInteractUI(false);

            Destroy(gameObject);
        }
    }
}

