using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class InteractNPC : Interactable
    {
        [Header("Shop Item List")]
        public List<Item> itemLists;

        private TextMesh textMesh;
        private DialogueTrigger dialogueTrigger;
        private InputHandler inputHandler;

        public override void Start()
        {
            dialogueTrigger = GetComponent<DialogueTrigger>();

            textMesh = GetComponentInChildren<TextMesh>(true);
            if (textMesh != null)
            {
                textMesh.text = "<NPC> "+ dialogueTrigger.dialogue.characterName;
            }
        }

        private void Update()
        {
            if(textMesh != null)
            {
                textMesh.transform.LookAt(Camera.main.transform.position);
                textMesh.transform.Rotate(0f, 180f, 0f);
            }
        }
        public override void Interact(PlayerManager playerManager)
        {
            //NPC와 대화 및 메뉴 선택
            TalkNPC(playerManager);
        }
        public void TalkNPC(PlayerManager playerManager)
        {
            inputHandler = playerManager.GetComponent<InputHandler>();

            //NPC랑 대화하는 동안은 플레이어의 움직임을 멈춰야함.
            inputHandler.StopMovement();
            Debug.Log("NPC와 대화하기");

            GetComponent<DialogueTrigger>().TriggerDialouge();

            //Interact Object UI SetActive False
            playerManager.InteractableUI.SetActiveInteractUI(false);
        }
        internal void OpenShop()
        {
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            dialogueManager.EndDialogue();

            //NPC가 가지고 있는 아이템을 보여주는 상점을 활성화시킴.

            GUIManager.instance.shopPanel.SetActiveShopPanel(true);
            GUIManager.instance.shopPanel.SetShopPanel(shopNameText);
            GUIManager.instance.shopPanel.CreateItemList(itemLists);
            inputHandler.menuFlag = !inputHandler.menuFlag;
        }

        internal void EndDialogue()
        {
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            dialogueManager.EndDialogue();
        }
    }
}
