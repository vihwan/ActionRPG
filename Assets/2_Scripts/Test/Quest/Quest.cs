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
    [CreateAssetMenu(menuName = "Quest/New Quest")]
    public class Quest : ScriptableObject
    {
        [Header("< Quest Basics >")]
        public string questName;
        [Multiline]
        public string description;

        [Header("< Objective Status >")]
        [SerializeField ,Disable] public QuestObjective currentQuestObjective;
        public List<QuestObjective> objectives = new List<QuestObjective>();

        [Space(10)]
        [Header("< Quest Reward >")]
        public int rewardExp;
        public int rewardGold;
        public List<RewardItem> rewardItemList = new List<RewardItem>();

        public delegate void QuestCompletedDelegate(Quest sender);
        public event QuestCompletedDelegate OnCompleted; //퀘스트가 완료될 때 실행되는 이벤트
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
        }
        public void OnObjectivesCompleted()
        {
            Debug.Log(string.Format("completed quest: {0}", questName));
            currentQuestObjective.state = QuestObjectiveState.Complete;
            if (OnCompleted != null)
            {
                OnCompleted(this);
            }
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
    }
}
