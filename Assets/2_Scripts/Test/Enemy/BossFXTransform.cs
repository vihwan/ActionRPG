using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class BossFXTransform : EnemyFXTransform
    {
        [Header("Second Phase FX")]
        public GameObject secondPhaseFX;

        [Header("Current Effect Fx List")]
        [SerializeField] private List<GameObject> currentFxList = new List<GameObject>();

        public override void Init()
        {
            base.Init();
        }

        public void InstantiateParticleFX(string fxName)
        {
            if(fxName == "SecondPhase_Keiya")
            {
                GameObject go = Instantiate(secondPhaseFX, this.transform);
                currentFxList.Add(go);
            }          
        }

        public void DestroyParticleFX(string fxName)
        {
            if(currentFxList.Count > 0)
            {
                foreach (GameObject fx in currentFxList)
                {
                    if(fx.name.Equals(fxName + "(Clone)"))
                    {
                        Debug.Log("이름 비교 성공");
                        ParticleSystem[] particleSystems = fx.GetComponentsInChildren<ParticleSystem>();
                        foreach (ParticleSystem ps in particleSystems)
                        {
                            ps.Stop();
                        }
                        Destroy(fx, 3f);
                        currentFxList.Remove(fx);
                        break;
                    }
                }
            }
        }
    }
}
