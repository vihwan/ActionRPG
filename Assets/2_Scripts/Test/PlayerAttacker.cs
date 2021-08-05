using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerAttacker : MonoBehaviour
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
    }
}
