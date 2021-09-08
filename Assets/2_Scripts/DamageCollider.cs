using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class DamageCollider : MonoBehaviour
    {
        private Collider damageCollider;
        private int currentWeaponDamage = 0;
        private PlayerStats playerStats;
        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;

            playerStats = FindObjectOfType<PlayerStats>();
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
            currentWeaponDamage = playerStats.Attack;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                PlayerStats playerStats = other.GetComponent<PlayerStats>();

                if(playerStats != null)
                {
                    playerStats.TakeDamage(currentWeaponDamage);
                }
            }

            if(other.tag == "Enemy")
            {
                EnemyStats enemyStats = other.GetComponent<EnemyStats>();

                if(enemyStats != null)
                {
                    enemyStats.TakeDamage(currentWeaponDamage);
                }
            }
        }
    }


}
