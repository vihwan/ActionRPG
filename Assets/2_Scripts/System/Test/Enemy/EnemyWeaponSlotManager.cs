using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XftWeapon;


namespace SG
{

    public class EnemyWeaponSlotManager : MonoBehaviour
    {
        //public WeaponItem rightHandWeapon;
        public WeaponHolderSlot rightHandSlot;
        [SerializeField] private EnemyDamageCollider enemyDamageCollider;
        [SerializeField] private XWeaponTrail xWeaponTrail;

        public void Init()
        {

            LoadWeaponDamageCollider();

            // if(rightHandWeapon != null)
            // {
            //    // LoadWeaponOnSlot(rightHandWeapon);
            // }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem)
        {
            GameObject weaponGO;
            rightHandSlot.LoadWeaponModel(weaponItem, out weaponGO);
        }

        public void LoadWeaponDamageCollider()
        {
            enemyDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<EnemyDamageCollider>();
            if(enemyDamageCollider != null)
            {
                enemyDamageCollider.Init();
                xWeaponTrail = enemyDamageCollider.GetComponentInChildren<XWeaponTrail>();
                if(xWeaponTrail != null)
                    xWeaponTrail.enabled = false;
            }
        }

        #region  Animation Event Functions
        public void OpenDamageCollider()
        {
            enemyDamageCollider.EnableDamageCollider();
            xWeaponTrail.enabled = true;
        }

        public void CloseDamageCollider()
        {
            enemyDamageCollider.DisableDamageCollider();
            xWeaponTrail.enabled = false;
        }

        public void EnableCombo()
        {
           
        }

        public void DisableCombo()
        {
            
        }

        #endregion
    }
}
