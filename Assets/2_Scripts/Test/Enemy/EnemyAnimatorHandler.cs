using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class EnemyAnimatorHandler : AnimatorManager
    {
        [SerializeField] private EnemyManager enemyManager;
        public void Init()
        {
            anim = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
        }

        //public override void PlayTargetAnimation(string targetAnim, bool isInteracting, float duration = 0.2F)
        //{
        //    base.PlayTargetAnimation(targetAnim, isInteracting, duration);
        //}

        private void OnAnimatorMove() 
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidbody.velocity = velocity;
        }
    }
}
