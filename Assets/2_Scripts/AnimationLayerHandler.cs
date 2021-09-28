using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class AnimationLayerHandler : MonoBehaviour
    {
        internal PlayerAnimatorHandler animatorHandler;
        private InputHandler inputHandler;

        public void Init()
        {
            animatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
            inputHandler = GetComponent<InputHandler>();
        }


        //무기 장착 로코모션 레이어 Weight를 0으로, 무기 미장착 로코모션 레이어 Weight를 1로
        public void HandlePlayerUnEquip()
        {
            animatorHandler.anim.SetLayerWeight(animatorHandler.anim.GetLayerIndex("Base Layer Equip"), 0f);
            animatorHandler.anim.SetLayerWeight(animatorHandler.anim.GetLayerIndex("Base Layer UnEquip"), 1f);
            animatorHandler.anim.SetLayerWeight(animatorHandler.anim.GetLayerIndex("Override Equip"), 0f);
            animatorHandler.anim.SetLayerWeight(animatorHandler.anim.GetLayerIndex("Override UnEquip"), 1f);
            animatorHandler.anim.SetLayerWeight(animatorHandler.anim.GetLayerIndex("Both Hand"), 1f);
            animatorHandler.anim.SetBool("isUnEquip", true);

            if (inputHandler.MoveAmount > 0)
            {
                animatorHandler.PlayTargetAnimation("WeaponChange_UnEquip_NotMove", false);
                animatorHandler.anim.SetBool("canRotate", true);
            }
            else
            {
                animatorHandler.PlayTargetAnimation("WeaponChange_UnEquip", false);
                animatorHandler.anim.SetBool("canRotate", true);
            }
        }

        public void HandlePlayerEquip()
        {
            animatorHandler.anim.SetLayerWeight(animatorHandler.anim.GetLayerIndex("Base Layer Equip"), 1f);
            animatorHandler.anim.SetLayerWeight(animatorHandler.anim.GetLayerIndex("Base Layer UnEquip"), 0f);
            animatorHandler.anim.SetLayerWeight(animatorHandler.anim.GetLayerIndex("Override Equip"), 1f);
            animatorHandler.anim.SetLayerWeight(animatorHandler.anim.GetLayerIndex("Override UnEquip"), 0f);
            animatorHandler.anim.SetLayerWeight(animatorHandler.anim.GetLayerIndex("Both Hand"), 0f);
            animatorHandler.anim.SetBool("isUnEquip", false);
        }

        public void HandlePlayerOnWeaponEquipPanelAnimation(bool state)
        {
            if (state)
            {
                animatorHandler.anim.SetLayerWeight(animatorHandler.anim.GetLayerIndex("Override WeaponEquipPanel"), 1f);
            }
            else
            {
                animatorHandler.anim.SetLayerWeight(animatorHandler.anim.GetLayerIndex("Override WeaponEquipPanel"), 0f);
            }
        }

        internal void SetPlayerAnimationIsWeaponOnWeaponEquipPanel()
        {

            if (animatorHandler.anim.GetCurrentAnimatorStateInfo(animatorHandler.anim.GetLayerIndex("Override WeaponEquipPanel")).IsName("WE_Idle") ||
                animatorHandler.anim.GetCurrentAnimatorStateInfo(animatorHandler.anim.GetLayerIndex("Override WeaponEquipPanel")).IsName("WE_UnWeapon"))
            {
                animatorHandler.PlayTargetAnimation("WE_Weapon", true, 0f);
                return;
            }
        }

        internal void SetPlayerAnimationIsUnWeaponOnWeaponEquipPanel()
        {
            if (animatorHandler.anim.GetCurrentAnimatorStateInfo(animatorHandler.anim.GetLayerIndex("Override WeaponEquipPanel")).IsName("WE_WeaponIdle") ||
                animatorHandler.anim.GetCurrentAnimatorStateInfo(animatorHandler.anim.GetLayerIndex("Override WeaponEquipPanel")).IsName("WE_Weapon"))
            {
                animatorHandler.PlayTargetAnimation("WE_UnWeapon", true, 0f);
                return;
            }
        }

        public void SetAnimaionLayerWeightCloseCharacterPanel()
        {
            HandlePlayerOnWeaponEquipPanelAnimation(false);

            if (animatorHandler.anim.GetCurrentAnimatorStateInfo(animatorHandler.anim.GetLayerIndex("Override WeaponEquipPanel")).IsName("WE_WeaponIdle") ||
                animatorHandler.anim.GetCurrentAnimatorStateInfo(animatorHandler.anim.GetLayerIndex("Override WeaponEquipPanel")).IsName("WE_Weapon"))
            {
                if (inputHandler.MoveAmount > 0)
                {
                    animatorHandler.PlayTargetAnimation("WeaponChange_UnEquip_NotMove", false);
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("WeaponChange_UnEquip", false);
                }    
            }
            else
                animatorHandler.PlayTargetAnimation("Locomotion", false);

            animatorHandler.anim.SetLayerWeight(animatorHandler.anim.GetLayerIndex("Base Layer Equip"), 0f);
            animatorHandler.anim.SetLayerWeight(animatorHandler.anim.GetLayerIndex("Base Layer UnEquip"), 1f);
            animatorHandler.anim.SetLayerWeight(animatorHandler.anim.GetLayerIndex("Override Equip"), 0f);
            animatorHandler.anim.SetLayerWeight(animatorHandler.anim.GetLayerIndex("Override UnEquip"), 1f);
            animatorHandler.anim.SetLayerWeight(animatorHandler.anim.GetLayerIndex("Both Hand"), 1f);
            animatorHandler.anim.SetBool("isUnEquip", true);
        }
    }
}
