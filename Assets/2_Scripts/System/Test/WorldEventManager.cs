using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class WorldEventManager : MonoBehaviourSingleton<WorldEventManager>
    {
        private EnemyBossHealthBarUI enemyBossHealthBarUI;

        public bool bossFightIsActive;
        public bool bossIsAwakened;
        public bool bossIsDefeated;

        public void Init()
        {
            enemyBossHealthBarUI = FindObjectOfType<EnemyBossHealthBarUI>();
        }

        public void ActiveBossFight()
        {
            bossFightIsActive = true;
            bossIsAwakened = true;
            enemyBossHealthBarUI.SetActiveEnemyBossHealthBar(true);
        }

        public void IsDefeatedBoss()
        {
            bossFightIsActive = false;
            bossIsAwakened = false;
            bossIsDefeated = true;
            enemyBossHealthBarUI.SetActiveEnemyBossHealthBar(false);
        }
    }
}
