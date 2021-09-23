using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class NPCManager : CharacterManager
    {
        public string npcName;
        public Sprite smallIcon;
        public Sprite dialoguePortrait;
        public Quest haveQuest;
        public QuestionMark mark;

        public void Init()
        {
            lockOnTransform = UtilHelper.Find<Transform>(transform, "LockOnTransform").transform;

            mark = GetComponentInChildren<QuestionMark>();
            if (mark != null)
                mark.Init();
        }
        
        //NPC 위에 붙어있는 Mark 오브젝트의 Sprite를 퀘스트의 진행 상태에 따라 바꾸게 만드는 함수입니다.

        public void ChangeQuestionMark(int progress)
        {
            switch (progress)
            {
                //시작이 안된 퀘스트는 노란색 느낌표로
                case 0:
                    mark.spriteRenderer.sprite = mark.exclamMark;
                    mark.spriteRenderer.color = Color.yellow;
                    break;

                //퀘스트 목표 중, NPC와 대화해야하는 
                case 1:
                    mark.spriteRenderer.sprite = mark.questMark;
                    mark.spriteRenderer.color = Color.yellow;
                    break;

                //그 외의 상황에서는 null상태로 바꿉니다.
                default:
                    mark.spriteRenderer.sprite = null;
                    break;
            }
        }
    }
}
