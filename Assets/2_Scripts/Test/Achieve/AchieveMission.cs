using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public enum AchieveMissionState
    {
        Proceeding,
        Completed,
    }

    [System.Serializable]
    public class AchieveMission
    {
        public string missionName;
        [TextArea(1, 1)]
        public string missionExplain;
        public Sprite missionIcon;
        [SerializeField] private int currentProgressCount;
        public int maxProgressCount;

        public AchieveMissionState achieveMissionState;
        public MissionTarget missionTarget;

        public int CurrentProgressCount
        {
            get => currentProgressCount;
            private set
            {
                currentProgressCount = value;
                if (currentProgressCount >= maxProgressCount)
                {
                    currentProgressCount = maxProgressCount;
                    achieveMissionState = AchieveMissionState.Completed;
                }
            }
        }

        /// <summary>
        /// Called when the script is loaded or a value is changed in the
        /// inspector (Called in the editor only).
        /// </summary>
        void OnValidate()
        {
            Debug.Log("OnValidate Activated");
            if (currentProgressCount >= maxProgressCount)
            {
                currentProgressCount = maxProgressCount;
                achieveMissionState = AchieveMissionState.Completed;
            }
            else
                achieveMissionState = AchieveMissionState.Proceeding;
        }
    }
}
