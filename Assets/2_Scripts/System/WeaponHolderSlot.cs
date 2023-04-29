using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;

        public GameObject currentWeaponModel;

        public void UnloadWeapon()
        {
            if(currentWeaponModel != null)
            {
                currentWeaponModel.SetActive(false);
            }
        }
        
        public void UnloadWeaponAndDestroy()
        {
            if(currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }


        public void LoadWeaponModel(WeaponItem weaponItem, out GameObject weaponGO)
        {
            UnloadWeaponAndDestroy();

            if (weaponItem == null)
            {
                UnloadWeapon();
                weaponGO = null;
                return;
            }

            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
            if(model != null)
            {
                if(parentOverride != null)
                {
                    model.transform.parent = parentOverride;

                }
                else
                {
                    model.transform.parent = transform;
                }

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            currentWeaponModel = model;
            weaponGO = currentWeaponModel;
            weaponGO.layer = LayerMask.NameToLayer("Weapon");

            ChangeWeaponLayer(weaponGO, "Weapon");
        }

        private void ChangeWeaponLayer(GameObject gameObject, string layerName)
        {
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer(layerName);
                ChangeWeaponLayer(child.gameObject, layerName);
            }
        }
    }
}
