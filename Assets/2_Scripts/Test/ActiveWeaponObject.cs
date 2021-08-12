using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class ActiveWeaponObject : MonoBehaviour
    {
        public GameObject handWeaponObject;
        public GameObject unequipWeaponObject;

        public void Initailize(GameObject weaponGo)
        {
            handWeaponObject = weaponGo;
        }

        public void SetActiveHandWeapon(int num)
        {
            bool status = true;

            if (num == 0)
                status = true;
            else if(num == 1)
                status = false;

            handWeaponObject.SetActive(status);
            unequipWeaponObject.SetActive(!status);
        }
    }
}

