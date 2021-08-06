using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimationBool : StateMachineBehaviour
{
    [System.Serializable]
    public struct BoolStatus
    {
        public string targetBool;
        public bool status;
    }

    public BoolStatus[] boolStatuses;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        foreach (BoolStatus b in boolStatuses)
        {
            animator.SetBool(b.targetBool, b.status);
        }
    }
}
