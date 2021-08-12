using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        private HealthBar healthBar;
        private AnimatorHandler animatorHandler;
        private PlayerManager playerManager;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerManager = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar = FindObjectOfType<HealthBar>();
            if(healthBar != null)
                healthBar.SetMaxHealth(maxHealth);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;
            healthBar.SetCurrentHealth(currentHealth);

            if(playerManager.isUnEquip == false)
                animatorHandler.PlayTargetAnimation("Damage_01", true);
            else
                animatorHandler.PlayTargetAnimation("Damage_01_UnEquip", true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;

                if (playerManager.isUnEquip == false)
                    animatorHandler.PlayTargetAnimation("Damage_Die", true);
                else
                    animatorHandler.PlayTargetAnimation("Damage_Die_UnEquip", true);
                //Handle Player Death
            }
        }
    }
}
