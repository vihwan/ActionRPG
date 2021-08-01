using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject SpineWeapon;
    public GameObject HandWeapon;


    private void OnActiveHandWeapon()
    {
        SpineWeapon.SetActive(false);
        HandWeapon.SetActive(true);
    }

    private void OnDeactiveHandWeapon()
    {
        SpineWeapon.SetActive(true);
        HandWeapon.SetActive(false);
    }
}
