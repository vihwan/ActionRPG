using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class WeaponInventorySlot : InventorySlot
    {
        [SerializeField] internal WeaponItem item;
        [SerializeField] private bool isArmed;
        [SerializeField] private Transform rarityTransform;
        [SerializeField] private List<GameObject> rareStars;
        [Header("Prefab")]
        [SerializeField] private GameObject RareStar;

        [Header("Enforce Image")]
        [SerializeField] private Image enforceImage;
        [SerializeField] private TMP_Text enforceText;


        private WeaponInventoryList weaponInventoryList;
        private void Awake()
        {
            itemBtn = GetComponentInChildren<Button>();
            if (itemBtn != null)
            {
                CharacterUI_WeaponPanel weaponPanel = FindObjectOfType<CharacterUI_WeaponPanel>();
                itemBtn.onClick.AddListener(() => weaponPanel.SetParameter(item));
                itemBtn.onClick.AddListener(SelectSlot);
                itemBtn.onClick.AddListener(() => CheckSlotIsCurrentWeapon(PlayerInventory.Instance.currentWeapon));
            }
            icon = UtilHelper.Find<Image>(itemBtn.transform, "ItemIcon");
            rarityTransform = itemBtn.transform.Find("RarityTransform").transform;
            RareStar = Resources.Load<Image>("Prefabs/RarityStar").gameObject;

            enforceImage = UtilHelper.Find<Image>(itemBtn.transform, "EnforceImage");
            enforceText = UtilHelper.Find<TMP_Text>(enforceImage.transform, "EnforceText");

            weaponInventoryList = GetComponentInParent<WeaponInventoryList>();
        }

        public void AddItem(WeaponItem weaponItem)
        {
            gameObject.SetActive(true);
            item = weaponItem;
            icon.sprite = weaponItem.itemIcon;
            icon.enabled = true;
            CreateRarityStar(rarityTransform, rareStars, item);
            isArmed = weaponItem.isArmed;

            enforceImage.gameObject.SetActive(true);
            enforceText.text = weaponItem.enforceLevel.ToString();
            ChangeBackgroundColor();
        }

        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            isArmed = false;
            ChangeBackgroundColor();
            gameObject.SetActive(false);
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

        public void UpdateSlotIsArmed(WeaponItem weaponItem)
        {
            isArmed = weaponItem.isArmed;
            ChangeBackgroundColor();
        }

        private void CheckSlotIsCurrentWeapon(WeaponItem currentWeapon)
        {
            if (item == currentWeapon)
            {
                CharacterUI_WeaponPanel ch = FindObjectOfType<CharacterUI_WeaponPanel>();
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