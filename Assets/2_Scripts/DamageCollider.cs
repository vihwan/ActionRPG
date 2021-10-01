using System;
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
            CheckColliderEnemy(other);
            CheckColliderEnemyTest(other);
        }

        private void CheckColliderEnemyTest(Collider collider)
        {
            if(collider.tag.Equals("Enemy"))
            {
                EnemyTestStats enemyTestStats = collider.GetComponent<EnemyTestStats>();
                if(enemyTestStats != null)
                {
                    enemyTestStats.TakeDamage(currentWeaponDamage);
                }
            }
        }

        private void CheckColliderEnemy(Collider collider)
        {
            Debug.Log(collider.gameObject.name);
            if(collider.tag.Equals("Enemy"))
            {
                EnemyStats enemyStats = collider.GetComponent<EnemyStats>();
                if(enemyStats != null)
                {
                    enemyStats.TakeDamage(currentWeaponDamage);
                }
            }
        }
    }
}
