using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class InteractNPC : Interactable
    {
        [Header("Shop Item List")]
        public List<Item> itemLists;
        [SerializeField] private TextMesh textMesh;
        [SerializeField] public QuestionMark mark;

        private NPCData npcData;
        private DialogueTrigger dialogueTrigger;
        private InputHandler inputHandler;

        public void Init()
        {
            npcData = GetComponent<NPCData>();
            if(npcData != null)
            {
                interactIcon = npcData.smallIcon;
                interactName = npcData.npcName;
            }
            dialogueTrigger = GetComponent<DialogueTrigger>();
            textMesh = GetComponentInChildren<TextMesh>(true);
            if (textMesh != null)
            {
                textMesh.text = "<NPC> "+ npcData.npcName;
            }

            mark = GetComponentInChildren<QuestionMark>();
            if (mark != null)
                mark.Init();
        }

        private void Update()
        {
            if(textMesh != null)
            {
                textMesh.transform.LookAt(Camera.main.transform.position);
                textMesh.transform.Rotate(0f, 180f, 0f);
            }

            if(mark != null)
            {
                mark.transform.LookAt(Camera.main.transform.position);
                mark.transform.Rotate(0f,180f,0f);
            }
        }
        public override void Interact(PlayerManager playerManager)
        {
            //NPC와 대화 및 메뉴 선택
            TalkNPC(playerManager);
        }
        public void TalkNPC(PlayerManager playerManager, bool isQuestTrigger = false)
        {
            inputHandler = playerManager.GetComponent<InputHandler>();

            //NPC랑 대화하는 동안은 플레이어의 움직임을 멈춰야함.
            inputHandler.StopMovement();
            Debug.Log("NPC와 대화하기");

            if (isQuestTrigger.Equals(true))
            {
                GetComponent<DialogueQuestTrigger>().TriggerStartEndQuestDialouge(isStartQuest: false);
            }
            else
            {
                GetComponent<DialogueTrigger>().TriggerDialouge();
            }

            //최근에 대화한 NPC 등록
            playerManager.GetComponent<PlayerQuestInventory>().SetRecentTalkNpc(npcData);
            //Interact Object UI SetActive False
            playerManager.InteractableUI.SetActiveInteractUI(false);
        }
        internal void OpenShop()
        {
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            dialogueManager.EndDialogue();

            //NPC가 가지고 있는 아이템을 보여주는 상점을 활성화시킴.

            GUIManager.it.shopPanel.SetActiveShopPanel(true);
            GUIManager.it.shopPanel.SetShopPanel(shopNameText);
            GUIManager.it.shopPanel.CreateItemList(itemLists);
            inputHandler.menuFlag = !inputHandler.menuFlag;
        }

        internal void OpenQuest()
        {
            //퀘스트 선택지 클릭 시
            DialogueQuestTrigger dialogueQuestTrigger = GetComponent<DialogueQuestTrigger>();
            dialogueQuestTrigger.TriggerStartEndQuestDialouge(isStartQuest: true);
        }

        internal void AcceptQuest()
        {
            //퀘스트 수락 시 출력되야하는 대화 출력
            DialogueQuestTrigger dialogueQuestTrigger = GetComponent<DialogueQuestTrigger>();
            dialogueQuestTrigger.TriggerAcceptRefuseQuestDialogue(isAcceptQuest: true);

            //NPC가 가지고 있는 Quest 정보의 상태를 변경
            QuestManager.it.SetQuestProgress(npcData.haveQuest, QuestProgress.Proceeding);
            //해당 NPC의 QuesetionMark를 없는 상태로 변경
            npcData.ChangeQuestionMark(2);

            //NPC가 가지고 있는 Quest를 PlayerQuestInventory에 등록하고 초기화
            inputHandler.GetComponent<PlayerQuestInventory>().AddQuest(npcData.haveQuest, npcData);
        }

        internal void RefuseQuest()
        {
            //퀘스트 거절 시 출력되야하는 대화 출력
            DialogueQuestTrigger dialogueQuestTrigger = GetComponent<DialogueQuestTrigger>();
            dialogueQuestTrigger.TriggerAcceptRefuseQuestDialogue(isAcceptQuest: false);
        }

        internal void OpenEnforceItem()
        {
            //강화하기 메뉴 열기
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            dialogueManager.EndDialogue();

            //강화하기 UI를 활성화시킴

            GUIManager.it.windowPanel.OpenEnforceWindowPanel();
            inputHandler.menuFlag = !inputHandler.menuFlag;
        }

        internal void EndDialogue()
        {
            //대화종료 선택 시
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            dialogueManager.EndDialogue();
        }

        
    }
}
