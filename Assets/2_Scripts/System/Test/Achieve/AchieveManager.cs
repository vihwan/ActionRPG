using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class AchieveManager : MonoBehaviourSingleton<AchieveManager>
    {
        [SerializeField] public List<Achieve> achieves = new List<Achieve>();

        public void Init()
        {
            Achieve[] achieveArr;
            achieveArr = Resources.LoadAll<Achieve>("Scriptable/Achieve");
            for (int i = 0; i < achieveArr.Length; i++)
            {
                achieves.Add(achieveArr[i]);
            }
        }
    }
}
