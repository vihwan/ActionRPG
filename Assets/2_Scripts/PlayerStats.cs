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
        [SerializeField] private int currentHealth;
        public int attack;
        public int defense;
        [SerializeField] private int critical;
        public int criticalDamage;
        public int stamina;

        [Header("Need Component")]
        private HealthBar healthBar;
        private AnimatorHandler animatorHandler;
        private PlayerManager playerManager;
        [SerializeField] private PlayerInventory playerInventory;

        public int CurrentHealth
        {
            get => currentHealth;
            private set
            {
                if (value >= maxHealth)
                    maxHealth = value;

                currentHealth = value;
            }
        }
        public int Critical
        {
            get => critical;
            private set
            {
                if (value >= 100)
                    value = 100;

                critical = value;
            }
        }
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
            if (healthBar != null)
                healthBar.SetMaxHealth(maxHealth);
            playerInventory = GetComponent<PlayerInventory>();
            InitializeStatusSet();
        }

        private void InitializeStatusSet()
        {
            playerLevel = 1;
            playerExp = 30;
            attack = 5;
            defense = 3;
            critical = 5;
            criticalDamage = 150;
            stamina = 100;

            UpdatePlayerStatus_Initialize();
        }

        public void UpdatePlayerStatus_Initialize()
        {
            if (playerInventory.currentWeapon != null)
            {
                maxHealth += playerInventory.currentWeapon.itemAttributes[(int)Attribute.Hp].value;
                currentHealth += playerInventory.currentWeapon.itemAttributes[(int)Attribute.Hp].value;
                attack += playerInventory.currentWeapon.itemAttributes[(int)Attribute.Attack].value;
                defense += playerInventory.currentWeapon.itemAttributes[(int)Attribute.Defense].value;
                critical += playerInventory.currentWeapon.itemAttributes[(int)Attribute.Critical].value;
                criticalDamage += playerInventory.currentWeapon.itemAttributes[(int)Attribute.CriticalDamage].value;
                stamina += playerInventory.currentWeapon.itemAttributes[(int)Attribute.Stamina].value;
            }
        }

        public void UpdatePlayerStatus_Equip(WeaponItem currentWeapon)
        {
            if (playerInventory.currentWeapon != null)
            {
                maxHealth += currentWeapon.itemAttributes[(int)Attribute.Hp].value;
                currentHealth += currentWeapon.itemAttributes[(int)Attribute.Hp].value;
                attack += currentWeapon.itemAttributes[(int)Attribute.Attack].value;
                defense += currentWeapon.itemAttributes[(int)Attribute.Defense].value;
                critical += currentWeapon.itemAttributes[(int)Attribute.Critical].value;
                criticalDamage += currentWeapon.itemAttributes[(int)Attribute.CriticalDamage].value;
                stamina += currentWeapon.itemAttributes[(int)Attribute.Stamina].value;
            }
        }

        public void UpdatePlayerStatus_UnEquip(WeaponItem currentWeapon)
        {
            if (playerInventory.currentWeapon != null)
            {
                maxHealth -= currentWeapon.itemAttributes[(int)Attribute.Hp].value;
                currentHealth -= currentWeapon.itemAttributes[(int)Attribute.Hp].value;
                attack -= currentWeapon.itemAttributes[(int)Attribute.Attack].value;
                defense -= currentWeapon.itemAttributes[(int)Attribute.Defense].value;
                critical -= currentWeapon.itemAttributes[(int)Attribute.Critical].value;
                criticalDamage -= currentWeapon.itemAttributes[(int)Attribute.CriticalDamage].value;
                stamina -= currentWeapon.itemAttributes[(int)Attribute.Stamina].value;
            }
        }

        public void UpdatePlayerStatus_Equip(EquipItem currentEquipItem)
        {
            maxHealth += currentEquipItem.itemAttributes[(int)Attribute.Hp].value;
            currentHealth += currentEquipItem.itemAttributes[(int)Attribute.Hp].value;
            attack += currentEquipItem.itemAttributes[(int)Attribute.Attack].value;
            defense += currentEquipItem.itemAttributes[(int)Attribute.Defense].value;
            critical += currentEquipItem.itemAttributes[(int)Attribute.Critical].value;
            criticalDamage += currentEquipItem.itemAttributes[(int)Attribute.CriticalDamage].value;
            stamina += currentEquipItem.itemAttributes[(int)Attribute.Stamina].value;

        }

        public void UpdatePlayerStatus_UnEquip(EquipItem currentEquipItem)
        {
            maxHealth -= currentEquipItem.itemAttributes[(int)Attribute.Hp].value;
            currentHealth -= currentEquipItem.itemAttributes[(int)Attribute.Hp].value;
            attack -= currentEquipItem.itemAttributes[(int)Attribute.Attack].value;
            defense -= currentEquipItem.itemAttributes[(int)Attribute.Defense].value;
            critical -= currentEquipItem.itemAttributes[(int)Attribute.Critical].value;
            criticalDamage -= currentEquipItem.itemAttributes[(int)Attribute.CriticalDamage].value;
            stamina -= currentEquipItem.itemAttributes[(int)Attribute.Stamina].value;
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
