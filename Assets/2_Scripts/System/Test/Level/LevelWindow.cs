using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class LevelWindow : MonoBehaviour
    {
        [SerializeField] private ExpBar expBar;
        [SerializeField] private TMPro.TMP_Text levelText;

        public void Init()
        {
            expBar = GetComponentInChildren<ExpBar>(true);
            if (expBar != null)
                expBar.Init();

            levelText = UtilHelper.Find<TMPro.TMP_Text>(transform, "LevelText");
        }

        public void SetLevelSystem()
        {
            SetLevelText(LevelManager.it.Level);
            SetExpSlider(LevelManager.it.GetExperienceNormalized());
            SetExpText(LevelManager.it.Experience, LevelManager.it.GetExperienceNextLevel(LevelManager.it.Level));

            LevelManager.it.OnLevelChanged += LevelSystem_OnLevelChanged;
            LevelManager.it.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        }


        private void SetExpText(int currentExp, int nextLevelExp)
        {
            expBar.expText.text = string.Format("{0} / {1}", currentExp, nextLevelExp);
        }

        private void SetExpSlider(float experienceNormalize)
        {
            expBar.slider.value = experienceNormalize;
        }

        private void SetLevelText(int level)
        {
            levelText.text = string.Format("Lv. {0}", level);
        }
        
        private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
        {
            SetLevelText(LevelManager.it.Level);
        }

        private void LevelSystem_OnExperienceChanged(object sender, EventArgs e)
        {
            SetExpSlider(LevelManager.it.GetExperienceNormalized());
            SetExpText(LevelManager.it.Experience, LevelManager.it.GetExperienceNextLevel(LevelManager.it.Level));
        }
    }
}
