using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extension;

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
            UdpManager.it?.Init();

            Database.it.gameObject.SetParentEx(this.transform);
            GUIManager.it.gameObject.SetParentEx(this.transform);
            NPCManager.it.gameObject.SetParentEx(this.transform);
            PlayerManager.it.gameObject.SetParentEx(this.transform);
            ItemDropManager.it.gameObject.SetParentEx(this.transform);
            DialogueManager.it.gameObject.SetParentEx(this.transform);
            EnforceManager.it.gameObject.SetParentEx(this.transform);
            QuestManager.it.gameObject.SetParentEx(this.transform);
            AchieveManager.it.gameObject.SetParentEx(this.transform);
            LevelManager.it.gameObject.SetParentEx(this.transform);
            WorldEventManager.it.gameObject.SetParentEx( this.transform);
            UdpManager.it.gameObject.SetParentEx(this.transform);
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
