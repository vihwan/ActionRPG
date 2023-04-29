using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class AchieveManager : MonoBehaviour
    {
        public static AchieveManager Instance;

        [SerializeField] public List<Achieve> achieves = new List<Achieve>();

        public void Init()
        {
            if(Instance == null)
                Instance = this;

            Achieve[] achieveArr;
            achieveArr = Resources.LoadAll<Achieve>("Scriptable/Achieve");
            for (int i = 0; i < achieveArr.Length; i++)
            {
                achieves.Add(achieveArr[i]);
            }
        }
    }
}
