using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class PlayerQuestInventory : MonoBehaviour
    {
        public static PlayerQuestInventory Instance;

        // 플레이어가 가지고 있는 퀘스트를 관리하는 매니저입니다.
        [SerializeField] private List<Quest> quests;

        [Header("Recent Get Info")]
        [SerializeField, ReadOnly] internal EnemyManager recentKilledEnemy;
        [SerializeField, ReadOnly] internal GoalPosition reachGoalPosition;
        [SerializeField, ReadOnly] internal NPCManager recentTalkNpc;

        public delegate void QuestManagerDelegate(PlayerQuestInventory sender);
        public delegate void QuestAddedDelegate(Quest addedQuest);
        public event QuestManagerDelegate OnChanged;
        public event QuestAddedDelegate OnQuestAdd;
        public List<Quest> Quests
        {
            get
            {
                if (quests == null)
                {
                    quests = new List<Quest>();
                }
                return quests;
            }
        }

        // Start is called before the first frame update
        public void Init()
        {
            if (Instance == null)
                Instance = this;

            for (int i = 0; i < quests.Count; i++)
            {
                quests[i].Init();
            }
        }
        private void Update()
        {
            for (int i = 0; i < quests.Count; i++)
            {
                quests[i].currentQuestObjective.UpdateObjective();
            }
        }
        public void AddQuest(Quest quest)
        {
            quest.Init();
            Quests.Add(quest);
            Debug.Log("퀘스트가 등록되었습니다. : " + quest.questName);
        }

        #region SetRecent Variables
        public void SetRecentKilledEnemy(EnemyManager enemyManager)
        {
            recentKilledEnemy = enemyManager;
        }

        public EnemyManager GetRecentKilledEnemy()
        {
            return recentKilledEnemy;
        }

        public void SetReachGoalPosition(GoalPosition goalPosition)
        {
            reachGoalPosition = goalPosition;
        }

        public GoalPosition GetReachGoalPosition()
        {
            return reachGoalPosition;
        }

        internal void DestroyReachGoalObject()
        {
            Destroy(reachGoalPosition.gameObject);
        }

        public void SetRecentTalkNpc(NPCManager npc)
        {
            recentTalkNpc = npc;
        }

        internal NPCManager GetRecentTalkNpc()
        {
            return recentTalkNpc;
        }

        #endregion
    }
}

