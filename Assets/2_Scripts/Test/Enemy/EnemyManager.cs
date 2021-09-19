using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SG
{
    public class EnemyManager : CharacterManager
    {
        [Header("Basic")]
        [SerializeField] public string enemyName;

        [Header("A.I Status")]
        public PlayerStats currentTarget;
        [SerializeField] internal bool isPerformingAction;
        [SerializeField] public bool isInteracting;
        [SerializeField] public float detectionRadius;
        [SerializeField] public float rotationSpeed = 15f;
        [SerializeField] public float maximumAttackRange = 2f;

        [Tooltip("시야각을 현실적이게 설정하기를 요망"), SerializeField] internal float minimumDetectionAngle = -50f;
        [Tooltip("시야각을 현실적이게 설정하기를 요망"), SerializeField] internal float maximumDetectionAngle = 50f;

        public float currentRecoveryTime = 0;

        public State currentState;

        [Header("Need Component")]
        [SerializeField] private EnemyLocomotionManager enemyLocomotionManager;
        [SerializeField] private EnemyAnimatorHandler enemyAnimatorHandler;
        [SerializeField] private EnemyInventory enemyInventory;
        [SerializeField] private EnemyStats enemyStats;
        [SerializeField] private EnemyWeaponSlotManager enemyWeaponSlotManager;
        public NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidbody;


        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            if(enemyLocomotionManager != null)
                enemyLocomotionManager.Init();

            enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
            if (enemyAnimatorHandler != null)
                enemyAnimatorHandler.Init();

            enemyInventory = GetComponent<EnemyInventory>();
            if (enemyInventory != null)
                enemyInventory.Init();

            enemyStats = GetComponent<EnemyStats>();
            if (enemyStats != null)
                enemyStats.Init();
            
            enemyWeaponSlotManager = GetComponentInChildren<EnemyWeaponSlotManager>();
            if(enemyWeaponSlotManager != null)
                enemyWeaponSlotManager.Init();

            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidbody = GetComponent<Rigidbody>();

            lockOnTransform = UtilHelper.Find<Transform>(transform, "LockOnTransform").transform;

            currentState = GetComponentInChildren<AmbushState>();

            navMeshAgent.enabled = false;
            enemyRigidbody.isKinematic = false;
        }

        private void OnDrawGizmos() {
            
        }

        private void FixedUpdate()
        {
            HandleState();
        }

        private void Update()
        {
            HandleRecoveryTimer();
            isInteracting = enemyAnimatorHandler.anim.GetBool("isInteracting");
        }

        private void HandleState()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorHandler);
                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction)
            {
                if (currentRecoveryTime <= 0f)
                {
                    isPerformingAction = false;
                }
            }
        }

        //몬스터 사망 시 실행되는 메소드
        public void Die()
        {
            enemyLocomotionManager.EnableFalseAllCollider();
            PlayerQuestInventory.Instance.SetRecentKilledEnemy(this);
            //드롭 아이템 생성
            enemyInventory.CreateDropItem();

            //플레이어 경험치 획득
            LevelManager.Instance.AddExperience(enemyInventory.Exp);
            Destroy(this.gameObject, 7f);
        }
    }
}