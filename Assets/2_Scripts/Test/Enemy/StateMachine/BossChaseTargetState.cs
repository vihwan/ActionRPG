using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{

    public class BossChaseTargetState : ChaseTargetState
    {
        [Header("Second Phase Attack")]
        public bool hasPhaseShifted;
        public EnemyAttackAction[] secondPhaseDashAttacks;

        protected override void GetNewDashAttack(EnemyManager enemyManager, float distanceFromTarget)
        {
            if (hasPhaseShifted)
            {
                if (secondPhaseDashAttacks.Length > 0)
                {
                    int rand = Random.Range(0, secondPhaseDashAttacks.Length);
                    attackState.currentAttack = secondPhaseDashAttacks[rand];
                }
            }
            else
            {
                base.GetNewDashAttack(enemyManager, distanceFromTarget);
            }
        }
    }
}
