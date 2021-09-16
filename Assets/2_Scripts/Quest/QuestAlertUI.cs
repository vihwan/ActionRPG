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

        public bool isActive = false;

        public void Init()
        {
            questPopup = transform.Find("Quest Popup").gameObject;
            questTitleText = UtilHelper.Find<TMP_Text>(questPopup.transform, "TitleText");
            questObjectiveText = UtilHelper.Find<TMP_Text>(questPopup.transform, "ObjectiveText");
            questProgressText = UtilHelper.Find<TMP_Text>(questPopup.transform, "ProgressText");
        }

        public void OnQuestAlertUI(Quest newQuest)
        {
            if (currentQuest == null)
            {
                currentQuest = newQuest;
                if (questPopup.activeSelf.Equals(true))
                    isActive = false;
                else
                    isActive = true;
            }
            else
            {
                if (currentQuest != newQuest)
                {
                    currentQuest = newQuest;
                    isActive = true;
                }
                else
                {
                    if (questPopup.activeSelf.Equals(true))
                        isActive = false;
                    else
                        isActive = true;
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
    }
}

