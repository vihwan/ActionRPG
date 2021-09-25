using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace SG
{
    public class LevelUpAlertUI : MonoBehaviour
    {
        private Animator animator;
        [SerializeField] private TMP_Text levelText;

        public void Init()
        {
            animator = GetComponent<Animator>();
            FindObjectOfType<LevelManager>().OnLevelChanged += LevelSystem_OnLevelChange;
        }

        private void LevelSystem_OnLevelChange(object sender, EventArgs e)
        {
            PlayLevelUpAlertAnimation();
        }

        public void PlayLevelUpAlertAnimation()
        {
            levelText.text = string.Format("{0}", LevelManager.Instance.Level);
            animator.SetTrigger("LevelUp");
        }
    }
}
