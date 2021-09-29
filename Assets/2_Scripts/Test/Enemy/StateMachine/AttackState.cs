﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class AttackState : State
    {
        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;
        public IdleState idleState;
        public CombatStanceState combatStanceState;
    
        [SerializeField] private bool willDoNextCombo = false;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            //공격시행
            //만약 공격 각도 혹은 사거리가 부족하여 공격을 할 수 없다면, 새로운 공격 애니메이션을 선택
            //공격이 가능하면, 움직임을 멈추고 상대를 공격
            //공격 회복 시간을 설정
            //CombatStanceState로 return
            if (enemyManager.currentTarget == null)
                return idleState;

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

            HandleRotateTowardsTarget(enemyManager);

            if (enemyManager.isInteracting.Equals(true) && enemyManager.canDoCombo.Equals(false))
            {
                return this;
            }

            if (enemyManager.canDoCombo.Equals(true) && willDoNextCombo)
            {
                enemyAnimatorHandler.DisableCombo();
                willDoNextCombo = false;
                AttackTarget(enemyManager, enemyAnimatorHandler);
                if (currentAttack != null)
                {
                    return this;
                }
                else
                {
                    willDoNextCombo = false;
                    return combatStanceState;
                }
            }

            if (enemyManager.isPerformingAction)
                return combatStanceState;


            if (currentAttack != null)
            {
                //Debug.Log(distanceFromTarget);
                if (distanceFromTarget < currentAttack.minimumDistanceNeedToAttack)
                {
                    return this;
                }
                else if (distanceFromTarget < currentAttack.maximumDistanceNeedToAttack)
                {
                    if (viewableAngle <= currentAttack.maximumAttackAngle &&
                        viewableAngle >= currentAttack.minimumAttackAngle)
                    {
                        if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerformingAction.Equals(false))
                        {
                            enemyAnimatorHandler.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                            enemyAnimatorHandler.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                            AttackTarget(enemyManager, enemyAnimatorHandler);
                            if(currentAttack != null) 
                                return this;
                            else 
                                return combatStanceState;
                        }
                    }
                }
                else if(distanceFromTarget > currentAttack.maximumDistanceNeedToAttack)
                {
                    currentAttack = null;
                    return combatStanceState;
                }
            }
            else
            {
                GetNewAttack(enemyManager);
            }

            return combatStanceState;
        }

        #region Attacks

        private void AttackTarget(EnemyManager enemyManager, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            enemyManager.isPerformingAction = true;
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
            //Debug.Log("공격 애니메이션 실행 : " + currentAttack.actionAnimation);
            if (currentAttack.canCombo)
            {
                currentAttack = currentAttack.nextComboAction;
                willDoNextCombo = true;
            }
            else
            {
                enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                currentAttack = null;
            }
        }

        private void GetNewAttack(EnemyManager enemyManager)
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
                        if (currentAttack != null)
                            return;

                        tempScore += enemyAttackAction.attackScore;

                        if (tempScore > randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }
        #endregion

        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {

            if (enemyManager.isPerformingAction)
            {
                //Debug.Log("회전 PerformingAction");
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
                //Debug.Log("회전 else");
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
    }
}
