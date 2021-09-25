using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{

    //적 게임 오브젝트가 보스 몬스터라면, 이 컴포넌트를 추가하면 되도록 설정
    [RequireComponent(typeof(EnemyManager))]
    [RequireComponent(typeof(EnemyStats))]
    public class EnemyBossManager : MonoBehaviour
    {
        //Handle Switch Phase
        //Handle Switch Attack Patterns
        [SerializeField] private EnemyBossHealthBarUI enemyBossHealthBarUI;
        [SerializeField] private EnemyManager enemyManager;
        [SerializeField] private EnemyStats enemyStats;

        private void Start() 
        {
            enemyBossHealthBarUI = FindObjectOfType<EnemyBossHealthBarUI>();
            enemyManager = GetComponent<EnemyManager>();
            if(enemyManager != null)
                enemyBossHealthBarUI.bossNameText.text = enemyManager.enemyName;

            enemyStats = GetComponent<EnemyStats>();
            if(enemyStats != null)
                enemyBossHealthBarUI.SetBossMaxHealth(enemyStats.maxHealth);
        }
    }
}
