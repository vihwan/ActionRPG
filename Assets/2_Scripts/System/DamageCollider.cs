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
        private PlayerManager playerManager;
        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
            playerStats = FindObjectOfType<PlayerStats>();
            playerManager = playerStats.GetComponent<PlayerManager>();
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
                EnemyManager enemyManager = collider.GetComponent<EnemyManager>();
                EnemyStats enemyStats = collider.GetComponent<EnemyStats>();
                if(enemyStats != null)
                {
                    //무기가 컬라이더의 어느 부분에 먼저 닿는지를 찾는 함수를 사용한다.
                    Vector3 contactPoint = collider.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    float directionHitFrom = Vector3.SignedAngle(playerManager.transform.forward, enemyManager.transform.forward, Vector3.up);
                    ChooseWhichDirectionDamageCameFrom(directionHitFrom);

                    //타격판정 이펙트 출력 함수.
                    //enemyEffects.PlayBloodSplatterFX(contactPoint);

                    enemyStats.TakeDamage(currentWeaponDamage);
                }
            }
        }
    
        protected virtual void CheckForParry(CharacterManager enemyManager)
        {
            // if(enemyManager.isParrying)
            // {
            //     enemyManager.GetComponentInChildren<EnemyAnimatorHandler>().PlayTargetAnimation("Parried", isInteracting: true);
            //     return;
            // }
        }

        // protected virtual void CheckForBlock(CharacterManager enemyManager, EnemyStats enemyStats, BlockingCollider shield)
        // {
        //     if(shield != null && enemyManager.isBlocking)
        //     {
        //         float physicalDamageAfterBlock = physicalDamage - (physicalDamage * shield.blockPhysicalDamageAbsorption) / 100;
                
        //         if(enemyStats != null)
        //         {
        //             enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock));
        //             return;
        //         }
        //     }
        // }
        protected virtual void ChooseWhichDirectionDamageCameFrom(float direction)
        {
            if(direction >= 145 && direction <= 180)
            {
                //currentDamageAnimation = "Damage_Forward_01";

            }
            else if(direction <= -145 && direction >= -180)
            {
                //currentDamageAnimation = "Damage_Forward_01";
                
            }
            else if(direction >= -45 && direction <= 45)
            {
                //currentDamageAnimation = "Damage_Back_01";
                
            }
            else if(direction >= -144 && direction <= -45)
            {
                //currentDamageAnimation = "Damage_Right_01";
                
            }
            else if(direction >= 45 && direction <= 144)
            {
                //currentDamageAnimation = "Damage_Left_01";
                
            }
        }
    }
}
