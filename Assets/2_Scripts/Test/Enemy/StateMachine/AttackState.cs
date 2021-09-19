using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class AttackState : State
    {
        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;

        public CombatStanceState combatStanceState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            //공격시행
            //만약 공격 각도 혹은 사거리가 부족하여 공격을 할 수 없다면, 새로운 공격 애니메이션을 선택
            //공격이 가능하면, 움직임을 멈추고 상대를 공격
            //공격 회복 시간을 설정
            //CombatStanceState로 return
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - this.transform.position;
            float viewableAngle = Vector3.Angle(targetDirection,transform.forward);

            if (enemyManager.isPerformingAction)
                return combatStanceState;



            if (currentAttack != null)
            {
                if (enemyManager.distanceFromTarget < currentAttack.minimumDistanceNeedToAttack)
                {
                    return this;
                }
                else if (enemyManager.distanceFromTarget < currentAttack.maximumDistanceNeedToAttack)
                {
                    if (enemyManager.viewableAngle <= currentAttack.maximumAttackAngle &&
                       enemyManager.viewableAngle >= currentAttack.minimumAttackAngle)
                    {
                        if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerformingAction.Equals(false))
                        {
                            enemyAnimatorHandler.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                            enemyAnimatorHandler.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                            enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
                            enemyManager.isPerformingAction = true;
                            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                            currentAttack = null;
                            return combatStanceState;
                        }
                    }
                }
            }
            else
            {
                GetNewAttack(enemyManager);
            }

            //AttackTarget(enemyManager, enemyAnimatorHandler);         
            return combatStanceState;
        }

        #region Attacks

        private void AttackTarget(EnemyManager enemyManager, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            if (enemyManager.isPerformingAction)
                return;

            if (currentAttack == null)
            {
                GetNewAttack(enemyManager);
            }
            else
            {
                enemyManager.isPerformingAction = true;
                enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
                currentAttack = null;
            }
        }
        
        private void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetsDirection = enemyManager.currentTarget.transform.position - this.transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

            int maxScore = 0;
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeedToAttack
                    && enemyManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeedToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int tempScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyManager.distanceFromTarget <= enemyAttackAction.maximumDistanceNeedToAttack
                    && enemyManager.distanceFromTarget >= enemyAttackAction.minimumDistanceNeedToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (currentAttack != null)
                            return;

                        tempScore += enemyAttackAction.attackScore;

                        if (tempScore > randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
