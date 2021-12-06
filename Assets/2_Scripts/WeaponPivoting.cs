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
    class UnEquipPivot{
        public Vector3 position = new Vector3((float)0.446,(float)0.166,(float)-0.288);
        public Vector3 rotation = new Vector3((float)58.201,(float)60.138,(float)-63.386); 
    }
    public class WeaponPivoting : MonoBehaviour
    {
        NormalPivot normalPivot = new NormalPivot();
        GuardPivot guardPivot = new GuardPivot();
        UnEquipPivot unEquipPivot = new UnEquipPivot();

        public void NormalPivoting()
        {
            this.gameObject.transform.localPosition = normalPivot.position;
            this.gameObject.transform.localEulerAngles = normalPivot.rotation;
        }

        public void GuardPivoting()
        {
            this.gameObject.transform.localPosition = guardPivot.position;
            this.gameObject.transform.localEulerAngles = guardPivot.rotation;
        }

        public void UnEquipPivoting()
        {
            this.gameObject.transform.localPosition = unEquipPivot.position;
            this.gameObject.transform.localEulerAngles = unEquipPivot.rotation;
        }
    }
}

