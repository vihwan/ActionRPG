using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace SG
{
    public class ShopListSlot : MonoBehaviour
    {
        [SerializeField] private Item item;
        [SerializeField] private Image itemIcon;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private Button slotBtn;

        private ShopPanel shopPanel;
        private void Awake()
        {
            shopPanel = GetComponentInParent<ShopPanel>();

            itemIcon = UtilHelper.Find<Image>(transform, "ItemIconMask/Icon");
            nameText = UtilHelper.Find<TMP_Text>(transform, "NameText");
            goldText = UtilHelper.Find<TMP_Text>(transform, "GoldImage/priceText");

            slotBtn = GetComponent<Button>();
            if (slotBtn != null)
                slotBtn.onClick.AddListener(OnClickSlotBtn);
            //슬롯 버튼을 누르면, 오른쪽에 정보 패널을 갱신
        }
        public void SetSlot(Item item)
        {
            this.item = item;
            itemIcon.sprite = item.itemIcon;
            nameText.text = item.itemName;
            goldText.text = item.price.ToString();
        }

        public void OnClickSlotBtn()
        {
            shopPanel.infoPanel.SetParameter(item);
        }
    }
}
