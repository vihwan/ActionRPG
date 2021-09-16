using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class AchieveListDisplay : MonoBehaviour
    {
        public TMP_Text titleText;
        public TMP_Text progressText;
        public Image icon;
        public Button displayBtn;
        public Action btnAction;
    
        public void Init()
        {
            titleText = UtilHelper.Find<TMP_Text>(transform, "Title");
            progressText = UtilHelper.Find<TMP_Text>(transform, "Progress");
            icon = UtilHelper.Find<Image>(transform, "Icon");
            displayBtn = GetComponent<Button>();
            displayBtn.onClick.AddListener(OnClickBtn);

        }

        public void SetActiveListDisplay(Achieve achieve)
        {
            titleText.text = achieve.achieveName;
            progressText.text = string.Format("{0}%", Mathf.RoundToInt(achieve.progress * 100));
            icon.sprite = achieve.achieveIcon;

        }

        public void AddDisplayBtnLister(Action action) 
        {
            btnAction = action;
        }

        public void OnClickBtn()
        {
            btnAction?.Invoke();
        }
    }
}
