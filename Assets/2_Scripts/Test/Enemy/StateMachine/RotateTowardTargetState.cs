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
            float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);
            
            //When we enter the state we will still be interacting from the attack animation
            //So, We pause here until it has finished
            if(enemyManager.isInteracting)
                return this; 

            if(viewableAngle > 120 & viewableAngle <= 180 && !enemyManager.isInteracting)
            {
                Debug.Log("Turn Left 180");
                enemyAnimatorHandler.PlayTargetAnimationRootRotation("Turn Left 180", isInteracting: true); 
                //HandleRotateTowardsTarget(enemyManager);  
                return this;
            }
            else if(viewableAngle < -120 && viewableAngle >= -180 && !enemyManager.isInteracting)
            {
                Debug.Log("Turn Right 180");
                enemyAnimatorHandler.PlayTargetAnimationRootRotation("Turn Right 180", isInteracting: true);
                //HandleRotateTowardsTarget(enemyManager);  
                return this;
            }
            else if(viewableAngle < -60 && viewableAngle >= -120 & !enemyManager.isInteracting)
            {
                Debug.Log("Turn Right");
                enemyAnimatorHandler.PlayTargetAnimationRootRotation("Turn Right", isInteracting: true);
                //HandleRotateTowardsTarget(enemyManager);  
                return this;
            }
            else if(viewableAngle > 60 && viewableAngle <= 120 & !enemyManager.isInteracting)
            {
                Debug.Log("Turn Left");
                enemyAnimatorHandler.PlayTargetAnimationRootRotation("Turn Left", isInteracting: true);
                //HandleRotateTowardsTarget(enemyManager);  
                return this;
            }

            return combatStanceState;
        }

        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction.Equals(Vector3.zero))
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
    }
}