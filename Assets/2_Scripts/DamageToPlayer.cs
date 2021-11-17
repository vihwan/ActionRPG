using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class DamageToPlayer : MonoBehaviour
    {
        private int damage = 25;

        private void OnTriggerEnter(Collider other)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if(playerStats != null)
            {
                playerStats.TakeDamage(damage, 3); //FallDown
                Debug.Log("데미지 입음");
            }
        }
    }

}

