using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class BossCombatStanceState : CombatStanceState
    {
        [Header("Second Phase Attack")]
        public bool hasPhaseShifted;
        public EnemyAttackAction[] secondPhaseAttacks;

        protected override void GetNewAttack(EnemyManager enemyManager)
        {
            if (hasPhaseShifted)
            {
                Vector3 targetsDirection = enemyManager.currentTarget.transform.position - this.transform.position;
                float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
                float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

                int maxScore = 0;
                for (int i = 0; i < secondPhaseAttacks.Length; i++)
                {
                    EnemyAttackAction enemyAttackAction = secondPhaseAttacks[i];

                    if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeedToAttack
                        && distanceFromTarget >= enemyAttackAction.minimumDistanceNeedToAttack)
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

                for (int i = 0; i < secondPhaseAttacks.Length; i++)
                {
                    EnemyAttackAction enemyAttackAction = secondPhaseAttacks[i];

                    if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeedToAttack
                        && distanceFromTarget >= enemyAttackAction.minimumDistanceNeedToAttack)
                    {
                        if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                            && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                        {
                            if (attackState.currentAttack != null)
                                return;

                            tempScore += enemyAttackAction.attackScore;

                            if (tempScore > randomValue)
                            {
                                attackState.currentAttack = enemyAttackAction;
                                enemyDamageCollider.attackScore = attackState.currentAttack.attackScore;
                            }
                        }
                    }
                }
            }
            else
            {
                base.GetNewAttack(enemyManager);
            }
        }

    }
}