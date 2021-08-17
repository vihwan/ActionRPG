using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerStats : MonoBehaviour
    {

        [Header("Basics")]
        public string playerName = "Diluc";
        public int playerLevel;
        public int playerExp;

        [Header("Status")]
        [Tooltip("Maxhealth = healthLevel * 10")]
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;
        public int attack;
        public int defense;
        public int critical;
        public float criticalDamage;
        public int stamina;


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
            InitializeStatusSet();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar = FindObjectOfType<HealthBar>();
            if (healthBar != null)
                healthBar.SetMaxHealth(maxHealth);
        }

        private void InitializeStatusSet()
        {
            playerLevel = 1;
            playerExp = 30;
            attack = 5;
            defense = 3;
            critical = 5;
            criticalDamage = 1.5f;
            stamina = 100;
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

            if (playerManager.isUnEquip == false)
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
