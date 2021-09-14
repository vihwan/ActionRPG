using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class QuestManager : MonoBehaviour
    {
        //Singleton
        public static QuestManager Instance;

        [Header("All Quests")]
        [SerializeField] private List<Quest> questsList;
        public void Init()
        {
            if (Instance == null)
                Instance = this;

            //리소스 폴더의 모든 퀘스트를 가져옵니다.
            Quest[] quests;

            quests = Resources.LoadAll<Quest>("Scriptable/Quest");
            //저장한 퀘스트들의 진행상태를 전부 아직안함 으로 바꿉니다.
            for (int i = 0; i < quests.Length; i++)
            {
                quests[i].questProgress = QuestProgress.NotStarting;
                questsList.Add(quests[i]);
            }

            NPCManager[] npcs = GameObject.FindObjectsOfType<NPCManager>();
            for (int i = 0; i < npcs.Length; i++)
            {
                if(npcs[i].haveQuest != null)
                {
                    if(npcs[i].haveQuest.questProgress == QuestProgress.NotStarting)
                    {
                        npcs[i].ChangeQuestionMark(0);
                        continue;
                    }
                }
                npcs[i].ChangeQuestionMark(2);
            }
        }
        public Quest GetQuestByName(string questName)
        {
            for (int i = 0; i < questsList.Count; i++)
            {
                if (questsList[i].questName == questName)
                {
                    return questsList[i];
                }
            }
            Debug.Log("퀘스트를 찾지 못했습니다. : " + questName);
            return null;
        }


        public void SetQuestProgress(Quest quest, QuestProgress questProgress)
        {
            int getIndex = questsList.BinarySearch(quest);
            if (getIndex < 0)
            {
                Debug.Log("퀘스트를 찾지 못했습니다. : " + quest.name);
                return;
            }
            else
            {
                questsList[getIndex].questProgress = questProgress;
                return;
            }
        }

        public void FindQuestTargetAndChangeQuestionMark(string objectiveTarget)
        {
            NPCManager[] npcs = GameObject.FindObjectsOfType<NPCManager>();
            for (int i = 0; i < npcs.Length; i++)
            {
                if (npcs[i].npcName == objectiveTarget)
                {
                    npcs[i].ChangeQuestionMark(1);
                    return;
                }
            }
        }

        ////이진 탐색 알고리즘
        //private int BinarySearch(Quest[] questArr, string questName)
        //{
        //    int low = 0;
        //    int high = questArr.Length - 1;
        //    int mid; 

        //    // high가 low보다 작아진다면 찾으려는 데이터가 데이터 집합에 없다.
        //    while (low <= high)
        //    {
        //        // 중앙값은 low와 high를 더한 값을 2로 나누면 된다.
        //        mid = (low + high) / 2;
        //        // 만약 찾으려는 값이 중앙값보다 작다면 high를 mid - 1로 둔다.
        //        if (questArr[mid] > findData)
        //        {
        //            high = mid - 1;
        //        }
        //        // 만약 찾으려는 값이 중앙값보다 크다면 low를 mid + 1로 둔다.
        //        else if (questArr[mid] < findData)
        //        {
        //            low = mid + 1;
        //        }
        //        // 중앙값과 찾으려는 값이 일치하면 mid를 반환한다.
        //        else
        //            return mid;
        //    }
        //    // 데이터를 찾지 못하면 -1를 반환한다.
        //    return -1;
        //}
    }
}

