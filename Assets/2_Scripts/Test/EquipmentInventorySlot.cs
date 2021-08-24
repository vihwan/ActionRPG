using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class EquipmentInventorySlot : MonoBehaviour
    {
        [SerializeField] internal EquipItem item;
        [SerializeField] private Button itemBtn;
        [SerializeField] private Image icon;
        [SerializeField] private bool isArmed;
        [SerializeField] internal bool isSelect = false;


        private EquipmentInventoryList equipmentInventoryList;

        private void Awake()
        {
            itemBtn = GetComponentInChildren<Button>();
            if (itemBtn != null)
            {
                CharacterUI_EquipmentPanel equipPanel = FindObjectOfType<CharacterUI_EquipmentPanel>();
                itemBtn.onClick.AddListener(() => equipPanel.SetParameterIndividualEquipItem(item));
                itemBtn.onClick.AddListener(SelectSlot);
               // itemBtn.onClick.AddListener(() => CheckSlotIsCurrentEquipment());
            }
            icon = UtilHelper.Find<Image>(itemBtn.transform, "Image");

            equipmentInventoryList = GetComponentInParent<EquipmentInventoryList>();
        }

        public void AddItem(EquipItem equipItem)
        {
            gameObject.SetActive(true);
            item = equipItem;
            icon.sprite = equipItem.itemIcon;
            icon.enabled = true;
            isArmed = equipItem.isArmed;
            ChangeBackgroundColor();
        }

        public void SelectSlot()
        {
            foreach (EquipmentInventorySlot slot in equipmentInventoryList.equipmentInventorySlots)
            {
                slot.isSelect = false;
                slot.ChangeBackgroundColor();
            }
            isSelect = true;
            ChangeBackgroundColor();
        }

        public void UpdateSlot(EquipItem equipItem)
        {
            isArmed = equipItem.isArmed;
            ChangeBackgroundColor();
            Debug.Log(equipItem.name + "의 isArmed : " + equipItem.isArmed);
        }

        private void CheckSlotIsCurrentEquipment(EquipItem equipItem)
        {
            if (item == equipItem)
            {
                CharacterUI_EquipmentPanel ch = FindObjectOfType<CharacterUI_EquipmentPanel>();
                if (ch != null)
                {
                    if (ch.comparisonPanel.activeSelf == true)
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
