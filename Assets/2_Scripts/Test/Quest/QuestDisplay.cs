using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class QuestDisplay : MonoBehaviour
    {
        [Header("Quest")]
        public Quest quest;

        [Header("Elements")]
        public Image icon;
        public TMP_Text titleText;
        public Button displayBtn;

        private Action clickBtnAction;
        // Start is called before the first frame update
        public void Init()
        {
            icon = UtilHelper.Find<Image>(transform, "Icon");
            titleText = UtilHelper.Find<TMP_Text>(transform, "Title");
            displayBtn = GetComponent<Button>();
            displayBtn.onClick.AddListener(OnClickBtn);


            if (this.quest != null)
                SetQuestDisplay(this.quest);
        }
        public void SetQuestDisplay(Quest quest)
        {
            if (this.quest == null)
            {
                this.quest = quest;
                titleText.text = quest.questName;
            }
        }
        public void AddClickBtnAction(Action action)
        {
            clickBtnAction = action;
        }
        private void OnClickBtn()
        {
            clickBtnAction?.Invoke();
        }

    }
}
