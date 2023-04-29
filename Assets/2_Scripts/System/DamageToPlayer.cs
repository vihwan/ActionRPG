using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class DamageToPlayer : MonoBehaviour
    {
        public int damage = 25;
        [Range(1,3)] public int attackScore = 1;
        public bool canDisableCollider;
        public float colliderStartTime;
        public float colliderDuration;

        bool triggerEnableCollider = false;
        float disableColliderTime;
        float elapsedTime = 0f;
        Collider collider;

        private void Start() 
        {
            ParticleSystem ps = GetComponent<ParticleSystem>();
            collider = GetComponent<Collider>();
            if(colliderStartTime > 0f) collider.enabled = false;
            if(colliderDuration == 0f)
            {
                if(ps != null) disableColliderTime = ps.main.duration;  
            }  
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if(playerStats != null)
            {
                playerStats.TakeDamage(damage, attackScore);
                Debug.Log("데미지 입음");
            }
        }

        private void Update() 
        {
            if(!triggerEnableCollider) CheckColliderEnable();
            else if(triggerEnableCollider && canDisableCollider) DisableCollider();
        }

        private void CheckColliderEnable()
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= colliderStartTime){
                elapsedTime = 0f;
                collider.enabled = true;  
                triggerEnableCollider = true;      
            }
        }

        private void DisableCollider()
        {
           elapsedTime += Time.deltaTime;
           if(elapsedTime >= disableColliderTime)
           {
               elapsedTime = 0f;
               collider.enabled = false;
           }
        }
    }
}

