﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class PlayerQuestInventory : MonoBehaviour
    {
        public static PlayerQuestInventory Instance;

        // 플레이어가 가지고 있는 퀘스트를 관리하는 매니저입니다.
        [SerializeField] private List<Quest> quests;

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
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
    }
}

