using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerAnimatorHandler : AnimatorManager
    {
        PlayerManager playerManager;
        [SerializeField] private PlayerLocomotion playerLocomotion;
        private int vertical;
        private int horizontal;

        public void Initalize()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            anim = GetComponent<Animator>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1f;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1f;
            }
            else
                v = 0;
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1f;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1f;
            }
            else
                h = 0;
            #endregion

            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }

            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public override void PlayTargetAnimation(string targetAnim, bool isInteracting, float duration = 0.2F)
        {
            base.PlayTargetAnimation(targetAnim, isInteracting, duration);

            /*08월 06일, Override 행동 이후 자연스러운 애니메이션 변화를 위해서 추가**/
            if (targetAnim == "Empty")
            {
                anim.CrossFade("Locomotion", 0.3f);
            }
        }

        public void CanRotate()
        {
            anim.SetBool("canRotate", true);
        }

        public void StopRotate()
        {
            anim.SetBool("canRotate", false);
        }

        public void EnableInvulnerable()
        {
            anim.SetBool("isInvulnerable", true);
        }

        public void DisableInvulnerable()
        {
            anim.SetBool("isInvulnerable", false);
        }

        public void EnableCombo()
        {
            anim.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            anim.SetBool("canDoCombo", false);
        }

        public void EnableCheckCounterAttack()
        {
            anim.SetBool("canCounter", true);
        }

        public void DisableCheckCounterAttack()
        {
            anim.SetBool("canCounter", false);
        }

        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false)
                return;

            float delta = Time.deltaTime;
            playerLocomotion.rigidBody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.rigidBody.velocity = velocity;
        }
    }
}

