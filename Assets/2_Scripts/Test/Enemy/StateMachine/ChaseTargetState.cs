using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class ChaseTargetState : State
    {
        public IdleState idleState;
        public CombatStanceState combatStanceState;
        public RotateTowardTargetState rotateTowardTargetState;
        public AttackState attackState;
        public EnemyAttackAction[] dashAttacks;
        public float viewableAngleLimitation = 90f;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {          
            if (enemyManager.currentTarget == null)
                return idleState;

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

            if(viewableAngle > viewableAngleLimitation || viewableAngle < -viewableAngleLimitation)
            {
                Debug.Log(viewableAngle);
                return rotateTowardTargetState;
            }

            HandleRotateTowardsTarget(enemyManager);

            if (enemyManager.isInteracting.Equals(true))
                return this;

            if (enemyManager.isPerformingAction)
            {
                enemyAnimatorHandler.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                enemyAnimatorHandler.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                return this;
            }
            
            if(distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                enemyAnimatorHandler.anim.SetFloat("Vertical", 2, 0.1f, Time.deltaTime);
                enemyAnimatorHandler.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                return this;
            }

            if(distanceFromTarget <= enemyManager.maximumAggroRadius)
            {
                GetNewDashAttack(enemyManager, distanceFromTarget);
                return combatStanceState;
            }
            
            return this;
        }

        protected virtual void GetNewDashAttack(EnemyManager enemyManager, float distanceFromTarget)
        {
            if (dashAttacks.Length > 0)
            {
                int rand = Random.Range(0, dashAttacks.Length);
                attackState.currentAttack = dashAttacks[rand];
            }   
        }

        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            // if (enemyManager.isPerformingAction)
            // {
            //     Debug.Log("Performing 회전");
            //     Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            //     direction.y = 0;
            //     direction.Normalize();

            //     if (direction.Equals(Vector3.zero))
            //     {
            //         direction = transform.forward;
            //     }

            //     Quaternion targetRotation = Quaternion.LookRotation(direction);
            //     enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            // }
            // else
            // {

            //Rotate With PathFinding
            //Debug.Log("회전한다");
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
            Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

            enemyManager.navMeshAgent.enabled = true;
            enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            enemyManager.enemyRigidbody.velocity = targetVelocity;
            // enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation,
            //                                                    enemyManager.navMeshAgent.transform.rotation,
            //                                                    enemyManager.rotationSpeed / Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);

        }
    }
}