using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SG
{
    public class InventoryMain_ContentList : MonoBehaviour
    {
        private Transform inventorySlotsParent;
        private TMP_Dropdown itemSort_Dropdown;
        private Button deleteBtn;
        [SerializeField] internal InventoryContentSlot[] inventoryContentSlots;

        InventoryMainContents inventoryMainContents;
        public void Init()
        {
            Transform t = transform;
            inventorySlotsParent = UtilHelper.Find<Transform>(t, "Slot Parent");

            itemSort_Dropdown = UtilHelper.Find<TMP_Dropdown>(t, "Dropdown");
            if (itemSort_Dropdown != null)
            {
                InitDropdown();
                itemSort_Dropdown.onValueChanged.AddListener(OnClickSortInventoryList);
            }

            deleteBtn = UtilHelper.Find<Button>(t, "DeleteButton");
            if (deleteBtn != null)
                deleteBtn.onClick.AddListener(OnClickDelete);

            inventoryMainContents = GetComponentInParent<InventoryMainContents>();
        }

        private void InitDropdown()
        {
            itemSort_Dropdown.ClearOptions();

            List<string> options = new List<string>();
            options.Add("가나다순");
            options.Add("레어도순");

            itemSort_Dropdown.AddOptions(options);
            itemSort_Dropdown.value = 0;
            itemSort_Dropdown.RefreshShownValue();
        }

        private void OnClickDelete()
        {

        }

        internal void OnClickSortInventoryList(int value)
        {
            switch (value)
            {
                case 0:
                    SortInventoryListToGANADA();
                    break;
                case 1:
                    SortInventoryListToRarity();
                    break;
            }
        }

        private void SortInventoryListToGANADA()
        {
            inventoryMainContents.playerInventory.SortWeaponInventoryListToGANADA();
        }

        private void SortInventoryListToRarity()
        {
            inventoryMainContents.playerInventory.SortWeaponInventoryListToRarity();
        }
    }
}
