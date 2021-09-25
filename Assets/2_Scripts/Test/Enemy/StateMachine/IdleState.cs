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
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                PlayerStats playerStats = colliders[i].transform.GetComponent<PlayerStats>();

                if (playerStats != null && playerStats.isDead.Equals(false))
                {
                    //check for team id
                    Vector3 targetDirection = playerStats.transform.position - transform.position;
                    float viewableAnlge = Vector3.Angle(targetDirection, transform.forward);
                    if (viewableAnlge > enemyManager.minimumDetectionAngle && viewableAnlge < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = playerStats;                        
                    }
                }
            }
        }
    }
}
