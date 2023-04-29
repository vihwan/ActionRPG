using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class EnemyDamageCollider : MonoBehaviour
    {
        private Collider damageCollider;
        private int currentWeaponDamage = 0;
        private EnemyStats enemyStats;
        public int attackScore;
        public void Init()
        {
            damageCollider = GetComponent<BoxCollider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;

            enemyStats = GetComponentInParent<EnemyStats>();
            currentWeaponDamage = enemyStats.attack;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                PlayerStats playerStats = other.gameObject.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    PlayerAnimatorHandler playerAnimatorHandler = playerStats.GetComponentInChildren<PlayerAnimatorHandler>();
                    InputHandler inputHandler = playerStats.GetComponent<InputHandler>();
                    if(playerAnimatorHandler.anim.GetBool("canCounter").Equals(true) && inputHandler.counterFlag.Equals(false))
                    {
                        //상대가 카운터 가능 시간대에 공격을 했다면
                        //플레이어가 카운터공격 가능 플래그를 true
                        //데미지를 무효화
                        inputHandler.counterFlag = true;
                        Debug.Log("<color=#E77D00>counterFlag</color> On ");
                        playerStats.TakeDamage(0, attackScore); 
                        return;    
                    }
                    playerStats.TakeDamage(currentWeaponDamage, attackScore);              
                }
                else
                    Debug.Log("PlayerStats is Null");
            }
        }
    }
}
