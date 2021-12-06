using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class CombatStanceState : State
    {
        public IdleState idleState;
        public ChaseTargetState chaseTargetState;
        public AttackState attackState;
        public EnemyDamageCollider enemyDamageCollider;

        public EnemyAttackAction[] enemyAttacks;

        bool randomDestinationSet = false;
        public float verticalMovementValue = 0;
        public float horizontalMovementValue = 0;

        public float randomDestinationTimer = 0f;


        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            //플레이어 주변을 서성이며 공격할 준비를 함   
            
            if(enemyManager.currentTarget == null)
                return idleState;     

             // 공격 사거리를 체크
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            attackState.hasPerformedAttack = false;

            if(enemyManager.isInteracting.Equals(true))
            {
                enemyAnimatorHandler.anim.SetFloat("Vertical", 0f);
                enemyAnimatorHandler.anim.SetFloat("Horizontal", 0f);
                return this;
            }

            if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {   //만약 플레이어가 사거리를 벗어나면 ChaseTargetState로 리턴
                return chaseTargetState;
            }
    
            if (enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                if (distanceFromTarget <= attackState.currentAttack.maximumDistanceNeedToAttack)
                {
                    //randomDestinationSet = false;
                    return attackState;
                }
            }
            else
            {
                GetNewAttack(enemyManager);       
            }  

            HandleRotateTowardsTarget(enemyManager);
            enemyAnimatorHandler.anim.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            enemyAnimatorHandler.anim.SetFloat("Horizontal", horizontalMovementValue,  0.2f, Time.deltaTime);
            //Decide Circling Action;       
            DecideCirclingAction(enemyManager, enemyAnimatorHandler, distanceFromTarget);
  
            return this;       
        }

        protected void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction.Equals(Vector3.zero))
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
            else
            {
                //Rotate With PathFinding
                Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidbody.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation,
                                                                   enemyManager.navMeshAgent.transform.rotation,
                                                                   enemyManager.rotationSpeed / Time.deltaTime);
            }
        }

        protected void DecideCirclingAction(EnemyManager enemyManager, EnemyAnimatorHandler enemyAnimatorHandler, float distanceFromTarget)
        {
            randomDestinationTimer += Time.deltaTime;
            if(randomDestinationTimer >= 1f)
            {
                WalkAroundTarget(enemyManager, enemyAnimatorHandler, distanceFromTarget);
                randomDestinationTimer = 0f;
            }
        }
    
        protected void WalkAroundTarget(EnemyManager enemyManager, EnemyAnimatorHandler enemyAnimatorHandler, float distanceFromTarget)
        {
            // if(distanceFromTarget <= enemyManager.maximumAggroRadius)
            //     verticalMovementValue = Random.Range(-1f, 0f);
            // else
            //     verticalMovementValue = 0f;

            // if (verticalMovementValue <= 1 && verticalMovementValue > 0)
            // {
            //     verticalMovementValue = 0.5f;
            // }
            // else if (verticalMovementValue >= -1 && verticalMovementValue < 0)
            // {
            //     verticalMovementValue = -0.5f;
            // }

            horizontalMovementValue = Random.Range(-1f, 1f);

            if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
            {
                horizontalMovementValue = 0.3f;
            }
            else if (horizontalMovementValue >= 1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -0.3f;
            }
        }

        //구르기 함수

        protected void RollingAroundTarget(EnemyManager enemyManager)
        {
            
        }

        protected virtual void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetsDirection = enemyManager.currentTarget.transform.position - this.transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

            int maxScore = 0;
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeedToAttack
                    && distanceFromTarget >= enemyAttackAction.minimumDistanceNeedToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int tempScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeedToAttack
                    && distanceFromTarget >= enemyAttackAction.minimumDistanceNeedToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (attackState.currentAttack != null)
                            return;

                        tempScore += enemyAttackAction.attackScore;

                        if (tempScore > randomValue)
                        {
                            attackState.currentAttack = enemyAttackAction;
                            enemyDamageCollider.attackScore = enemyAttackAction.attackScore;
                        }
                    }
                }
            }
        }
    }
}
