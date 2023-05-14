using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class NPCManager : MonoBehaviourSingleton<NPCManager>
    {
        private NPCData[] statuses;
        public NPCData[] npcs => statuses;

        public void init()
        {
            statuses = FindObjectsOfType<NPCData>();
            if (statuses != null)
            {
                for (int i = 0; i < statuses.Length; i++)
                {
                    statuses[i].Init();
                }
            }
        }
    }
}
