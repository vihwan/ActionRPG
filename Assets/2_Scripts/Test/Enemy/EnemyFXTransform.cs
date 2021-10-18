using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class EnemyFXTransform : MonoBehaviour
    {
        [SerializeField] private List<GameObject> normalFxList = new List<GameObject>();
        public virtual void Init()
        {
            if(normalFxList.Count > 0)
            {
                foreach (var fx in normalFxList)
                {
                    Instantiate(fx, this.transform);
                }
            }
        }
    }
}
