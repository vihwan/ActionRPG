using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SG
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Quest/New Quest")]
    public class Quest : ScriptableObject
    {
        [Header("Quest Basics")]
        public string questName;
        [Multiline]
        public string description;

        [Header("Objective Status")]
        public QuestObjective currentQuestObjective;
        [HideInInspector]
        public QuestObjective.QuestObjectiveState currentObjectiveState;
        public List<QuestObjective> objectives = new List<QuestObjective>();

        [Header("Quest Reward")]
        public int rewardExp;
        public int rewardGold;
        public List<Item> rewardItemList = new List<Item>();

        public delegate void QuestCompletedDelegate(Quest sender);
        public event QuestCompletedDelegate OnCompleted; //퀘스트가 완료될 때 실행되는 이벤트
        private void Start()
        {
            // Set the quest and the first objective to active
            currentQuestObjective.state = QuestObjective.QuestObjectiveState.active;
            
            if(objectives.Count > 0)
            {
                for (int i = 0; i < objectives.Count; i++)
                {
                    objectives[i].ParentScript = this;
                    Debug.Log("Objectives Found");
                }
            }
        }
        public void OnObjectivesCompleted()
        {
            Debug.Log(string.Format("completed quest: {0}", questName));
            currentObjectiveState = QuestObjective.QuestObjectiveState.complete;
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
            if(newObjective.index > 0)
            {
                objectives[newObjective.index - 1].nextObjective = newObjective;
            }
        }
    }
}
