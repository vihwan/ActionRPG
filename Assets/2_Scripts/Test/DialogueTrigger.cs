using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    [RequireComponent(typeof(InteractNPC))]
    public class DialogueTrigger : MonoBehaviour
    {
        public List<Dialogue> dialogueList = new List<Dialogue>();
        public DialogueChoice[] dialogueChoices;
        private InteractNPC interactNPC;
        private NPCManager nPCManager;

        private void Start()
        {
            interactNPC = GetComponent<InteractNPC>();
            nPCManager = GetComponent<NPCManager>();
        }
        public void TriggerDialouge()
        {
            SetDialogueChoicesAction();
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            dialogueManager.SetDialougeList(nPCManager,dialogueList, dialogueChoices);
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
                        dialogueChoices[i].AddListener(() => interactNPC.OpenQuest());
                        continue;
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
                    //디버그용 함수
                    else if (dialogueChoices[i].dialogChoiceType.Equals(DialogChoiceType.GetMoney))
                    {
                        dialogueChoices[i].AddListener(() => PlayerInventory.Instance.GetGold(500000));
                        continue;
                    }
                }
            }
        }
    }
}
