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
        [SerializeField] private Transform rarityTransform;
        [SerializeField] private List<GameObject> rareStars;
        [Header("Prefab")]
        [SerializeField] private GameObject RareStar;

        [Header("Enforce Image")]
        [SerializeField] private Image enforceImage;
        [SerializeField] private TMP_Text enforceText;

        [Header("Reference Item")]
        [SerializeField] internal WeaponItem weaponItem;
        [SerializeField] internal EquipItem equipItem;
        [SerializeField] internal ConsumableItem consumableItem;
        [SerializeField] internal IngredientItem ingredientItem;

        [Header("Image Source")]
        [SerializeField] private Sprite selectFrameSprite;
        [SerializeField] private Sprite isArmedFrameSprite;

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
            icon = UtilHelper.Find<Image>(itemBtn.transform, "ItemIcon");
            quantityText = UtilHelper.Find<TMP_Text>(itemBtn.transform, "QuantityText");
            rarityTransform = itemBtn.transform.Find("RarityTransform").transform;
            RareStar = Resources.Load<Image>("Prefabs/RarityStar").gameObject;

            enforceImage = UtilHelper.Find<Image>(itemBtn.transform, "EnforceImage");
            enforceText = UtilHelper.Find<TMP_Text>(enforceImage.transform, "EnforceText");
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
            enforceImage.gameObject.SetActive(true);
            enforceText.text = weaponItem.enforceLevel.ToString();
            CreateRarityStar(rarityTransform, rareStars, weaponItem);
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
            enforceImage.gameObject.SetActive(true);
            enforceText.text = equipItem.enforceLevel.ToString();
            CreateRarityStar(rarityTransform, rareStars, equipItem);
            ChangeBackgroundColor();
        }

        public void AddItem(ConsumableItem consumableItem)
        {
            ClearItem();

            gameObject.SetActive(true);
            this.consumableItem = consumableItem;
            icon.sprite = consumableItem.itemIcon;
            icon.enabled = true;
            isArmed = consumableItem.isArmed;

            quantityText.text = consumableItem.quantity.ToString();
            quantityText.gameObject.SetActive(true);
            enforceImage.gameObject.SetActive(false);
            CreateRarityStar(rarityTransform, rareStars, consumableItem);
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
            enforceImage.gameObject.SetActive(false);
            CreateRarityStar(rarityTransform, rareStars, ingredientItem);
            ChangeBackgroundColor();
        }

        private void CreateRarityStar(Transform transform, List<GameObject> rareStarsList, Item item)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            rareStarsList.Clear();

            int rarityCount = 0;
            if (rarityCount < item.rarity)
            {
                while (rarityCount < item.rarity)
                {
                    GameObject star = Instantiate(RareStar, transform);
                    rareStarsList.Add(star);
                    rarityCount++;
                }
            }
        }

        public void ClearItem()
        {
            weaponItem = null;
            equipItem = null;
            consumableItem = null;
            ingredientItem = null;

            isArmed = false;
            gameObject.SetActive(false);
        }

        public bool GetIsArmed()
        {
            return this.isArmed;
        }

        public void SelectSlot()
        {
            contentList.SetAllSlotsDeselect();
            isSelect = true;
            ChangeBackgroundColor();
        }

        public void UpdateSlot(ConsumableItem consumableItem)
        {
            isArmed = consumableItem.isArmed;
            ChangeBackgroundColor();
            Debug.Log(consumableItem.name + "의 isArmed : " + consumableItem.isArmed);
        }
        public void ChangeBackgroundColor()
        {
            //장착, 미장착 상태인 무기를 구별하기 위해 버튼 색상을 바꾼다.
            //임시적인 방안이므로, 나중에 다른 방법을 사용할 수 있습니다.
            if (isArmed)
            {
                itemBtn.GetComponent<Image>().sprite = Database.Instance.prefabDatabase.itemSlotIsArmed;
                enforceImage.sprite = Database.Instance.prefabDatabase.itemSlotIsArmed;
                return;
            }
            else if (isSelect)
            {
                itemBtn.GetComponent<Image>().sprite = Database.Instance.prefabDatabase.itemSlotIsSelect;
                enforceImage.sprite = Database.Instance.prefabDatabase.itemSlotIsSelect;
            }               
            else if (!isSelect)
            {
                itemBtn.GetComponent<Image>().sprite = Database.Instance.prefabDatabase.itemSlotNormal;
                enforceImage.sprite = Database.Instance.prefabDatabase.itemSlotNormal;
            }
        }
    }
}
