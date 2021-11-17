using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class AttackState : State
    {
        public EnemyAttackAction currentAttack;
        public IdleState idleState;
        public ChaseTargetState chaseTargetState;
        public RotateTowardTargetState rotateTowardTargetState;
        public CombatStanceState combatStanceState;
        public EnemyDamageCollider enemyDamageCollider;
    
        [SerializeField] private bool willDoNextCombo = false;
        public bool hasPerformedAttack = false;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            //공격시행
            //만약 공격 각도 혹은 사거리가 부족하여 공격을 할 수 없다면, 새로운 공격 애니메이션을 선택
            //공격이 가능하면, 움직임을 멈추고 상대를 공격
            //공격 회복 시간을 설정
            //CombatStanceState로 return

            if(enemyManager.currentTarget == null)
                return idleState;

            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            RotateTowardsTargetWhileAttacking(enemyManager);

            if(enemyManager.isInteracting.Equals(true) && enemyManager.canDoCombo.Equals(false))
                return this;

            if(willDoNextCombo && enemyManager.canDoCombo)
            {
                // Attack With Combo
                enemyAnimatorHandler.DisableCombo();
                AttackTargetWithCombo(enemyManager, enemyAnimatorHandler);
                RollForComboChange(enemyManager);
                return this;
            }

            if(distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return chaseTargetState;
            }

            if(!hasPerformedAttack && !enemyManager.canDoCombo)
            {
                //Attack
                //Roll for a Combo Chance
                AttackTarget(enemyManager, enemyAnimatorHandler);
                RollForComboChange(enemyManager);
                return this;
            }

            if(willDoNextCombo && hasPerformedAttack)
            {
                return this; //Goes Back up to perform the Combo
            }

            return combatStanceState;
        }



        #region Attacks

        private void AttackTarget(EnemyManager enemyManager, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
            hasPerformedAttack = true;
        }

        private void AttackTargetWithCombo(EnemyManager enemyManager, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            willDoNextCombo = false;
            enemyManager.canDoCombo = false;
            AttackTarget(enemyManager, enemyAnimatorHandler);
        }

        private void RollForComboChange(EnemyManager enemyManager)
        {
            float comboChance = UnityEngine.Random.Range(0f, 100f);
            if(enemyManager.allowAIToPerformCombo && comboChance <= enemyManager.comboLikelyhood)
            {
                enemyDamageCollider.attackScore = currentAttack.attackScore;    
                if(currentAttack.nextComboAction != null)
                {
                    willDoNextCombo = true;
                    currentAttack = currentAttack.nextComboAction;
                    //enemyDamageCollider.attackScore = currentAttack.attackScore;      
                    //Debug.Log("다음 공격 전환");
                }
                else
                {
                    willDoNextCombo = false;
                    currentAttack = null;
                }
            }
        }

        #endregion

        private void RotateTowardsTargetWhileAttacking(EnemyManager enemyManager)
        {
            if (enemyManager.canRotate && enemyManager.isInteracting)
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
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }
    }
}
