using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator anim;
        public bool canRotate;

        public virtual void PlayTargetAnimation(string targetAnim, bool isInteracting, float duration = 0.2f, bool canRotate = false)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("canRotate", canRotate);
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, duration);
        }

        public virtual void PlayTargetAnimationRootRotation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isRotatingWithRootMotion", true);
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
        }
    }
}
