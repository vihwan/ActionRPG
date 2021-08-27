using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class AnimatorHandler : MonoBehaviour
    {
        PlayerManager playerManager;
        [SerializeField] private Animator anim;
        [SerializeField] private PlayerLocomotion playerLocomotion;
        private int vertical;
        private int horizontal;
        public bool canRotate;
        public Animator Anim { get => anim; private set => anim = value; }

        private InputHandler inputHandler;

        public void Initalize()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            Anim = GetComponent<Animator>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");

            inputHandler = GetComponentInParent<InputHandler>();
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

            Anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            Anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting, float duration = 0.2f)
        {
            Anim.applyRootMotion = isInteracting;
            Anim.SetBool("isInteracting", isInteracting);
            Anim.CrossFade(targetAnim, duration);
            //StartCoroutine(CheckAnimationUnEquip_NotMove());

            /*08월 06일, Override 행동 이후 자연스러운 애니메이션 변화를 위해서 추가**/
            if(targetAnim == "Empty")
            {
                Anim.CrossFade("Locomotion", 0.3f);
            }
        }
/*        IEnumerator CheckAnimationUnEquip_NotMove()
        {
            while(!Anim.GetCurrentAnimatorStateInfo(0).IsName("WeaponChange_UnEquip_NotMove"))
            {
                if(inputHandler.MoveAmount > 0)
                {
                    Debug.Log("동작동작");
                    Anim.Play("Locomotion");
                    break;
                }
                yield return null;
            }
        }*/

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }

        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false)
                return;

            float delta = Time.deltaTime;
            playerLocomotion.Rigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.Rigidbody.velocity = velocity;
        }

        public void EnableCombo()
        {
            anim.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            anim.SetBool("canDoCombo", false);
        }
    }
}

