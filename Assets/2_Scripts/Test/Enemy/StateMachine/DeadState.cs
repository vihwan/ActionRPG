using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class DeadState : State
    {
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
           if(!enemyStats.isDead)
           {
               return this;
           }

           return this;
        }
    }
}
