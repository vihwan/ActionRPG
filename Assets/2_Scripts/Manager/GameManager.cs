using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        private void Awake()
        {
            if (Database.it != null)
                Database.it.Init();

            if (GUIManager.it != null)
                GUIManager.it.Init();

            if (NPCManager.it != null)
                NPCManager.it.init();

            if(PlayerManager.it != null)
                PlayerManager.it.Init();

            if (ItemDropManager.it != null)
                ItemDropManager.it.Init();

            if (DialogueManager.it != null)
                DialogueManager.it.Init();

            if (EnforceManager.it != null)
                EnforceManager.it.Init();

            if (QuestManager.it != null)
                QuestManager.it.Init();

            if (AchieveManager.it != null)
                AchieveManager.it.Init();

            if (LevelManager.it != null)
                LevelManager.it.Init();

            if(WorldEventManager.it != null)
                WorldEventManager.it.Init();

            if (UdpManager.it != null)
                UdpManager.it.Init();
        }
    }
}
