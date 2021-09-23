using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        private Database database;
        private PlayerManager playerManager;
        private NPCManager[] npcManagers;
        private GUIManager guiManager;
        private ItemDropManager itemDropManager;
        private DialogueManager dialogueManager;
        private EnforceManager enforceManager;
        private QuestManager questManager;
        private AchieveManager achieveManager;
        private LevelManager levelManager;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            database = FindObjectOfType<Database>();
            if (database != null)
                database.Init();

            guiManager = FindObjectOfType<GUIManager>();
            if (guiManager != null)
                guiManager.Init();

            npcManagers = FindObjectsOfType<NPCManager>();
            if(npcManagers != null)
            {
                for (int i = 0; i < npcManagers.Length; i++)
                {
                    npcManagers[i].Init();
                }
            }
            
            playerManager = FindObjectOfType<PlayerManager>();
            if (playerManager != null)
                playerManager.Init();

            itemDropManager = GetComponentInChildren<ItemDropManager>();
            if (itemDropManager != null)
                itemDropManager.Init();

            dialogueManager = GetComponentInChildren<DialogueManager>();
            if (dialogueManager != null)
                dialogueManager.Init();

            enforceManager = GetComponentInChildren<EnforceManager>();
            if (enforceManager != null)
                enforceManager.Init();

            questManager = GetComponentInChildren<QuestManager>();
            if (questManager != null)
                questManager.Init();

            achieveManager = GetComponentInChildren<AchieveManager>();
            if (achieveManager != null)
                achieveManager.Init();

            levelManager = GetComponentInChildren<LevelManager>();
            if (levelManager != null)
                levelManager.Init();

            DontDestroyOnLoad(this.gameObject);
        }
    }

}
