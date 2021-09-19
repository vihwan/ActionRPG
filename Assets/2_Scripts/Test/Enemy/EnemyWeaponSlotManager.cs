using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{

    public class EnemyWeaponSlotManager : MonoBehaviour
    {
        public WeaponItem rightHandWeapon;
        public WeaponHolderSlot rightHandSlot;
        DamageCollider damageCollider;

        public void Init()
        {
            if(rightHandWeapon != null)
            {
                LoadWeaponOnSlot(rightHandWeapon);
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem)
        {
            GameObject weaponGO;
            rightHandSlot.LoadWeaponModel(weaponItem, out weaponGO);
            LoadWeaponDamageCollider();
        }

        public void LoadWeaponDamageCollider()
        {
            damageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void OpenDamageCollider()
        {
            damageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            damageCollider.DisableDamageCollider();
        }
    }
}
