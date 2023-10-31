using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        void Awake()
        {
            DontDestroyOnLoad(this);

            Database.it?.Init();
            GUIManager.it?.Init();
            NPCManager.it?.init();
            PlayerManager.it?.Init();
            ItemDropManager.it?.Init();
            DialogueManager.it?.Init();
            EnforceManager.it?.Init();
            QuestManager.it?.Init();
            AchieveManager.it?.Init();
            LevelManager.it?.Init();
            WorldEventManager.it?.Init();
            UdpManager.it.Init();
        }


        void Reset()
        {

        }

        private void OnApplicationQuit()
        {
            DestroyImmediate(this);
        }
    }
}
