using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    [RequireComponent(typeof(InteractNPC))]
    public class DialogueQuestTrigger : MonoBehaviour
    {
        public List<Dialogue> startQuestDialogue = new List<Dialogue>();
        public List<Dialogue> acceptQuestDialouge = new List<Dialogue>();
        public List<Dialogue> refuseQuestDialouge = new List<Dialogue>();
        public List<Dialogue> endQuestDialogue = new List<Dialogue>();
        public DialogueChoice[] yesNoQuestChoices;

        private InteractNPC interactNPC;
        private NPCManager npcManager;
        DialogueManager dialogueManager;
        private void Start()
        {
            interactNPC = GetComponent<InteractNPC>();
            npcManager = GetComponent<NPCManager>();
            dialogueManager = FindObjectOfType<DialogueManager>();
        }
        internal void TriggerStartEndQuestDialouge(bool isStartQuest)
        {
            SetyesNoQuestDialogueChoicesAction();
            switch (isStartQuest)
            {
                case true:
                    //퀘스트 권유 다이얼로그
                    dialogueManager.SetDialougeList(npcManager,startQuestDialogue, yesNoQuestChoices);
                    break;

                case false:
                    //퀘스트 완료 다이얼로그
                    dialogueManager.SetDialougeList(npcManager,endQuestDialogue, yesNoQuestChoices);
                    break;
            }
        }

        internal void TriggerAcceptRefuseQuestDialogue(bool isAcceptQuest)
        {
            switch (isAcceptQuest)
            {
                case true:
                    //퀘스트 수락 다이얼로그
                    dialogueManager.SetDialougeList(npcManager,acceptQuestDialouge, null);
                    break;

                case false:
                    //퀘스트 거절 다이얼로그
                    dialogueManager.SetDialougeList(npcManager,refuseQuestDialouge, null);
                    break;
            }
        }

        private void SetyesNoQuestDialogueChoicesAction()
        {
            if(yesNoQuestChoices.Length > 0)
            {
                for (int i = 0; i < yesNoQuestChoices.Length; i++)
                {
                    if (yesNoQuestChoices[i].dialogChoiceType.Equals(DialogChoiceType.Yes))
                    {
                        yesNoQuestChoices[i].AddListener(() => interactNPC.AcceptQuest());
                        continue;
                    }
                    else if (yesNoQuestChoices[i].dialogChoiceType.Equals(DialogChoiceType.No))
                    {
                        yesNoQuestChoices[i].AddListener(() => interactNPC.RefuseQuest());
                        continue;
                    }
                }
            }
        }
    }
}
