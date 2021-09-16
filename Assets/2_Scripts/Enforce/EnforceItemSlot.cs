using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class EnforceItemSlot : InventorySlot
    {
        [SerializeField] private Item item;
        [SerializeField] public bool isArmed;
        [SerializeField] private Image enforceImage;
        [SerializeField] private TMP_Text enforceText;
        [SerializeField] private Image restrictImage;
        [SerializeField] private TMP_Text restrictText;
        [SerializeField] private Transform rarityTransform;
        [SerializeField] private List<GameObject> rareStars;

        [Header("Prefab")]
        [SerializeField] private GameObject RareStar;

        private readonly string isArmString = "장착중";
        private readonly string isMaxEnforceString = "최고 레벨";

        private Action btnAction;
        public Item Item
        {
            get => item;
            private set
            {
                item = value;
                if (item != null)
                {
                    if (item.itemType == ItemType.Weapon)
                    {
                        isArmed = (item as WeaponItem).isArmed;
                        enforceImage.gameObject.SetActive(true);
                        enforceText.text = (item as WeaponItem).enforceLevel.ToString();

                    }
                    else
                    {
                        isArmed = (item as EquipItem).isArmed;
                        enforceImage.gameObject.SetActive(true);
                        enforceText.text = (item as EquipItem).enforceLevel.ToString();
                    }
                }
            }
        }
        public bool IsSelect
        {
            get => isSelect;
            private set
            {
                isSelect = value;
                ChangeBackgroundColor();
            }
        }
        private void Awake()
        {
            itemBtn = GetComponentInChildren<Button>();
            if (itemBtn != null)
            {
                CharacterUI_WeaponPanel weaponPanel = FindObjectOfType<CharacterUI_WeaponPanel>();
                itemBtn.onClick.AddListener(OnClickBtn);
            }
            icon = UtilHelper.Find<Image>(itemBtn.transform, "ItemIcon");
            enforceImage = UtilHelper.Find<Image>(itemBtn.transform, "EnforceImage");
            enforceText = UtilHelper.Find<TMP_Text>(enforceImage.transform, "EnforceText");
            restrictImage = UtilHelper.Find<Image>(itemBtn.transform, "RestrictImage");
            restrictText = UtilHelper.Find<TMP_Text>(restrictImage.transform, "RestrictText");
            rarityTransform = itemBtn.transform.Find("RarityTransform").transform;
            RareStar = Resources.Load<Image>("Prefabs/RarityStar").gameObject;
        }
        private void OnClickBtn()
        {
            btnAction?.Invoke();
        }
        public void SetBtnListener(Action action)
        {
            btnAction = action;
        }
        public void SetIsSelectSlot(bool status)
        {
            IsSelect = status;
        }
        public void SetEnforceItemSlot(Item item)
        {
            gameObject.SetActive(true);
            this.Item = item;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            CreateRarityStar(rarityTransform, rareStars, item);
            ChangeBackgroundColor();
            CheckRestrictItem();
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
        private void CheckRestrictItem()
        {
            if (isArmed.Equals(true))
            {
                restrictImage.gameObject.SetActive(true);
                restrictText.text = isArmString;
                return;
            }
            else
            {
                restrictImage.gameObject.SetActive(false);
                restrictText.text = null;
            }

            if (item.itemType == ItemType.Weapon)
            {
                if ((item as WeaponItem).enforceLevel == 5)
                {
                    restrictImage.gameObject.SetActive(true);
                    restrictText.text = isMaxEnforceString;
                }
                else
                {
                    restrictImage.gameObject.SetActive(false);
                    restrictText.text = null;
                }
            }
            else
            {
                if ((item as EquipItem).enforceLevel == 5)
                {
                    restrictImage.gameObject.SetActive(true);
                    restrictText.text = isMaxEnforceString;
                    return;
                }
                else
                {
                    restrictImage.gameObject.SetActive(false);
                    restrictText.text = null;
                }
            }
        }

        public void ClearEnforceItemSlot()
        {
            Item = null;
            icon.sprite = null;
            icon.enabled = false;
            isArmed = false;
            IsSelect = false;
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
