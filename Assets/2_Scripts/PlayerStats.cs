using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerStats : MonoBehaviour
    {

        [Header("Status")]
        [Tooltip("Maxhealth Basic = healthLevel * 10")]
        [SerializeField] private int healthLevel = 10;
        [SerializeField] private int maxHealth;
        [SerializeField] private int currentHealth;

        [SerializeField] private int maxMana;
        [SerializeField] private int currentMana;

        [SerializeField] private float maxStamina;
        [SerializeField] private float currentStamina;
        [SerializeField] private float staminaRegenerationAmount = 1f;

        [SerializeField] private int attack;
        [SerializeField] private int defense;
        [SerializeField] private int critical;
        [SerializeField] private int criticalDamage;

        [SerializeField] public bool isDead;

        [Header("Need Component")]
        private HealthBar healthBar;
        private ManaBar manaBar;
        private StaminaBar staminaBar;
        private PlayerAnimatorHandler animatorHandler;
        private PlayerManager playerManager;
        private InputHandler inputHandler;
        [SerializeField] private PlayerInventory playerInventory;


        private Action OnTakeDamaged;
        private Action OnUseStamina;

        //Property
        public int MaxHealth
        {
            get => maxHealth;
            private set
            {
                maxHealth = value;
                if (maxHealth < CurrentHealth)
                    CurrentHealth = maxHealth;
            }
        }
        public int CurrentHealth
        {
            get => currentHealth;
            private set
            {
                currentHealth = value;
                if (currentHealth >= MaxHealth)
                    currentHealth = MaxHealth;
            }
        }
        public int MaxMana
        {
            get => maxMana;
            private set
            {
                maxMana = value;
                if (maxMana < CurrentMana)
                    CurrentMana = maxMana;
            }
        }
        public int CurrentMana
        {
            get => currentMana;
            private set
            {
                currentMana = value;
                if (currentMana >= MaxMana)
                    currentMana = MaxMana;
            }
        }
        public float MaxStamina
        {
            get => maxStamina;
            private set
            {
                maxStamina = value;
                if (maxStamina < CurrentStamina)
                    CurrentStamina = maxStamina;
            }
        }
        public float CurrentStamina
        {
            get => currentStamina;
            private set
            {
                currentStamina = value;
                if (currentStamina >= MaxStamina)
                    currentStamina = MaxStamina;
            }
        }
        public int Attack { get => attack; private set => attack = value; }
        public int Defense { get => defense; private set => defense = value; }
        public int Critical
        {
            get => critical;
            private set
            {
                critical = value;
                if (critical >= 100)
                    critical = 100;
            }
        }
        public int CriticalDamage { get => criticalDamage; private set => criticalDamage = value; }


        public void Init()
        {
            animatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
            playerManager = GetComponent<PlayerManager>();
            inputHandler = GetComponent<InputHandler>();
            healthBar = FindObjectOfType<HealthBar>();
            manaBar = FindObjectOfType<ManaBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            playerInventory = GetComponent<PlayerInventory>();

            MaxHealth = healthLevel * 10;
            CurrentHealth = MaxHealth;
            MaxMana = 100;
            MaxStamina = 100;

            InitializeStatusSet();
            UpdateAllStatusText();

            OnTakeDamaged += HealthBar_OnTakeDamaged;
            OnUseStamina += StaminaBar_OnUseStamina;
        }



        private void InitializeStatusSet()
        {
            //Player Default Status
            Attack = 5;
            Defense = 3;
            Critical = 5;
            CriticalDamage = 150;
            CurrentStamina = 100;
            CurrentMana = 100;

            //Plus Player Stats depend on Equipping Items.
            UpdatePlayerStatus_Initialize();
            healthBar.SetMaxHealth(MaxHealth);
            manaBar.SetMaxMana(MaxMana);
            staminaBar.SetMaxStamina(MaxStamina);
        }


        private void HealthBar_OnTakeDamaged()
        {
            healthBar.SetCurrentHealth(CurrentHealth);
            healthBar.SetHealthText(CurrentHealth, MaxHealth);
        }

        private void StaminaBar_OnUseStamina()
        {
            staminaBar.SetCurrentStamina(CurrentStamina);
            staminaBar.SetStaminaText(CurrentStamina, MaxStamina);
        }


        public void UseStamina(int amount)
        {
            CurrentStamina -= amount;
            OnUseStamina?.Invoke();
        }

        public void RegenerationStamina()
        {
            if (!playerManager.isInteracting)
            {
                if (CurrentStamina < MaxStamina)
                {
                    CurrentStamina += staminaRegenerationAmount * Time.deltaTime;
                    OnUseStamina?.Invoke();
                }
            }
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
                MaxHealth += currentWeapon.itemAttributes[(int)Attribute.Hp].value;
                CurrentHealth = MaxHealth;
                Attack += currentWeapon.itemAttributes[(int)Attribute.Attack].value;
                Defense += currentWeapon.itemAttributes[(int)Attribute.Defense].value;
                Critical += currentWeapon.itemAttributes[(int)Attribute.Critical].value;
                CriticalDamage += currentWeapon.itemAttributes[(int)Attribute.CriticalDamage].value;
                MaxStamina += currentWeapon.itemAttributes[(int)Attribute.Stamina].value;
                CurrentStamina = MaxStamina;
            }
        }

        public void UpdatePlayerStatus_UnEquip(WeaponItem currentWeapon)
        {
            if (playerInventory.currentWeapon != null)
            {
                MaxHealth -= currentWeapon.itemAttributes[(int)Attribute.Hp].value;
                CurrentHealth = MaxHealth;
                Attack -= currentWeapon.itemAttributes[(int)Attribute.Attack].value;
                Defense -= currentWeapon.itemAttributes[(int)Attribute.Defense].value;
                Critical -= currentWeapon.itemAttributes[(int)Attribute.Critical].value;
                CriticalDamage -= currentWeapon.itemAttributes[(int)Attribute.CriticalDamage].value;
                MaxStamina -= currentWeapon.itemAttributes[(int)Attribute.Stamina].value;
                CurrentStamina = MaxStamina;
            }
        }

        public void UpdatePlayerStatus_Equip(EquipItem currentEquipItem)
        {
            MaxHealth += currentEquipItem.itemAttributes[(int)Attribute.Hp].value;
            CurrentHealth = MaxHealth;
            Attack += currentEquipItem.itemAttributes[(int)Attribute.Attack].value;
            Defense += currentEquipItem.itemAttributes[(int)Attribute.Defense].value;
            Critical += currentEquipItem.itemAttributes[(int)Attribute.Critical].value;
            CriticalDamage += currentEquipItem.itemAttributes[(int)Attribute.CriticalDamage].value;
            MaxStamina += currentEquipItem.itemAttributes[(int)Attribute.Stamina].value;
            CurrentStamina = MaxStamina;
        }

        public void UpdatePlayerStatus_UnEquip(EquipItem currentEquipItem)
        {
            MaxHealth -= currentEquipItem.itemAttributes[(int)Attribute.Hp].value;
            CurrentHealth = MaxHealth;
            Attack -= currentEquipItem.itemAttributes[(int)Attribute.Attack].value;
            Defense -= currentEquipItem.itemAttributes[(int)Attribute.Defense].value;
            Critical -= currentEquipItem.itemAttributes[(int)Attribute.Critical].value;
            CriticalDamage -= currentEquipItem.itemAttributes[(int)Attribute.CriticalDamage].value;
            MaxStamina -= currentEquipItem.itemAttributes[(int)Attribute.Stamina].value;
            CurrentStamina = MaxStamina;
        }

        public void SetMaxStatusBar()
        {
            healthBar.SetMaxHealth(MaxHealth);
            manaBar.SetMaxMana(MaxMana);
            staminaBar.SetMaxStamina(MaxStamina);

            UpdateAllStatusText();
        }

        public void UpdateAllStatusText()
        {
            healthBar.SetHealthText(CurrentHealth, MaxHealth);
            manaBar.SetManaText(CurrentMana, MaxMana);
            staminaBar.SetStaminaText(CurrentStamina, MaxStamina);
        }

        public void SetStatusByLevelUp()
        {
            MaxHealth += Mathf.RoundToInt(10 * Mathf.Sqrt(LevelManager.Instance.Level + 1));
            CurrentHealth = MaxHealth;
            Attack += Mathf.RoundToInt(Mathf.Sqrt(LevelManager.Instance.Level + 1));
            defense += Mathf.RoundToInt(Mathf.Sqrt(LevelManager.Instance.Level + 1));
            MaxStamina += Mathf.RoundToInt(Mathf.Sqrt(LevelManager.Instance.Level + 1));
            CurrentStamina = MaxStamina;
        }

        public void TakeDamage(int damage, int attackScore)
        {
            if (playerManager.isInvulnerable) //????????????
                return;

            if (isDead)
                return;

            if(playerManager.isUnEquip)
            {
               playerManager.ChangePlayerMotionToEquip();
            }

            PlayTakeDamageAnimation(ref damage, attackScore);

            CurrentHealth -= damage;
            OnTakeDamaged?.Invoke();
            Debug.Log(damage + "??? ???????????? ?????????.");

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                isDead = true;

                if (playerManager.isUnEquip.Equals(false))
                    animatorHandler.PlayTargetAnimation("Damage_Die", true);
                else
                    animatorHandler.PlayTargetAnimation("Damage_Die_UnEquip", true);
                
                //Handle Player Death
                playerManager.SetAllMonsterCurrentTargetToNull();
            }
        }

        private void PlayTakeDamageAnimation(ref int damage, int attackScore)
        {
            animatorHandler.anim.SetBool("isDamaged", true);
            if(playerManager.isFalldown)
            {
                animatorHandler.PlayTargetAnimation("Falldown_Damage", isInteracting: true);
                return;
            }

            if(animatorHandler.anim.GetBool("isBlocking").Equals(true))
            {
               damage = Mathf.RoundToInt(damage * 0.5f);
               animatorHandler.PlayTargetAnimation("BlockAccept", isInteracting: true);
               return;
            }

            inputHandler.StopMovement();
            if(attackScore == 3)
            {        
                //FallDown
                // animatorHandler.anim.SetInteger("DamageScore", 3);
                // animatorHandler.anim.SetTrigger("isDamage");
                animatorHandler.PlayTargetAnimation("Damage_03", isInteracting: true);
                animatorHandler.anim.SetBool("isFalldown", true);
                return;
            }
            else if(attackScore == 2)
            {
                //LargeHit
                // animatorHandler.anim.SetInteger("DamageScore", 2);
                // animatorHandler.anim.SetTrigger("isDamage");
                animatorHandler.PlayTargetAnimation("Damage_02", isInteracting: true);
                return;
            }

            // animatorHandler.anim.SetInteger("DamageScore", 1);
            // animatorHandler.anim.SetTrigger("isDamage");
            animatorHandler.PlayTargetAnimation("Damage_01", isInteracting: true);
        }

        public void PlusStatsByComsumableItem(ConsumableItem consumableItem)
        {
            CurrentHealth += consumableItem.consumableAttributes[(int)ConsumeAttribute.Hp].value;
            CurrentMana += consumableItem.consumableAttributes[(int)ConsumeAttribute.Mp].value;

            //????????? UI ??????
            healthBar.SetCurrentHealth(CurrentHealth);
            manaBar.SetCurrentMana(CurrentMana);

            UpdateAllStatusText();
        }

        public bool GetCurrentHealthEqualsMaxHealth()
        {
            if (CurrentHealth == MaxHealth)
                return true;
            else
                return false;
        }
    }
}
