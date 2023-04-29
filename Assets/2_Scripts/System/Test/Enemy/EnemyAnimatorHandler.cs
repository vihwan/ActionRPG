using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class EnemyAnimatorHandler : AnimatorManager
    {
        [SerializeField] private EnemyManager enemyManager;
        [SerializeField] private EnemyBossManager enemyBossManager;
        [SerializeField] private EnemyWeaponSlotManager enemyWeaponSlotManager;
        [SerializeField] private BossFXTransform bossFXTransform;
        public void Init()
        {
            anim = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
            if (enemyManager.isBoss)
            {
                enemyBossManager = enemyManager.GetComponent<EnemyBossManager>();
                bossFXTransform = enemyManager.GetComponentInChildren<BossFXTransform>();
            } 
            enemyWeaponSlotManager = GetComponent<EnemyWeaponSlotManager>();
        }

        public override void PlayTargetAnimation(string targetAnim, bool isInteracting, float duration = 0.2F, bool canRotate = false)
        {
            if(enemyWeaponSlotManager != null)
                enemyWeaponSlotManager.CloseDamageCollider();

            base.PlayTargetAnimation(targetAnim, isInteracting, duration, canRotate);
        }

        public void CanRotate()
        {
            anim.SetBool("canRotate", true);
        }

        public void StopRotate()
        {
            anim.SetBool("canRotate", false);
        }

        public void EnableCombo()
        {
            anim.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            anim.SetBool("canDoCombo", false);
        }

        public void IsInteractingFalse()
        {
            anim.SetBool("isInteracting", false);
        }

        public void InstantiateBossParticleFX(string fxName)
        {
            if (bossFXTransform != null)
                bossFXTransform.InstantiateParticleFX(fxName);
        }

        public void DestroyBossParticleFX(string fxName)
        {
            if (bossFXTransform != null)
                bossFXTransform.DestroyParticleFX(fxName);
        }

        private void OnAnimatorMove() 
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidbody.velocity = velocity;

            if(enemyManager.isRotatingWithRootMotion)
            {
                enemyManager.transform.rotation *= anim.deltaRotation;
            }
        }
    }
}
