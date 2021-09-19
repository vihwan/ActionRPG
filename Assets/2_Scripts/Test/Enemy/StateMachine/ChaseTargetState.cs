using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class ChaseTargetState : State
    {
        public CombatStanceState combatStanceState;
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            if (enemyManager.isPerformingAction)
            {
                enemyAnimatorHandler.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            //대상을 추적
            //만약 공격사거리 안에 적이 들어오면 CombatStanceState로 전환
            float distanceFromTarget;
            HandleMoveToTarget(enemyManager, enemyAnimatorHandler, out distanceFromTarget);

            if (distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                return combatStanceState;
            }

            return this;
        }

        public void HandleMoveToTarget(EnemyManager enemyManager, EnemyAnimatorHandler enemyAnimatorHandler, out float distanceFromTarget)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

            if (distanceFromTarget > enemyManager.maximumAttackRange)
            {
                enemyAnimatorHandler.anim.SetFloat("Vertical", 2, 0.1f, Time.deltaTime);
            }

            HandleRotateTowardsTarget(enemyManager);
            enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
            enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
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
    }
}