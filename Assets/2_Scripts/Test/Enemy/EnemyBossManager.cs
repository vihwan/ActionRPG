using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    //적 게임 오브젝트가 보스 몬스터라면, 이 컴포넌트를 추가하면 되도록 설정
    [RequireComponent(typeof(EnemyManager))]
    [RequireComponent(typeof(EnemyStats))]
    public class EnemyBossManager : MonoBehaviour
    {
        //Handle Switch Phase
        //Handle Switch Attack Patterns
        [SerializeField] private bool hasPhaseShifted;

        [Header("Need Component")]
        [SerializeField] private EnemyBossHealthBarUI enemyBossHealthBarUI;
        [SerializeField] private EnemyManager enemyManager;
        [SerializeField] private EnemyStats enemyStats;
        [SerializeField] private EnemyAnimatorHandler enemyAnimatorManager;
        [SerializeField] private BossChaseTargetState bossChaseTargetState;
        [SerializeField] private BossCombatStanceState bossCombatStanceState;
        [SerializeField] private BossFXTransform bossFXTransform;

        public bool HasPhaseShifted { 
            get => hasPhaseShifted; 
            set 
            {
                hasPhaseShifted = value;
                bossChaseTargetState.hasPhaseShifted = value;
                bossCombatStanceState.hasPhaseShifted = value;
            }
        }

        private void Start() 
        {
            enemyBossHealthBarUI = FindObjectOfType<EnemyBossHealthBarUI>();
            enemyManager = GetComponent<EnemyManager>();
            if(enemyManager != null)
                enemyBossHealthBarUI.bossNameText.text = enemyManager.enemyName;

            enemyStats = GetComponent<EnemyStats>();
            if(enemyStats != null)
                enemyBossHealthBarUI.SetBossMaxHealth(enemyStats.maxHealth);

            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorHandler>();
            bossChaseTargetState = GetComponentInChildren<BossChaseTargetState>();
            bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
            bossFXTransform = GetComponentInChildren<BossFXTransform>();
            if (bossFXTransform != null)
                bossFXTransform.Init();
        }

        public void UpdateBossHealthBar(int currentHealth, int maxHealth)
        {
            enemyBossHealthBarUI.SetBossCurrentHealth(currentHealth);
            if (currentHealth <= maxHealth / 2 && !HasPhaseShifted)
            {
                ShiftToSecondPhase();
            }
        }

        public void ShiftToSecondPhase()
        {
            //Play an Animation /w An Event That Trigger Particle Fx / Weapon Fx
            //Switch Attack Actions
            enemyAnimatorManager.anim.SetBool("isPhaseShifting", true);
            enemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);
            HasPhaseShifted = true;
            Debug.Log("Phase Shift!!");
        }
    }
}
