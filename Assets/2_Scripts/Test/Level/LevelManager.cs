using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{

    [System.Serializable]
    public class LevelToExperience
    {
        public int Level;
        public int Experience;
    }

    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;
        private PlayerManager playerManager;
        public event EventHandler OnExperienceChanged;
        public event EventHandler OnLevelChanged;

        [SerializeField] private int level;
        [SerializeField] private int experience;

        [SerializeField] private List<LevelToExperience> levelToExperiences = new List<LevelToExperience>();

        public int Experience { get => experience; private set => experience = value; }

        public void Init()
        {
            if (Instance == null)
                Instance = this;

            playerManager = FindObjectOfType<PlayerManager>();

            level = 0;
            Experience = 0;

            levelToExperiences = CSVReader.ReadCSV_ExpTable();
            SetLevelManagerEventHandler();
        }

        public void SetLevelManagerEventHandler()
        {
            GUIManager.instance.levelWindow.SetLevelSystem();
            playerManager.SetLevelSystem();
        }

        public void AddExperience(int amount)
        {
            Experience += amount;
            while (Experience >= GetExperienceNextLevel(level))
            {
                Experience -= GetExperienceNextLevel(level);
                level++;
                if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
            }
            if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
        }

        public int GetLevel()
        {
            return level;
        }

        public float GetExperienceNormalized()
        {
            return (float)Experience / GetExperienceNextLevel(level);
        }

        public int GetExperienceNextLevel(int level)
        {
            if (level < levelToExperiences.Count)
                return levelToExperiences[level].Experience;
            else
            {
                Debug.LogError("Level Invaild" + level);
                return 100;
            }
        }
    }
}

