using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SG
{
    public class EquipSlot : MonoBehaviour
    {
        [SerializeField] private EquipItem item;
        internal Button itemBtn;
        private Image icon;
        private Image enhanceFrame;
        private TMP_Text enhanceLevelText;

        [SerializeField] private bool isArmed;
        [SerializeField] internal bool isSelect = false;

        [SerializeField] private Sprite frameNormal;
        [SerializeField] private Sprite frameGold;

        [SerializeField] private CharacterUI_EquipmentPanel equipmentPanel;

        public void Init()
        {
            equipmentPanel = GetComponentInParent<CharacterUI_EquipmentPanel>();

            itemBtn = GetComponentInChildren<Button>(true);
            if (itemBtn != null)
            {
                //버튼을 누르면, 해당 아이템 정보가 오른쪽에 출력
                itemBtn.onClick.AddListener(equipmentPanel.DeselectAllEquipSlots);
                itemBtn.onClick.AddListener(() => OpenIndividualEquipItemPanel(item));
            }

            enhanceFrame = UtilHelper.Find<Image>(transform, "EnhanceFrame");
            enhanceLevelText = UtilHelper.Find<TMP_Text>(transform, "EnhanceFrame/Text");
            icon = UtilHelper.Find<Image>(transform, "Button/Icon");
            frameNormal = Resources.Load<Sprite>("Sprites/Item/Frame/frame_normal");
            frameGold = Resources.Load<Sprite>("Sprites/Item/Frame/frame_select");
        }

        public void OpenIndividualEquipItemPanel(EquipItem equipItem)
        {
            isSelect = true;
            ChangeFrameColor(true);
            equipmentPanel.OpenIndividualItemPanel();
            equipmentPanel.SetParameterIndividualEquipItem(equipItem);
        }

        public EquipItem GetEquipItem()
        {
            return item;
        }

        //장비 슬롯의 정보를 추가하거나 업데이트할 때 쓰는 함수입니다.
        public void AddItem(EquipItem equipItem)
        {
            item = equipItem;
            icon.sprite = equipItem.itemIcon;
            isArmed = equipItem.isArmed;
            enhanceLevelText.text = "+" + equipItem.enforceLevel;

            if (icon.sprite == null)
                icon.enabled = false;
            else
                icon.enabled = true;

            if (equipItem.enforceLevel == 0)
                enhanceFrame.gameObject.SetActive(false);
            else
                enhanceFrame.gameObject.SetActive(true);

        }
        public void ClearEquipSlot()
        {
            item = null;
            icon = null;
            isArmed = false;
            ChangeFrameColor(false);
            gameObject.SetActive(false);
        }
        public void ChangeFrameColor(bool state)
        {
            //선택, 미선택 상태인 장비 슬롯을 구별하기 위해 프레임 스프라이트를 바꾼다.
            //임시적인 방안이므로, 나중에 다른 방법을 사용할 수 있습니다.
            if (state == true)
            {       
                itemBtn.GetComponent<Image>().sprite = frameGold;
                enhanceFrame.sprite = frameGold;
            }
            else
            {
                itemBtn.GetComponent<Image>().sprite = frameNormal;
                enhanceFrame.sprite = frameNormal;
            }
        }
    }
}

