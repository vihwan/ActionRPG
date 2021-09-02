using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class WeaponInventorySlot : InventorySlot
    {
        [SerializeField] internal WeaponItem item;
        [SerializeField] private bool isArmed;

        private WeaponInventoryList weaponInventoryList;
        private void Awake()
        {
            itemBtn = GetComponentInChildren<Button>();
            if (itemBtn != null)
            {
                CharacterUI_WeaponPanel weaponPanel = FindObjectOfType<CharacterUI_WeaponPanel>();
                itemBtn.onClick.AddListener(() => weaponPanel.SetParameter(item));
                itemBtn.onClick.AddListener(SelectSlot);
                itemBtn.onClick.AddListener(() => CheckSlotIsCurrentWeapon(weaponInventoryList.playerInventory.currentWeapon));
            }
            icon = UtilHelper.Find<Image>(itemBtn.transform, "Image");

            weaponInventoryList = GetComponentInParent<WeaponInventoryList>();
        }

        public void AddItem(WeaponItem weaponItem)
        {
            gameObject.SetActive(true);
            item = weaponItem;
            icon.sprite = weaponItem.itemIcon;
            icon.enabled = true;
            isArmed = weaponItem.isArmed;
            ChangeBackgroundColor();
        }

        public void ClearInventorySlot()
        {
            item = null;
            icon = null;
            icon.enabled = false;
            isArmed = false;
            ChangeBackgroundColor();
            gameObject.SetActive(false);
        }
        public void SelectSlot()
        {
            foreach (WeaponInventorySlot slot in weaponInventoryList.weaponInventorySlots)
            {
                slot.isSelect = false;
                slot.ChangeBackgroundColor();
            }
            isSelect = true;
            ChangeBackgroundColor();
        }

        public void UpdateSlot(WeaponItem weaponItem)
        {
            isArmed = weaponItem.isArmed;
            ChangeBackgroundColor();
            Debug.Log(weaponItem.name + "의 isArmed : " + weaponItem.isArmed);
        }

        private void CheckSlotIsCurrentWeapon(WeaponItem currentWeapon)
        {
            if(item == currentWeapon)
            {
                CharacterUI_WeaponPanel ch = FindObjectOfType<CharacterUI_WeaponPanel>();
                if (ch != null)
                {
                    if(ch.comparisonPanel.activeSelf == true)
                        ch.CloseComparisonPanel();
                }
            }
        }

        public void ChangeBackgroundColor()
        {
            //장착, 미장착 상태인 무기를 구별하기 위해 버튼 색상을 바꾼다.
            //임시적인 방안이므로, 나중에 다른 방법을 사용할 수 있습니다.
            if (isArmed)
            {
                itemBtn.GetComponent<Image>().color = Color.cyan;
                return;
            }
            else if (isSelect)
                itemBtn.GetComponent<Image>().color = Color.green;
            else if (!isSelect)
                itemBtn.GetComponent<Image>().color = Color.white;
        }

    }
}