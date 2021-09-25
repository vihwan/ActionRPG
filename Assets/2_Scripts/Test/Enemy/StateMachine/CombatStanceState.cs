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

        bool randomDestinationSet = false;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            //플레이어 주변을 서성이며 공격할 준비를 함     

            if(enemyManager.currentTarget == null)
                return idleState;

            // 공격 사거리를 체크
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            // if(!randomDestinationSet)
            // {
            //     randomDestinationSet = true;
            //     //Decide Circling Action
            // }

            HandleRotateTowardsTarget(enemyManager);

            if (enemyManager.isPerformingAction)
            {
                enemyAnimatorHandler.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            }
            if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.maximumAttackRange)
            {   //공격 사거리 안에 들어오면 AttackState를 리턴
                //randomDestinationSet = false;
                return attackState;
            }
            else if (distanceFromTarget > enemyManager.maximumAttackRange)
            {   //만약 플레이어가 사거리를 벗어나면 ChaseTargetState로 리턴
                return chaseTargetState;
            }
            else //공격이후 쿨다운이면, 계속 이 State를 리턴
                return this;
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
    
        private void WalkAroundTarget(EnemyAnimatorHandler enemyAnimatorHandler)
        {

        }
    }
}
