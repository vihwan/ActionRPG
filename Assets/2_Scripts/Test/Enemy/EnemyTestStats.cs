using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class EnemyTestStats : MonoBehaviour
    {
        public int healthLevel = 1000;
        public int maxHealth;
        public int currentHealth;

        public Material normalMat;
        public Material damageMat;

        public float changeColorTimer = 0f;
        public bool isDamaged = false;

        private MeshRenderer meshRenderer;

        private void Awake() 
        {
            meshRenderer = GetComponent<MeshRenderer>();
            maxHealth = healthLevel * 10;
            currentHealth = maxHealth;         
        }

        private void Update() 
        {
            if (isDamaged.Equals(true))
            {
                changeColorTimer += Time.deltaTime;
                if (changeColorTimer >= 0.1f)
                {
                    changeColorTimer = 0f;
                    meshRenderer.material = normalMat;
                    isDamaged = false;
                }
            }
        }

        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;
            Debug.Log(damage + " 데미지를 입힌다!");
            ChangeMatColor();

            if (currentHealth <= 0)
            {
                currentHealth = 0;       
            }
        }

        private void ChangeMatColor()
        {
            meshRenderer.material = damageMat;
            isDamaged = true;
        }
    }
}
