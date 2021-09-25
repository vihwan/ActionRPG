using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class EquipmentInventorySlot : InventorySlot
    {
        [SerializeField] internal EquipItem item;
        [SerializeField] private bool isArmed;
        [SerializeField] private Transform rarityTransform;
        [SerializeField] private List<GameObject> rareStars;

        [Header("Prefab")]
        [SerializeField] private GameObject RareStar;

        [Header("Enforce Image")]
        [SerializeField] private Image enforceImage;
        [SerializeField] private TMP_Text enforceText;

        private EquipmentInventoryList equipmentInventoryList;
        private CharacterUI_EquipmentPanel equipPanel;
        private void Awake()
        {
            equipPanel = FindObjectOfType<CharacterUI_EquipmentPanel>();

            itemBtn = GetComponentInChildren<Button>();
            if (itemBtn != null)
            {
                itemBtn.onClick.AddListener(() => equipPanel.SetParameterIndividualEquipItem(item));
                itemBtn.onClick.AddListener(SelectSlot);
            }
            icon = UtilHelper.Find<Image>(itemBtn.transform, "ItemIcon");
            rarityTransform = itemBtn.transform.Find("RarityTransform").transform;
            RareStar = Resources.Load<Image>("Prefabs/RarityStar").gameObject;

            enforceImage = UtilHelper.Find<Image>(itemBtn.transform, "EnforceImage");
            enforceText = UtilHelper.Find<TMP_Text>(enforceImage.transform, "EnforceText");

            equipmentInventoryList = GetComponentInParent<EquipmentInventoryList>();
        }

        public void AddItem(EquipItem equipItem)
        {
            gameObject.SetActive(true);
            item = equipItem;
            icon.sprite = equipItem.itemIcon;
            icon.enabled = true;
            CreateRarityStar(rarityTransform, rareStars, item);
            isArmed = equipItem.isArmed;

            enforceImage.gameObject.SetActive(true);
            enforceText.text = equipItem.enforceLevel.ToString();
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


        public void ClearInventorySlot()
        {
            item = null;
            icon.enabled = false;
            icon.sprite = null;
            isArmed = false;
            ChangeBackgroundColor();
            gameObject.SetActive(false);
        }
        public void UpdateSlotIsArmed(EquipItem equipItem)
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
