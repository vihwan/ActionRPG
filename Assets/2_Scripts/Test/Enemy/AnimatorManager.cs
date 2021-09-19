using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator anim;

        public virtual void PlayTargetAnimation(string targetAnim, bool isInteracting, float duration = 0.2f)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, duration);
        }
    }
}
