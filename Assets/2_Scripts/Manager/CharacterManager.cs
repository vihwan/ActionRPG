using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class CharacterManager : MonoBehaviourSingleton<CharacterManager>
    {
        [SerializeField] internal Transform lockOnTransform;

        public virtual void Init()
        {

        }
    }
}

