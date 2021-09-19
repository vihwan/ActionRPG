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
        [SerializeField] private bool isDead;

        private EnemyAnimatorHandler enemyAnimatorHandler;
        private EnemyManager enemyManager;

        public void Init()
        {
            enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
            enemyManager = GetComponent<EnemyManager>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if(isDead)
                return;

            currentHealth = currentHealth - damage;
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
