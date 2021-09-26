using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{

    public class EnemyWeaponSlotManager : MonoBehaviour
    {
        //public WeaponItem rightHandWeapon;
        public WeaponHolderSlot rightHandSlot;
        [SerializeField] private EnemyDamageCollider enemyDamageCollider;

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
            }
        }

        #region  Animation Event Functions
        public void OpenDamageCollider()
        {
            enemyDamageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            enemyDamageCollider.DisableDamageCollider();
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
