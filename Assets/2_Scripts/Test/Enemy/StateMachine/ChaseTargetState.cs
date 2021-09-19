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
                return this;

            //대상을 추적
            //만약 공격사거리 안에 적이 들어오면 CombatStanceState로 전환
            HandleMoveToTarget(enemyManager, enemyAnimatorHandler);

            if (enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                return combatStanceState;
            }

            return this;
        }

        public void HandleMoveToTarget(EnemyManager enemyManager, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            if (enemyManager.isPerformingAction)
                return;

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - this.transform.position;
            enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, this.transform.position);
            float viewableAnlge = Vector3.Angle(targetDirection, transform.forward);

            if (enemyManager.distanceFromTarget > enemyManager.maximumAttackRange)
            {
                enemyAnimatorHandler.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }
            // else if (distanceFromTarget <= enemyManager.maximumAttackRange)
            // {
            //     enemyAnimatorHandler.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            //     // enemyRigidbody.velocity = Vector3.zero;
            // }

            HandleRotateTowardsTarget(enemyManager);
            enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
            enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            if (enemyManager.isPerformingAction)
                return;

            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - this.transform.position;
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
                Debug.Log(enemyManager.enemyRigidbody.velocity);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }
    }
}