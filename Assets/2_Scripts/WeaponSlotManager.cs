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

        private void Awake()
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
        }
        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            GameObject weaponGO;

            if (isLeft)
            {
                leftHandSlot.LoadWeaponModel(weaponItem, out weaponGO);
                LoadLeftWeaponDamageCollider();

                #region Handle Left Weapon Idle Animations
                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.left_Hand_Idle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Left Arm Empty", 0.2f);
                }
                #endregion
            }
            else
            {
                
                rightHandSlot.LoadWeaponModel(weaponItem, out weaponGO);
                LoadRightWeaponDamageCollider();
                unholderSlot.LoadUnEquipWeaponModel(weaponItem);
                activeWeaponObject.Initailize(weaponGO);

/*                #region Handle Right Weapon Idle Animations
                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.right_Hand_Idle, 0.2f);

                }
                else
                {
                    animator.CrossFade("Right Arm Empty", 0.2f);
                }
                #endregion*/
            }
        }

        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void OpenRightHandDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void OpenLeftHandDamageCollider()
        {
            leftHandDamageCollider.EnableDamageCollider();
        }

        public void CloseRightHandDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }

        public void CloseLeftHandDamageCollider()
        {
            leftHandDamageCollider.DisableDamageCollider();
        }
    }
}

