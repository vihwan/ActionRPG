using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerStats : MonoBehaviour
    {

        [Header("Basics")]
        public readonly string playerName = "Diluc";
        [SerializeField] private int playerLevel;
        [SerializeField] private int playerExp;

        [Header("Status")]
        [Tooltip("Maxhealth = healthLevel * 10")]
        [SerializeField] private int healthLevel = 10;
        [SerializeField] private int maxHealth;
        [SerializeField] private int currentHealth;
        [SerializeField] private int attack;
        [SerializeField] private int defense;
        [SerializeField] private int critical;
        [SerializeField] private int criticalDamage;
        [SerializeField] private int stamina;

        [Header("Need Component")]
        private HealthBar healthBar;
        private AnimatorHandler animatorHandler;
        private PlayerManager playerManager;
        [SerializeField] private PlayerInventory playerInventory;

        //Property
        public int PlayerLevel { get => playerLevel; private set => playerLevel = value; }
        public int PlayerExp { get => playerExp; private set => playerExp = value; }
        public int MaxHealth { get => maxHealth; private set => maxHealth = value; }
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
        public int Attack { get => attack; private set => attack = value; }
        public int Defense { get => defense; private set => defense = value; }
        public int CriticalDamage { get => criticalDamage; private set => criticalDamage = value; }
        public int Stamina { get => stamina; private set => stamina = value; }

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
            playerInventory = GetComponent<PlayerInventory>();
            InitializeStatusSet();
        }

        private void InitializeStatusSet()
        {
            //Player Default Status
            playerLevel = 1;
            playerExp = 30;
            attack = 5;
            defense = 3;
            critical = 5;
            criticalDamage = 150;
            stamina = 100;

            //Plus Player Stats depend on Equipping Items.
            UpdatePlayerStatus_Initialize();
            healthBar.SetMaxHealth(maxHealth);
        }

        public void UpdatePlayerStatus_Initialize()
        {
            UpdatePlayerStatus_Equip(playerInventory.currentWeapon);

            for (int i = 0; i < playerInventory.currentEquipmentSlots.Length; i++)
            {
                UpdatePlayerStatus_Equip(playerInventory.currentEquipmentSlots[i]);
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

        public void SetMaxHealthBar()
        {
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
