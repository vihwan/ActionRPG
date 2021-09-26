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
        private EnemyBossHealthBarUI enemyBossHealthBarUI;
        private EnemyManager enemyManager;

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
                    enemyBossHealthBarUI = FindObjectOfType<EnemyBossHealthBarUI>();
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
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (isDead)
                return;

            currentHealth = currentHealth - damage;

            if (enemyManager.isBoss)
                enemyBossHealthBarUI.SetBossCurrentHealth(currentHealth);
            else
                enemyHealthBarUI.SetHealth(currentHealth);

            enemyAnimatorHandler.PlayTargetAnimation("Damage_01", true);
            Debug.Log(damage + " 데미지를 입힌다!");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                enemyAnimatorHandler.PlayTargetAnimation("Damage_Die", true);
                enemyManager.Die();
            }
        }

        public void PlayTakeDamageAnimationByAttackScore()
        {

        }
    }

}
