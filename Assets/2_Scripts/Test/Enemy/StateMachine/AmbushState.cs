using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class AmbushState : State
    {
        public bool isSleeping;
        public string sleepAnimation;
        public string wakeAnimation;
        public LayerMask detectionLayer;
        public ChaseTargetState chaseTargetState;


        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            if (enemyManager.currentTarget == null)
            {
                if (isSleeping && enemyManager.isInteracting.Equals(false))
                {
                    enemyAnimatorHandler.PlayTargetAnimation(sleepAnimation, true);
                }

                HandleTargetDetection(enemyManager, enemyAnimatorHandler);
            }

            if (enemyManager.currentTarget != null && !isSleeping)
            {
                // if(enemyAnimatorHandler.anim.GetCurrentAnimatorStateInfo(enemyAnimatorHandler.anim.GetLayerIndex("Action Layer")).IsName("WakeUp"))
                //     return this;
                return chaseTargetState;
            }
            else
                return this;
        }

        private void HandleTargetDetection(EnemyManager enemyManager, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, enemyManager.detectionRadius, detectionLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                PlayerStats playerStats = colliders[i].transform.GetComponent<PlayerStats>();

                if (playerStats != null)
                {
                    Vector3 targetDirection = playerStats.transform.position - enemyManager.transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle
                        && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = playerStats;
                        enemyAnimatorHandler.PlayTargetAnimation(wakeAnimation, true);
                        Invoke(nameof(IsSleepingFalse), 2.7f);
                        return;
                    }
                }
            }
        }

        private void IsSleepingFalse()
        {
            isSleeping = false;
        }
    }
}
