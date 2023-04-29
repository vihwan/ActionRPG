using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XftWeapon;

namespace SG
{
    //플레이어 등 위에 붙어있을, 미장착 무기를 생성하는 스크립트.
    public class WeaponUnholderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        public GameObject currentUnequipWeaponModel;
        public XWeaponTrail xWeaponTrail;

        public void LoadUnEquipWeaponModel(WeaponItem weaponItem)
        {
            UnloadWeaponAndDestroy();

            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
            if (model != null)
            {
                if (parentOverride != null)
                {
                    model.transform.parent = parentOverride;

                }
                else
                {
                    model.transform.parent = transform;
                }

                // 무기 : 양손검
                // 등에 붙어있을 무기 위치와 Rotation을 임의로 설정해, 무기 집어넣을 때 자연스럽도록 만든다.
                model.transform.localPosition = new Vector3(0.1380693f, 0.5421505f, -0.1332065f);
                model.transform.localRotation = Quaternion.Euler(new Vector3(-166.547f, -131.76f, 77.48399f));
                model.transform.localScale = Vector3.one;
            }
            currentUnequipWeaponModel = model;
            currentUnequipWeaponModel.GetComponentInChildren<WeaponPivoting>().UnEquipPivoting();
            xWeaponTrail = currentUnequipWeaponModel.GetComponentInChildren<XWeaponTrail>();
            if(xWeaponTrail != null)
                xWeaponTrail.enabled = false;
        }

        private void UnloadWeaponAndDestroy()
        {
            if (currentUnequipWeaponModel != null)
            {
                Destroy(currentUnequipWeaponModel);
            }
        }
    }
}
