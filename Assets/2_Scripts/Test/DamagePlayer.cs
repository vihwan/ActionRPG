using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class DamagePlayer : MonoBehaviour
    {
        private int damage = 25;

        private void OnTriggerEnter(Collider other)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if(playerStats != null)
            {
                playerStats.TakeDamage(damage);
                Debug.Log("데미지 입음");
            }
        }
    }

}

