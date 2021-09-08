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

        private Animator animator;
        private EnemyManager enemyManager;

        public void Init()
        {
            animator = GetComponentInChildren<Animator>();
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
            currentHealth = currentHealth - damage;
            animator.Play("Take Damage");
            Debug.Log(damage + " 데미지를 입힌다!");
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Die");
                enemyManager.Die();
            }
        }
    }

}
