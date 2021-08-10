using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        private WeaponItem item;
        public Image icon;

        private void Start()
        {
            icon = GetComponentInChildren<Image>(true);
        }

        public void AddItem(WeaponItem weaponItem)
        {
            item = weaponItem;
            icon.sprite = weaponItem.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            item = null;
            icon = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }
    }
}

