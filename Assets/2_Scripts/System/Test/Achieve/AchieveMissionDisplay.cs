using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class AchieveMissionDisplay : MonoBehaviour
    {
        public TMP_Text titleText;
        public TMP_Text explainText;
        public Image missionIcon;
        public TMP_Text progressText;


        public void Init()
        {
            titleText = UtilHelper.Find<TMP_Text>(transform, "MissionTitle");
            explainText = UtilHelper.Find<TMP_Text>(transform, "MissionExplain");
            missionIcon = UtilHelper.Find<Image>(transform, "Icon");
            progressText = UtilHelper.Find<TMP_Text>(transform, "Image/ProgressText");
        }

        public void SetMission(AchieveMission achieveMission)
        {
            titleText.text = achieveMission.missionName;
            explainText.text = achieveMission.missionExplain;
            missionIcon.sprite = achieveMission.missionIcon;
            progressText.text = string.Format("( {0} / {1} )", 
                                            achieveMission.CurrentProgressCount, achieveMission.maxProgressCount);
        }
    }
}
