using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerAttackAnimation : MonoBehaviour
    {
        PlayerAnimatorHandler animatorHandler;
        InputHandler inputHandler;
        public string lastAttack;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
            inputHandler = GetComponent<InputHandler>();
        }

        #region  Attack Handle Functions
        public void HandleWeaponCombo(WeaponItem weaponItem)
        {
            animatorHandler.anim.SetBool("canDoCombo", false);
            if (lastAttack == weaponItem.OneHanded_LightAttack1)
            {
                animatorHandler.PlayTargetAnimation(weaponItem.OneHanded_LightAttack2, true);
                lastAttack = weaponItem.OneHanded_LightAttack2;
            }
            else if (lastAttack == weaponItem.OneHanded_LightAttack2)
            {
                animatorHandler.PlayTargetAnimation(weaponItem.OneHanded_LightAttack3, true);
                lastAttack = weaponItem.OneHanded_LightAttack3;
            }
            else if (lastAttack == weaponItem.OneHanded_LightAttack3)
            {
                animatorHandler.PlayTargetAnimation(weaponItem.OneHanded_LightAttack4, true);
                lastAttack = weaponItem.OneHanded_LightAttack4;
            }
            else if (lastAttack == weaponItem.OneHanded_LightAttack4)
            {
                animatorHandler.PlayTargetAnimation(weaponItem.OneHanded_LightAttack5, true);
                lastAttack = weaponItem.OneHanded_LightAttack5;
            }
        }

        public void HandleLightAttack(WeaponItem weaponItem)
        {
            animatorHandler.PlayTargetAnimation(weaponItem.OneHanded_LightAttack1, true);
            lastAttack = weaponItem.OneHanded_LightAttack1;
        }

        public void HandleHeavyAttack(WeaponItem weaponItem)
        {
            animatorHandler.PlayTargetAnimation(weaponItem.OneHanded_HeavyAttack1, true);
            lastAttack = weaponItem.OneHanded_HeavyAttack1;
        }

        public void HandleSkillAttack(PlayerSkill playerSkill)
        {
            if (playerSkill == null)
                return;

            animatorHandler.anim.SetBool("canDoCombo", false);
            animatorHandler.PlayTargetAnimation(playerSkill.SkillAnimationName, true);
        }

        public void HandleStandUpRevengeAttack()
        {
            animatorHandler.anim.SetBool("isFalldown", false);
            animatorHandler.PlayTargetAnimation("StandUp_Revenge", true, duration: 0f);
        }

        #endregion

        #region  Defense Handle Functions

        public void HandleGuard(WeaponItem weaponItem)
        {
            if (PlayerManager.it.isBlocking)
                return;

            animatorHandler.PlayTargetAnimation(weaponItem.Weapon_Block, isInteracting: true);
            animatorHandler.anim.SetBool("isBlocking", true);
            Debug.Log("가드 실행");
        }

        public void HandleCounterAttack(WeaponItem weaponItem)
        {
            animatorHandler.PlayTargetAnimation(weaponItem.Weapon_CounterAttack, true);
        }

        #endregion
    }
}
