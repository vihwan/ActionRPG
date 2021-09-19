using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class EnemyDamageCollider : MonoBehaviour
    {
        private Collider damageCollider;
        private int currentWeaponDamage = 0;
        private EnemyStats enemyStats;
        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;

            enemyStats = GetComponentInParent<EnemyStats>();
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
            currentWeaponDamage = enemyStats.attack;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                PlayerStats playerStats = other.GetComponent<PlayerStats>();

                if (playerStats != null)
                {
                    playerStats.TakeDamage(currentWeaponDamage);
                }
                else
                    Debug.Log("PlayerStats is Null");
            }
        }
    }

}
