using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public enum AchieveState
    {
        Proceeding,
        Completed,
        GetReward
    }


    [CreateAssetMenu(menuName = "Achieve/New Achieve")]
    public class Achieve : ScriptableObject
    {
        [Header("< Achieve Basics >")]
        public string achieveName;
        public Sprite achieveIcon;
        [ReadOnly] public float progress;
        [ReadOnly] public AchieveState achieveState;

        [Header("< Missions >"), Space(10)]
        public List<AchieveMission> achieveMissions = new List<AchieveMission>();

        [Header("< Reward >")]
        public RewardItem rewardItem;

        public void Init()
        {
            SetProgress();
        }

        private void SetProgress()
        {
            int count = 0;
            for (int i = 0; i < achieveMissions.Count; i++)
            {
                if (achieveMissions[i].achieveMissionState.Equals(AchieveMissionState.Completed))
                    count++;
            }
            Debug.Log("Count : " + count);

            progress =  (float)count / (float)achieveMissions.Count;
            if (progress >= 1f)
            {
                progress = 1f;
                achieveState = AchieveState.Completed;
            }
            else
                achieveState = AchieveState.Proceeding;
        }
    }
}
