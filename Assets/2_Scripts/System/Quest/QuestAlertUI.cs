using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

namespace SG
{
    public class QuestAlertUI : MonoBehaviour
    {
        public Quest currentQuest;
        public GameObject questPopup;
        public TMP_Text questTitleText;
        public TMP_Text questObjectiveText;
        public TMP_Text questProgressText;
        [SerializeField] private bool isActive = false;

        [Header("Quest Objective Alert")]
        public TMP_Text questObjectiveAlertText;


        private Animator animator;

        public void Init()
        {
            questPopup = transform.Find("Quest Popup").gameObject;
            questTitleText = UtilHelper.Find<TMP_Text>(questPopup.transform, "TitleText");
            questObjectiveText = UtilHelper.Find<TMP_Text>(questPopup.transform, "ObjectiveText");
            questProgressText = UtilHelper.Find<TMP_Text>(questPopup.transform, "ProgressText");
            animator = GetComponent<Animator>();
        }

        public void OnQuestAlertUI(Quest newQuest)
        {
            if (currentQuest == null)
            {
                currentQuest = newQuest;
                isActive = true;
                Debug.Log("퀘스트 UI 신규 등록");
            }
            else
            {
                if (currentQuest.questName != newQuest.questName)
                {
                    currentQuest = newQuest;
                    isActive = true;
                    Debug.Log("퀘스트 UI 신규 등록");
                }
                else
                {
                    isActive = !isActive;
                    Debug.Log("기존 퀘스트 UI : " + isActive);
                }
            }

            SetActiveQuestAlertUI(isActive);
            SetText(currentQuest);
        }

        private void SetText(Quest quest)
        {
            questTitleText.text = quest.questName;
            questObjectiveText.text = quest.currentQuestObjective.title;
            questProgressText.text =
                    string.Format("( {0} / {1} )", quest.currentQuestObjective.currentProgressCount
                                                 , quest.currentQuestObjective.maxProgressCount);
        }

        public void UpdateCurrentObjectiveText()
        {   
            if(currentQuest.questProgress == QuestProgress.Completed)
            {
                SetActiveQuestAlertUI(false);
                return;
            }     

            questObjectiveText.text = currentQuest.currentQuestObjective.title;
            questProgressText.text =
                    string.Format("( {0} / {1} )", currentQuest.currentQuestObjective.currentProgressCount
                                                 , currentQuest.currentQuestObjective.maxProgressCount);     
        }

        public void SetActiveQuestAlertUI(bool state)
        {
            questPopup.SetActive(state);
        }

        public void OnActiveQuestObjectiveAlertText(QuestObjective nextObjective)
        {
           // questObjectiveAlertText
           questObjectiveAlertText.text = string.Format("목표 달성\n다음 목표 활성 : {0}", nextObjective.title);
           animator.SetTrigger("Play");
        }
    }
}

