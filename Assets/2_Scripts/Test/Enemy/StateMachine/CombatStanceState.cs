using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class CombatStanceState : State
    {
        public ChaseTargetState chaseTargetState;
        public AttackState attackState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            
            //플레이어 주변을 서성이며 공격할 준비를 함     

            // 공격 사거리를 체크
            enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            if(enemyManager.currentRecoveryTime <= 0 && enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
            {   //공격 사거리 안에 들어오면 AttackState를 리턴
                return attackState;
            }
            else if( enemyManager.distanceFromTarget > enemyManager.maximumAttackRange)
            {   //만약 플레이어가 사거리를 벗어나면 ChaseTargetState로 리턴
                return chaseTargetState;
            }
            else //공격이후 쿨다운이면, 계속 이 State를 리턴
                return this;
        }
    }
}
