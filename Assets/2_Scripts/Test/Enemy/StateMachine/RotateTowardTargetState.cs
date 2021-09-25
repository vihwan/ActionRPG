using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class RotateTowardTargetState : State
    {
        public CombatStanceState combatStanceState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            enemyAnimatorHandler.anim.SetFloat("Vertical", 0f);
            enemyAnimatorHandler.anim.SetFloat("Horizontal", 0f);

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward,Vector3.up);
            
            if(viewableAngle >= 100 & viewableAngle <= 180 && !enemyManager.isInteracting)
            {
                enemyAnimatorHandler.PlayTargetAnimation("Turn Behind", true);
            return this;
            }

            return this;
        }
    }
}