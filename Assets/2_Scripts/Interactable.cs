using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class Interactable : MonoBehaviour
    {
        public float radius = 0.6f;
        public string interactableText;
        public string shopNameText;
        public bool canInteract;

        private GameObject player;

        public virtual void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

/*        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }*/

        public virtual void Interact(PlayerManager playerManager)
        {
            //Called when Player Interact
            Debug.Log("New Interacted with an Object");
        }
    }
}
