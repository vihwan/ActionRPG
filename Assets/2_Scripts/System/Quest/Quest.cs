using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SG
{
    [System.Serializable]
    public class RewardItem
    {
        public Item rewardItem;
        public int itemCount;
    }


    [System.Serializable]
    public enum QuestProgress
    {
        NotStarting,  //시작안함
        Proceeding,   //진행중
        Completed     //완료됨
    }

    [System.Serializable]
    [CreateAssetMenu(menuName = "Quest/New Quest")]
    public class Quest : ScriptableObject
    {
        [Header("< Quest Basics >")]
        public string questName;
        public string subQuestName;
        
        [Multiline]
        public string description;
        [SerializeField, Tooltip("해당 퀘스트를 의뢰한 NPC. 나중에 퀘스트 완료의 대화 내용을 출력하기 위해, 미리 참조해둡니다.")]
        public NPCData NPC_Requester;
        [Tooltip("해당 퀘스트의 클리어 여부를 지정하는 변수입니다.")]
        public QuestProgress questProgress;


        [Header("< Objective Status >")]
        [SerializeField] public QuestObjective currentQuestObjective;
        public List<QuestObjective> objectives = new List<QuestObjective>();

        [Space(10)]
        [Header("< Quest Reward >")]
        public int rewardExp;
        public int rewardGold;
        public List<RewardItem> rewardItemList = new List<RewardItem>();

        //퀘스트 완료 시 수행되는 이벤트
        public Action OnCompleted;

        public void Init()
        {
            // Set the quest and the first objective to active
            if (objectives.Count > 0)
            {
                for (int i = 0; i < objectives.Count; i++)
                {
                    objectives[i].ParentScript = this;
                    objectives[i].state = QuestObjectiveState.Inactive;
                    objectives[i].currentProgressCount = 0;
                }

                Debug.Log("Objectives Found");

                for (int j = 1; j < objectives.Count; j++)
                {
                    objectives[j - 1].nextObjective = objectives[j];
                    Debug.Log(objectives[j - 1].title + "의 nextObjective : " + objectives[j - 1].nextObjective.title);
                }
            }
            currentQuestObjective = objectives[0];
            currentQuestObjective.state = QuestObjectiveState.Active;

            OnCompleted += GetQuestReward;
            OnCompleted += DeleteThisQuest;
        }

        public void SetNPCRequester(NPCData npc)
        {
            NPC_Requester = npc;
        }
        public void OnObjectivesCompleted()
        {
            Debug.Log(string.Format("completed quest: {0}", questName));
            //퀘스트가 클리어되면 클리어 상태로 바꿉니다.
            questProgress = QuestProgress.Completed;
        }
        
        public void AddObjective()
        {
            QuestObjective newObjective = new QuestObjective();
            objectives.Add(newObjective);
            newObjective.index = objectives.Count - 1;

            //동작하지 않고 있음. PlayerQuestInventory에서 재지정
            if (newObjective.index > 0)
            {

                objectives[newObjective.index - 1].nextObjective = newObjective;
            }
        }

        //퀘스트 보상을 획득하는 함수. OnCompleted Action에 등록

        private void GetQuestReward()
        {
            //골드 회득
            PlayerInventory.Instance.GetGold(rewardGold);
            //경험치 획득
            LevelManager.it.AddExperience(rewardExp);
            //경험치 획득 팝업 생성
            PopUpGenerator.Instance.GetMessageGetExp(rewardExp);
            
            //보상 아이템 획득
            for (int i = 0; i < rewardItemList.Count; i++)
            {
                PlayerInventory.Instance.SaveGetItemToInventory(rewardItemList[i].rewardItem, rewardItemList[i].itemCount);
            }
        }

        private void DeleteThisQuest()
        {
            PlayerQuestInventory.Instance.DeleteQuest(this);
        }
    }
}
