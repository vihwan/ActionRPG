using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class AchieveMissionTitle : MonoBehaviour
    {
        public Image rewardIcon;
        public Slider progressSlider;
        public Button rewardBtn;
        public TMP_Text rewardBtnText;
        public TMP_Text progressText;

        public Action btnAction;

        public void SetMissionTitle(Achieve achieve)
        {
            rewardIcon.sprite = achieve.rewardItem.rewardItem.itemIcon;
            
            int progress = Mathf.RoundToInt(achieve.progress * 100);
            progressText.text = string.Format("{0} %", progress);
            SetSlider(progress);
            if(progress >= 100 && achieve.achieveState == AchieveState.Completed)
            {
                rewardBtnText.text = "보상 받기";
                rewardBtn.interactable = true;
            }
            else if(progress >= 100 && achieve.achieveState == AchieveState.GetReward)
            {
                rewardBtnText.text = "완료";
                rewardBtn.interactable = false;
            }
            else 
            {
                rewardBtnText.text = "진행중";
                rewardBtn.interactable = false;
            }
        }

        private void SetSlider(int value)
        {
            progressSlider.value = value;
        }

        public void GetRewardItem()
        {
            rewardBtnText.text = "완료";
            rewardBtn.interactable = false;
            btnAction?.Invoke();
        }
    }
}
