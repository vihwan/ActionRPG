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
        [ReadOnly] public int index;
        public string title;
        public QuestObjectiveType kind;
        [Tooltip("이름으로 목표 달성 여부를 비교하기 때문에, 정확히 기입하셔야 합니다.")]
        public string objectiveTarget;
        public QuestObjectiveState state;
        public QuestObjective nextObjective;

        [Header("Progress")]
        [SerializeField , ReadOnly] private float progress = 0f; //목표 진행도
        public int currentProgressCount;
        public int maxProgressCount;

        public Quest ParentScript { get; set; }
        public float Progress
        {
            get => progress;
            private set
            {
                progress = value;
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
        //진행도를 Update문으로 검사 할 수 없다..
        //Monobehavior가 아니라서..
        public void UpdateObjective()
        {
            //목표 진행중일 경우, 진행도를 계산한다.
            if (state == QuestObjectiveState.Active)
            {
                if (kind.Equals(QuestObjectiveType.Destroy))
                {
                    if(PlayerQuestInventory.Instance.recentKilledEnemy != null)
                    {
                        if (PlayerQuestInventory.Instance.GetRecentKilledEnemy().enemyName == objectiveTarget)
                        {
                            currentProgressCount++;
                            PlayerQuestInventory.Instance.recentKilledEnemy = null;
                            Debug.Log("카운트 증가!");
                        }
                    }
                }

                Progress = (float) currentProgressCount / maxProgressCount;
                //진행도가 1에 근접하고 완료 상태가 아니면 완료 상태로 전환되고 OnCompleted 이벤트가 실행된다.
                if (Mathf.Approximately(1f, Progress) && state != QuestObjectiveState.Complete)
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
                ParentScript.currentQuestObjective = nextObjective;
                ParentScript.currentQuestObjective.state = QuestObjectiveState.Active;
                ParentScript.currentQuestObjective.Initialize();
            }
            else
            {
                // All objectives complete, end quest
                ParentScript.OnObjectivesCompleted();
            }
            Debug.Log(string.Format("completed objective: {0}", title));
        }

        //internal void SetObjectTargetType<T>()
        //{
        //    var t = typeof(T);

        //    if(t.GetType().Equals(typeof(Item)))
        //    {
        //        objectiveTarget = (Item)objectiveTarget;
        //    }          
        //}
    }
}
