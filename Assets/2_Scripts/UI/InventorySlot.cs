using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SG
{
    public abstract class InventorySlot : MonoBehaviour
    {
        public Button itemBtn;
        public Image icon;
        public bool isSelect = false;
    }
}
