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


            DontDestroyOnLoad(this.gameObject);
        }
    }

}
