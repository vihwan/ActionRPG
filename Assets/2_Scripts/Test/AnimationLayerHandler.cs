using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class AnimationLayerHandler : MonoBehaviour
    {
        private AnimatorHandler animatorHandler;
        private InputHandler inputHandler;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            inputHandler = GetComponent<InputHandler>();
        }

        
        //무기 장착 로코모션 레이어 Weight를 0으로, 무기 미장착 로코모션 레이어 Weight를 1로
        public void HandlePlayerUnEquip()
        {
            animatorHandler.Anim.SetLayerWeight(animatorHandler.Anim.GetLayerIndex("Base Layer Equip"), 0f);
            animatorHandler.Anim.SetLayerWeight(animatorHandler.Anim.GetLayerIndex("Base Layer UnEquip"), 1f);
            animatorHandler.Anim.SetLayerWeight(animatorHandler.Anim.GetLayerIndex("Override Equip"), 0f);
            animatorHandler.Anim.SetLayerWeight(animatorHandler.Anim.GetLayerIndex("Override UnEquip"), 1f);
            animatorHandler.Anim.SetBool("isUnEquip", true);

            if(inputHandler.MoveAmount > 0)
            {
                animatorHandler.PlayTargetAnimation("WeaponChange_UnEquip_NotMove", false);
            }
            else
            {
                animatorHandler.PlayTargetAnimation("WeaponChange_UnEquip", false);
            }

           // Debug.Log(animatorHandler.Anim.GetLayerName(1) + "의 Weight : " + animatorHandler.Anim.GetLayerWeight(1));
        }

        public void HandlePlayerEquip()
        {
            animatorHandler.Anim.SetLayerWeight(animatorHandler.Anim.GetLayerIndex("Base Layer Equip"), 1f);
            animatorHandler.Anim.SetLayerWeight(animatorHandler.Anim.GetLayerIndex("Base Layer UnEquip"), 0f);
            animatorHandler.Anim.SetLayerWeight(animatorHandler.Anim.GetLayerIndex("Override Equip"), 1f);
            animatorHandler.Anim.SetLayerWeight(animatorHandler.Anim.GetLayerIndex("Override UnEquip"), 0f);
            animatorHandler.Anim.SetBool("isUnEquip", false);

            //Debug.Log(animatorHandler.Anim.GetLayerName(1) + "의 Weight : " + animatorHandler.Anim.GetLayerWeight(1) + "\n"
            //    + animatorHandler.Anim.GetLayerName(2) + "의 Weight : " + animatorHandler.Anim.GetLayerWeight(2));
        }
    }
}
