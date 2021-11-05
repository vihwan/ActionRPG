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
            //플레이어의 공격에 따라 출력되는 적의 피격 애니메이션을 다르게 해야한다.
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
