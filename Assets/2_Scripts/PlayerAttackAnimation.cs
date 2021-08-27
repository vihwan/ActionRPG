using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerAttackAnimation : MonoBehaviour
    {
        AnimatorHandler animatorHandler;
        InputHandler inputHandler;
        public string lastAttack;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            inputHandler = GetComponent<InputHandler>();
        }

        public void HandleWeaponCombo(WeaponItem weaponItem)
        {
            animatorHandler.Anim.SetBool("canDoCombo", false);
            if(lastAttack == weaponItem.OneHanded_LightAttack1)
            {
                animatorHandler.PlayTargetAnimation(weaponItem.OneHanded_LightAttack2, true);
                lastAttack = weaponItem.OneHanded_LightAttack2;
            }
            else if(lastAttack == weaponItem.OneHanded_LightAttack2)
            {
                animatorHandler.PlayTargetAnimation(weaponItem.OneHanded_LightAttack3, true);
                lastAttack = weaponItem.OneHanded_LightAttack3;
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

            animatorHandler.Anim.SetBool("canDoCombo", false);
            animatorHandler.PlayTargetAnimation(playerSkill.skillAnimationName, true);
        }
    }
}
