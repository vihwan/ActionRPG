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

    public class LevelManager : MonoBehaviourSingleton<LevelManager>
    {
        private PlayerManager playerManager;
        public event EventHandler OnExperienceChanged;
        public event EventHandler OnLevelChanged;

        [SerializeField] private int level;
        [SerializeField] private int experience;

        [SerializeField] private List<LevelToExperience> levelToExperiences = new List<LevelToExperience>();

        public int Level { get => level; private set => level = value; }
        public int Experience { get => experience; private set => experience = value; }


        public void Init()
        {
            playerManager = FindObjectOfType<PlayerManager>();

            Level = 0;
            Experience = 0;

            levelToExperiences = CSVReader.ReadCSV_ExpTable();
            SetLevelManagerEventHandler();
        }

        public void SetLevelManagerEventHandler()
        {
            GUIManager.it.levelWindow.SetLevelSystem();
            playerManager.SetLevelSystem();
        }

        public void AddExperience(int amount)
        {
            Experience += amount;
            while (Experience >= GetExperienceNextLevel(Level))
            {
                Experience -= GetExperienceNextLevel(Level);
                Level++;
                if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
            }
            if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
        }

        public float GetExperienceNormalized()
        {
            return (float)Experience / GetExperienceNextLevel(Level);
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

