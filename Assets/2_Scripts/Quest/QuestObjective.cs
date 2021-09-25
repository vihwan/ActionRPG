using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    //퀘스트를 달성하기 위해 수행해야하는 내용을 담고 있는 클래스
    //기본적으로 대화하기, 파괴하기(사냥), 수집하기, 특정 장소로 향하기 등이 있다.
    //또한 각 목적들이 수행되었는지를 확인하는 변수도 필요.
    public enum QuestObjectiveState
    {
        Inactive, // 진행중이 아닌 상태
        Active,   // 진행중
        Complete  // 달성
    }
    public enum QuestObjectiveType
    {
        Travel,  // 특정 지점 도달
        Destroy, // 파괴, 몬스터 사냥 등
        Talk,    // NPC와 대화
        Collect  // 아이템 수집
    }

    [System.Serializable]
    public class QuestObjective
    {
        public int index;
        public string title;
        public QuestObjectiveType type;
        [Tooltip("string으로 비교하는 것이기에 정확히 입력해야합니다.")]
        public string objectiveTarget;
        public Item objectiveItem;
        public QuestObjectiveState state;
        public QuestObjective nextObjective;

        [Header("Progress")]
        [SerializeField] private float progress = 0f; //목표 진행도
        public int currentProgressCount;
        public int maxProgressCount;

        public Quest ParentScript { get; set; }
        public float Progress
        {
            get => progress;
            private set
            {
                progress = value;
                if (progress >= 1f)
                    progress = 1f;
            }
        }

        public delegate void ObjectiveDelegate(QuestObjective sender);
        public event ObjectiveDelegate OnCompleted; //목표가 달성될 때 실행되는 이벤트
        public event ObjectiveDelegate OnStarted; //목표가 시작될 때 실행되는 이벤트

        public void Initialize()
        {
            InvokeOnStartedEvent();

            // Spawn anything set for delayed spawn
        }
        public void InvokeOnStartedEvent()
        {
            if (OnStarted != null)
            {
                OnStarted(this);
            }
        }

        //Objective의 타입에 따라 진행 방식을 다르게 설정
        //PlayerQuestInventory에서 해당 함수를 Update문으로 돌려 검사합니다.

        public void UpdateObjective()
        {
            //목표 진행중일 경우, 진행도를 계산한다.
            if (state.Equals(QuestObjectiveState.Active))
            {
                if (type.Equals(QuestObjectiveType.Destroy))
                {
                    if (PlayerQuestInventory.Instance.recentKilledEnemy != null)
                    {
                        if (PlayerQuestInventory.Instance.GetRecentKilledEnemy().enemyName.Equals(objectiveTarget))
                        {
                            currentProgressCount++;
                            PlayerQuestInventory.Instance.recentKilledEnemy = null;
                            Debug.Log("카운트 증가!");
                        }
                    }
                }
                else if (type.Equals(QuestObjectiveType.Travel))
                {
                    if (PlayerQuestInventory.Instance.reachGoalPosition != null)
                    {
                        if (PlayerQuestInventory.Instance.GetReachGoalPosition().goalName.Equals(objectiveTarget))
                        {
                            currentProgressCount++;
                            Debug.Log("목표지점 도달. 카운트 증가!");
                            PlayerQuestInventory.Instance.DestroyReachGoalObject();
                            PlayerQuestInventory.Instance.reachGoalPosition = null;
                        }
                    }
                }
                else if (type.Equals(QuestObjectiveType.Collect))
                {
                    //특정 아이템을 지정하고, 아이템 타입에 따라 인벤토리를 탐색하여 해당 아이템의 수량을 가져온다.
                    switch (objectiveItem.itemType)
                    {
                        case ItemType.Consumable:
                            currentProgressCount = PlayerInventory.Instance.GetHaveItem(objectiveItem as ConsumableItem);
                            break;
                        case ItemType.Ingredient:
                            currentProgressCount = PlayerInventory.Instance.GetHaveItem(objectiveItem as IngredientItem);
                            break;
                        default:
                            break;
                    }
                }
                else if (type.Equals(QuestObjectiveType.Talk))
                {
                    //최근에 대화한 상대가 퀘스트에서 대화해야하는 상대와 일치하면 카운트를 늘린다.
                    if (PlayerQuestInventory.Instance.recentTalkNpc != null)
                    {
                        //최근에 대화한 대상이 타겟과 같다면
                        if (PlayerQuestInventory.Instance.GetRecentTalkNpc().npcName.Equals(objectiveTarget))
                        {
                            //최근에 대화한 NPC의 QuestionMark를 표시가 안되도록 변경시킨다.
                            PlayerQuestInventory.Instance.GetRecentTalkNpc().ChangeQuestionMark(2);
                            //퀘스트를 주었던 대상의 EndQuestDialogue 대화의 트리거를 실행시킨다.
                            ParentScript.NPC_Requester.GetComponent<InteractNPC>().TalkNPC(PlayerManager.Instance, true);


                            //퀘스트 진행도를 하나 올린다.
                            currentProgressCount++;
                            Debug.Log("NPC와 대화. 카운트 증가!");
                            //최근에 대화한 NPC를 Null로 초기화시킨다.
                            PlayerQuestInventory.Instance.SetRecentTalkNpc(null);
                        }
                    }
                }

                Progress = (float)currentProgressCount / maxProgressCount;
                //진행도가 1에 근접하고 완료 상태가 아니면 완료 상태로 전환되고 OnCompleted 이벤트가 실행된다.
                if ((Mathf.Approximately(1f, Progress) || Progress >= 1) && state != QuestObjectiveState.Complete)
                {
                    state = QuestObjectiveState.Complete;
                    Debug.Log("목표 달성");
                    OnCompletedObjective();
                }
            }
        }
        private void OnCompletedObjective()
        {
            if (OnCompleted != null)
            {
                OnCompleted(this);
            }
            if (nextObjective != null)
            {
                GUIManager.instance.questAlertUI.OnActiveQuestObjectiveAlertText(nextObjective);
                ParentScript.currentQuestObjective = nextObjective;
                ParentScript.currentQuestObjective.state = QuestObjectiveState.Active;
                ParentScript.currentQuestObjective.Initialize();
                if(ParentScript.currentQuestObjective.type.Equals(QuestObjectiveType.Talk))
                {
                    PlayerQuestInventory.Instance.SetRecentTalkNpc(null);
                    QuestManager.Instance.FindQuestTargetAndChangeQuestionMark(ParentScript.currentQuestObjective.objectiveTarget);
                }
            }
            else
            {
                // All objectives complete, end quest\
                ParentScript.currentQuestObjective.state = QuestObjectiveState.Complete;
                //퀘스트 클리어 이벤트를 실행
                ParentScript.OnObjectivesCompleted();
            }
            Debug.Log(string.Format("completed objective: {0}", title));
        }  
    }
}
