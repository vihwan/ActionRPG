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
        [SerializeField] internal ConsumableItem consumableItem;
        [SerializeField] internal IngredientItem ingredientItem;

        [SerializeField] private InventoryMainContents mainContents;
        [SerializeField] private InventoryMain_ContentList contentList;
        private void Awake()
        {
            mainContents = GetComponentInParent<InventoryMainContents>();
            contentList = GetComponentInParent<InventoryMain_ContentList>();

            itemBtn = GetComponentInChildren<Button>();
            if (itemBtn != null)
            {
                itemBtn.onClick.AddListener(() => mainContents.infoPanel.SetParameter(this));
                itemBtn.onClick.AddListener(() => contentList.SetBeforeSelectSlot(this));
                itemBtn.onClick.AddListener(SelectSlot);
            }
            icon = UtilHelper.Find<Image>(itemBtn.transform, "Image");
            quantityText = UtilHelper.Find<TMP_Text>(itemBtn.transform, "QuantityText");
        }

        public void AddItem(WeaponItem weaponItem)
        {
            ClearItem();

            gameObject.SetActive(true);
            this.weaponItem = weaponItem;
            icon.sprite = weaponItem.itemIcon;
            icon.enabled = true;
            isArmed = weaponItem.isArmed;

            quantityText.gameObject.SetActive(false);
            ChangeBackgroundColor();
        }

        public void AddItem(EquipItem equipItem)
        {
            ClearItem();

            gameObject.SetActive(true);
            this.equipItem = equipItem;
            icon.sprite = equipItem.itemIcon;
            icon.enabled = true;
            isArmed = equipItem.isArmed;

            quantityText.gameObject.SetActive(false);
            ChangeBackgroundColor();
        }

        public void AddItem(ConsumableItem consumableItem)
        {
            ClearItem();

            gameObject.SetActive(true);
            this.consumableItem = consumableItem;
            icon.sprite = consumableItem.itemIcon;
            icon.enabled = true;
            isArmed = false;

            quantityText.text = consumableItem.quantity.ToString();
            quantityText.gameObject.SetActive(true);
            ChangeBackgroundColor();
        }

        public void AddItem(IngredientItem ingredientItem)
        {
            ClearItem();

            gameObject.SetActive(true);
            this.ingredientItem = ingredientItem;
            icon.sprite = ingredientItem.itemIcon;
            icon.enabled = true;
            isArmed = false;

            quantityText.text = ingredientItem.quantity.ToString();
            quantityText.gameObject.SetActive(true);
            ChangeBackgroundColor();
        }

        public void ClearItem()
        {
            weaponItem = null;
            equipItem = null;
            consumableItem = null;
            ingredientItem = null;

            icon.enabled = false;
            icon.sprite = null;
            isArmed = false;
            gameObject.SetActive(false);
        }

        public void SelectSlot()
        {
            contentList.DeSelectAllSlots();
            isSelect = true;
            ChangeBackgroundColor();
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
