using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class EventColliderBossFight : MonoBehaviour
    {
        public bool isActive;
        private void OnTriggerEnter(Collider other) 
        {
            if(other.gameObject.tag.Equals("Player"))
            {
                if(!isActive)
                {
                    isActive = true;
                    WorldEventManager.it.ActiveBossFight();
                }             
            }
        }
    }
}
