using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class ActiveWeaponObject : MonoBehaviour
    {
        public GameObject handWeaponObject;
        public GameObject unequipWeaponObject;

        public void Initailize(GameObject weaponGo, bool state)
        {
            handWeaponObject = weaponGo;
            unequipWeaponObject = GetComponentInChildren<WeaponUnholderSlot>().currentUnequipWeaponModel;
            if (unequipWeaponObject != null)
                unequipWeaponObject.SetActive(false);

            handWeaponObject.SetActive(!state);
            unequipWeaponObject.SetActive(state);
        }

        public void SetActiveHandWeapon(int num)
        {
            bool status = false;

            if (num == 0)
                status = true;
            else if(num == 1)
                status = false;

            handWeaponObject.SetActive(status);
            unequipWeaponObject.SetActive(!status);
        }
    }
}

