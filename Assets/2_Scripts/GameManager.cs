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
        private GUIManager guiManager;
        private ItemDropManager itemDropManager;
        private DialogueManager dialogueManager;
        private EnforceManager enforceManager;
        private QuestManager questManager;
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

            DontDestroyOnLoad(this.gameObject);
        }
    }

}
