using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class EnemyManager : CharacterManager
    {
        private CapsuleCollider capsuleCollider;
        private EnemyInventory enemyInventory;
        private EnemyStats enemyStats;
        private void Awake()
        {
            enemyInventory = GetComponent<EnemyInventory>();
            if (enemyInventory != null)
                enemyInventory.Init();

            enemyStats = GetComponent<EnemyStats>();
            if (enemyStats != null)
                enemyStats.Init();

            capsuleCollider = GetComponent<CapsuleCollider>();
        }

        //몬스터 사망 시 실행되는 메소드
        public void Die()
        {
            capsuleCollider.enabled = false;
            enemyInventory.CreateDropItem();
            Destroy(this.gameObject, 7f);
        }
    }
}