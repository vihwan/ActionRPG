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
        [SerializeField] public float detectionRadius;
        [SerializeField] public float distanceFromTarget;
        [SerializeField] public float rotationSpeed = 15f;
        [SerializeField] public float maximumAttackRange = 2f;

        [SerializeField] public float viewableAngle;

        [Tooltip("시야각을 현실적이게 설정하기를 요망"), SerializeField] internal float minimumDetectionAngle = -50f;
        [Tooltip("시야각을 현실적이게 설정하기를 요망"), SerializeField] internal float maximumDetectionAngle = 50f;

        public float currentRecoveryTime = 0;

        public State currentState;

        [Header("Need Component")]
        [SerializeField] private EnemyLocomotionManager enemyLocomotionManager;
        [SerializeField] private EnemyAnimatorHandler enemyAnimatorHandler;
        [SerializeField] private EnemyInventory enemyInventory;
        [SerializeField] private EnemyStats enemyStats;
        public NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidbody;
        public CapsuleCollider capsuleCollider;


        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();

            enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
            if (enemyAnimatorHandler != null)
                enemyAnimatorHandler.Init();

            enemyInventory = GetComponent<EnemyInventory>();
            if (enemyInventory != null)
                enemyInventory.Init();

            enemyStats = GetComponent<EnemyStats>();
            if (enemyStats != null)
                enemyStats.Init();

            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidbody = GetComponent<Rigidbody>();
            capsuleCollider = GetComponent<CapsuleCollider>();

            lockOnTransform = UtilHelper.Find<Transform>(transform, "LockOnTransform").transform;

            currentState = GetComponentInChildren<IdleState>();

            navMeshAgent.enabled = false;
            enemyRigidbody.isKinematic = false;
        }

        private void FixedUpdate()
        {
            HandleState();
        }

        private void Update()
        {
            HandleRecoveryTimer();
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



            // if (enemyLocomotionManager.currentTarget != null)
            // {
            //     enemyLocomotionManager.distanceFromTarget =
            //         Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position, transform.position);
            // }

            // if (enemyLocomotionManager.currentTarget == null)
            // {
            //     enemyLocomotionManager.HandleDetection();
            // }
            // else if (enemyLocomotionManager.distanceFromTarget > enemyLocomotionManager.stoppingDistance)
            // {
            //     enemyLocomotionManager.HandleMoveToTarget();
            // }
            // else if (enemyLocomotionManager.distanceFromTarget <= enemyLocomotionManager.stoppingDistance)
            // {
            //     Handle Our Attacks
            //     AttackTarget();
            // }
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
            capsuleCollider.enabled = false;
            PlayerQuestInventory.Instance.SetRecentKilledEnemy(this);
            //드롭 아이템 생성
            enemyInventory.CreateDropItem();

            //플레이어 경험치 획득
            LevelManager.Instance.AddExperience(enemyInventory.Exp);
            Destroy(this.gameObject, 7f);
        }
    }
}