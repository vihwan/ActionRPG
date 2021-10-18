using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [RequireComponent(typeof(EnemyManager))]
    public class EnemyStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;
        public int attack;
        [SerializeField] internal bool isDead;

        private EnemyAnimatorHandler enemyAnimatorHandler;
        private EnemyHealthBarUI enemyHealthBarUI;
        private EnemyManager enemyManager;
        private EnemyBossManager enemyBossManager;
        private CombatStanceState combatStanceState;
        private AttackState attackState;

        public void Init()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;

            enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>(true);
            enemyManager = GetComponent<EnemyManager>();
            if (enemyManager != null)
            {
                if (enemyManager.isBoss.Equals(true))
                {
                    enemyBossManager = GetComponent<EnemyBossManager>();
                }
                else
                {
                    enemyHealthBarUI = GetComponentInChildren<EnemyHealthBarUI>(true);
                    if (enemyHealthBarUI != null)
                    {
                        enemyHealthBarUI.Init();
                        enemyHealthBarUI.SetMaxHealth(maxHealth);
                    }
                }
            }

            combatStanceState = GetComponentInChildren<CombatStanceState>();
            attackState = GetComponentInChildren<AttackState>();
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {
            if (isDead) 
                return;

            if (enemyManager.isPhaseShifting)
            {
                TakeDamageNoAnimation(damage);
            }
            else
            {
                currentHealth -= damage;
                enemyAnimatorHandler.PlayTargetAnimation(damageAnimation, true);
                Debug.Log(damage + " 데미지를 입힌다!");

                if (enemyManager.isBoss && enemyBossManager != null)
                    enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
                else if (!enemyManager.isBoss)
                    enemyHealthBarUI.SetHealth(currentHealth);

                enemyManager.currentState = combatStanceState;
                attackState.currentAttack = null;
            }       

            if (currentHealth <= 0)
            {
                //Handle Death
                currentHealth = 0;
                isDead = true;
                enemyAnimatorHandler.PlayTargetAnimation("Damage_Die", true);
                enemyManager.Die();
            }
        }

        public void TakeDamageNoAnimation(int damage)
        {
            currentHealth -= damage;
            if (enemyManager.isBoss && enemyBossManager != null)
                enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            else if (!enemyManager.isBoss)
                enemyHealthBarUI.SetHealth(currentHealth);
        }

        public void PlayTakeDamageAnimationByAttackScore()
        {

        }
    }

}
