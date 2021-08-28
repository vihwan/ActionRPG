using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SG
{
    public class InventoryContentSlot : InventorySlot
    {
        [SerializeField] private bool isArmed;
        [SerializeField] private TMP_Text quantityText;

        [Header("Reference Item")]
        [SerializeField] internal WeaponItem weaponItem;
        [SerializeField] internal EquipItem equipItem;

        private void Awake()
        {
            itemBtn = GetComponentInChildren<Button>();
            if (itemBtn != null)
            {
                itemBtn.onClick.AddListener(null);
            }
            icon = UtilHelper.Find<Image>(itemBtn.transform, "Image");
            quantityText = UtilHelper.Find<TMP_Text>(itemBtn.transform, "QuantityText");
        }

        public void AddItem()
        {

        }
    }
}
