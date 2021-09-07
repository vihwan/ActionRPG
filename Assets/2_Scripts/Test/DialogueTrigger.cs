﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    [RequireComponent(typeof(InteractNPC))]
    public class DialogueTrigger : MonoBehaviour
    {
        public Dialogue dialogue;
        public DialogueChoice[] dialogueChoices;
        private InteractNPC interactNPC;

        private void Start()
        {
            interactNPC = GetComponent<InteractNPC>();
        }
        public void TriggerDialouge()
        {
            SetDialogueChoicesAction();
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            dialogueManager.StartDialogue(dialogue, dialogueChoices);
        }
        private void SetDialogueChoicesAction()
        {
            if(dialogueChoices.Length != 0)
            {
                for (int i = 0; i < dialogueChoices.Length; i++)
                {
                    if (dialogueChoices[i].dialogChoiceType.Equals(DialogChoiceType.OpenShop))
                    {
                        dialogueChoices[i].AddListener(() => interactNPC.OpenShop());
                        continue;
                    }
                    else if (dialogueChoices[i].dialogChoiceType.Equals(DialogChoiceType.OpenQuest))
                    {

                    }
                    else if (dialogueChoices[i].dialogChoiceType.Equals(DialogChoiceType.OpenEnforce))
                    {
                        dialogueChoices[i].AddListener(() => interactNPC.OpenEnforceItem());
                        continue;
                    }
                    else if (dialogueChoices[i].dialogChoiceType.Equals(DialogChoiceType.EndDialog))
                    {
                        dialogueChoices[i].AddListener(() => interactNPC.EndDialogue());
                        continue;
                    }
                }
            }
        }
    }
}