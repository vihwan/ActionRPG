using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SG
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        public CapsuleCollider characterCollider;
        public CapsuleCollider characterBlockerCollider;

        public void Init()
        {
            Physics.IgnoreCollision(characterCollider, characterBlockerCollider, true);
        }

        public void EnableIsTriggerAllCollider()
        {
            characterCollider.isTrigger = false;
            characterBlockerCollider.isTrigger = true;
        }

    }
}
