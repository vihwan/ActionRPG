using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class IdleState : State
    {
        public ChaseTargetState chaseTargetState;
        public LayerMask detectionLayer;
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            //Look for potential target
            //switch to the chase target state if target is found
            enemyAnimatorHandler.anim.SetFloat("Horizontal", 0f, 0.2f, Time.deltaTime);
            enemyAnimatorHandler.anim.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
            HandleDetection(enemyManager);

            if(enemyManager.currentTarget != null)
            {
                Debug.Log("Change to ChaseTargetState");
                return chaseTargetState;
            }
            else
                return this;
        }

        private void HandleDetection(EnemyManager enemyManager)
        {
            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, enemyManager.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                PlayerStats playerStats = colliders[i].transform.GetComponent<PlayerStats>();

                if (playerStats != null && playerStats.isDead.Equals(false))
                {
                    //check for team id
                    Vector3 targetDirection = playerStats.transform.position - enemyManager.transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = playerStats;
                        Debug.Log("목표 발견 : " + enemyManager.currentTarget);
                        return;                
                    }
                }
            }
        }
    }
}
