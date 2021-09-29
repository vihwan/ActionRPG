using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class WeaponSlotManager : MonoBehaviour
    {
        [Header("Weapon Slot")]
        [SerializeField] private WeaponHolderSlot leftHandSlot;
        [SerializeField] private WeaponHolderSlot rightHandSlot;
        [SerializeField] private WeaponUnholderSlot unholderSlot;

        private DamageCollider leftHandDamageCollider;
        private DamageCollider rightHandDamageCollider;

        private Animator animator;
        private ActiveWeaponObject activeWeaponObject;
        private PlayerManager playerManager;

        public void Init()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot item in weaponHolderSlots)
            {
                if (item.isLeftHandSlot)
                {
                    leftHandSlot = item;
                }
                else if (item.isRightHandSlot)
                {
                    rightHandSlot = item;
                }
            }

            animator = GetComponent<Animator>();
            activeWeaponObject = GetComponentInChildren<ActiveWeaponObject>();
            unholderSlot = GetComponentInChildren<WeaponUnholderSlot>();
            playerManager = FindObjectOfType<PlayerManager>();
        }
        public void LoadWeaponOnSlot(WeaponItem weaponItem)
        {
            GameObject weaponGO;
            rightHandSlot.LoadWeaponModel(weaponItem, out weaponGO);
            LoadRightWeaponDamageCollider();
            unholderSlot.LoadUnEquipWeaponModel(weaponItem);
            activeWeaponObject.Initailize(weaponGO, playerManager.isUnEquip);
        }


        #region Attack Animation Event Functions
        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void OpenDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }


        [System.Obsolete]
        public void OpenLeftHandDamageCollider()
        {
            leftHandDamageCollider.EnableDamageCollider();
        }

        [System.Obsolete]
        public void CloseLeftHandDamageCollider()
        {
            leftHandDamageCollider.DisableDamageCollider();
        }
         #endregion
    } 
}

