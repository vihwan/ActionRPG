using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SG
{
    public class Interactable : MonoBehaviour
    {
        [Header("Attribute")]
        public string interactName;
        public Sprite interactIcon;

        public string interactableText;
        public string shopNameText;
        public bool canInteract;

        private GameObject player;
        public virtual void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        public virtual void Interact(PlayerManager playerManager)
        {
            //Called when Player Interact
            Debug.Log("New Interacted with an Object");
        }
    }
}
