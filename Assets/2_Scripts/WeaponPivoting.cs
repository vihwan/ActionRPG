using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    class NormalPivot{
        public Vector3 position = new Vector3((float)0.445,(float)0.185,(float)-0.298);
        public Vector3 rotation = new Vector3((float)-7.555,(float)29.576,(float)-76.956);
    }
    class GuardPivot{
        public Vector3 position = new Vector3((float)0.348,(float)0.182,(float)-0.455);
        public Vector3 rotation = new Vector3((float)56.641,(float)68.25401,(float)-69.236);
    }
    public class WeaponPivoting : MonoBehaviour
    {
        NormalPivot normalPivot = new NormalPivot();
        GuardPivot guardPivot = new GuardPivot();

        public void NormalPivot()
        {
            this.gameObject.transform.localPosition = normalPivot.position;
            this.gameObject.transform.localEulerAngles = normalPivot.rotation;
        }

        public void GuardPivot()
        {
            this.gameObject.transform.localPosition = guardPivot.position;
            this.gameObject.transform.localEulerAngles = guardPivot.rotation;
        }
    }
}

