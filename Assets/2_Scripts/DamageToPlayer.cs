using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class DamageToPlayer : MonoBehaviour
    {
        public int damage = 25;
        public bool canDisableCollider;
        
        float disableColliderTime;
        float elapsedTime = 0f;

        private void Start() 
        {
            ParticleSystem ps = GetComponent<ParticleSystem>();
            if(ps != null) disableColliderTime = ps.main.duration;
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if(playerStats != null)
            {
                playerStats.TakeDamage(damage, 3); //FallDown
                Debug.Log("데미지 입음");
            }
        }

        private void Update() 
        {
            if(canDisableCollider) DisableCollider();
        }

        private void DisableCollider()
        {
           elapsedTime += Time.deltaTime;
           if(elapsedTime >= disableColliderTime)
           {
               elapsedTime = 0f;
               GetComponent<Collider>().enabled = false;
           }
        }
    }
}

