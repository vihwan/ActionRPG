using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SG
{
    public class EquipSlot : MonoBehaviour
    {
        private EquipItem item;
        private Button itemBtn;
        private Image icon;
        private TMP_Text enhanceLevelText;

        [SerializeField] private bool isArmed;
        [SerializeField] internal bool isSelect = false;


        private void Awake()
        {
            itemBtn = GetComponentInChildren<Button>(true);
            if(itemBtn != null)
            {
                //버튼을 누르면, 해당 아이템 정보가 오른쪽에 출력
                itemBtn.onClick.AddListener(null);
            }
            enhanceLevelText = UtilHelper.Find<TMP_Text>(transform, "EnhanceFrame/Text");
            icon = UtilHelper.Find<Image>(transform, "Button/Icon");
        }

        public void AddItem(EquipItem equipItem)
        {
            item = equipItem;
            icon.sprite = equipItem.itemIcon;
            icon.enabled = true;
            isArmed = equipItem.isArmed;
            enhanceLevelText.text = "+" + equipItem.enhanceLevel;
            ChangeBackgroundColor();
        }

        public void ClearEquipSlot()
        {
            item = null;
            icon = null;
            icon.enabled = false;
            isArmed = false;
            ChangeBackgroundColor();
            gameObject.SetActive(false);
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

