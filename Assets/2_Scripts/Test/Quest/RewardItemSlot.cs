using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace SG
{
    public class RewardItemSlot : InventorySlot, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text quantityText;
        [SerializeField] private Transform rarityTransform;
        [SerializeField] private List<GameObject> rareStars;

        [Header("Prefab")]
        [SerializeField] private GameObject RareStar;

        [Header("Enforce Image")]
        [SerializeField] private Image enforceImage;
        [SerializeField] private TMP_Text enforceText;

        [Header("Reference Item")]
        [SerializeField] internal Item item;

        private QuestPanel questPanel;
        public void Init()
        {
            itemBtn = GetComponentInChildren<Button>();
            if (itemBtn != null)
            {
                itemBtn.interactable = false;
            }
            icon = UtilHelper.Find<Image>(itemBtn.transform, "ItemIcon");
            quantityText = UtilHelper.Find<TMP_Text>(itemBtn.transform, "QuantityText");
            rarityTransform = itemBtn.transform.Find("RarityTransform").transform;
            RareStar = Resources.Load<Image>("Prefabs/RarityStar").gameObject;

            enforceImage = UtilHelper.Find<Image>(itemBtn.transform, "EnforceImage");
            enforceText = UtilHelper.Find<TMP_Text>(enforceImage.transform, "EnforceText");

            questPanel = GetComponentInParent<QuestPanel>();
        }
        public void AddItem(Item item, int itemCount)
        {
            this.item = item;
            icon.sprite = item.itemIcon;
            quantityText.text = itemCount.ToString();

            if (item.GetType().Equals(typeof(ConsumableItem)) ||
                item.GetType().Equals(typeof(IngredientItem))
                )
            {
                quantityText.gameObject.SetActive(true);
                enforceImage.gameObject.SetActive(false);
                enforceText.text = "0";
            }
            else
            {
                quantityText.gameObject.SetActive(false);
                enforceImage.gameObject.SetActive(true);
                enforceText.text = "0";
            }
            CreateRarityStar(rarityTransform, rareStars, item);
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
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (questPanel.infoPanel.gameObject.activeSelf.Equals(true))
            {
                return;
            }

            questPanel.infoPanel.SetParameter(this);
            questPanel.infoPanel.transform.position = this.transform.position +
                new Vector3(questPanel.infoPanel.GetComponent<RectTransform>().sizeDelta.x * 0.5f,
                            questPanel.infoPanel.GetComponent<RectTransform>().sizeDelta.y * 0.5f,
                            0f);
            questPanel.infoPanel.gameObject.SetActive(true);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            questPanel.infoPanel.gameObject.SetActive(false);
        }
    }
}
