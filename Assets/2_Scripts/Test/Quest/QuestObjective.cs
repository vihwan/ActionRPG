using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    //퀘스트를 달성하기 위해 수행해야하는 내용을 담고 있는 클래스
    //기본적으로 대화하기, 파괴하기(사냥), 수집하기, 특정 장소로 향하기 등이 있다.
    //또한 각 목적들이 수행되었는지를 확인하는 변수도 필요.
    [System.Serializable]
    public class QuestObjective
    {
        [ReadOnly] public int index;
        public string title;
        public QuestObjectiveType kind;
        public QuestObjectiveState state;
        public float progress = 0f; //목표 진행도
        public QuestObjective nextObjective;
        public Quest ParentScript { get; set; }
        public enum QuestObjectiveState
        {
            inactive,
            active,
            complete
        }
        public enum QuestObjectiveType
        {
            travel,  // 특정 지점 도달
            destroy, // 파괴, 몬스터 사냥 등
            talk,    // NPC와 대화
            collect  // 아이템 수집
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
        //ㅅㅂ..
        private void Update()
        {
            // Only allow checking for current objective. If a later objective is completed before it is available to the player, 
            // it will be set to complete as soon as it is available since all of its targets are completed

            //목표 진행중일 경우, 진행도를 계산한다.
            if (state == QuestObjectiveState.active)
            {
/*                if (targets != null && targets.Length > 0)
                {
                    switch (kind)
                    {
                        case QuestObjectiveType.destroy:
                            progress = 0f;
                            foreach (var target in targets)
                            {
                                if (target == null || target.gameObject == null)
                                {
                                    progress += 1f / targets.Length;
                                }
                            }
                            break;
                        case QuestObjectiveType.travel:
                            progress = 0f;

                            foreach (var target in targets)
                            {
                                if (target.state == QuestObjectiveState.complete)
                                {
                                    progress += 1f / targets.Length;
                                }
                            }
                            break;
                        case QuestObjectiveType.talk:
                            break;
                        case QuestObjectiveType.collect:
                            break;
                    }
                }*/

                //진행도가 1에 근접하고 완료 상태가 아니면 완료 상태로 전환되고 OnCompleted 이벤트가 실행된다.
                if (Mathf.Approximately(1f, progress) && state != QuestObjectiveState.complete)
                {
                    state = QuestObjectiveState.complete;
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
                ParentScript.currentQuestObjective.state = QuestObjectiveState.active;
                ParentScript.currentQuestObjective.Initialize();
            }
            else
            {
                // All objectives complete, end quest
                ParentScript.OnObjectivesCompleted();
            }
            Debug.Log(string.Format("completed objective: {0}", title));
        }
    }
}
