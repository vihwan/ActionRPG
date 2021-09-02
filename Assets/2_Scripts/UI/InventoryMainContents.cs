using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class InventoryMainContents : MonoBehaviour
    {
        [Header("Lists")]
        [SerializeField] private List<InventoryMain_ContentList> contentLists;
        private InventoryMain_ContentList weaponList;
        private InventoryMain_ContentList topsList;
        private InventoryMain_ContentList bottomsList;
        private InventoryMain_ContentList glovesList;
        private InventoryMain_ContentList shoesList;
        private InventoryMain_ContentList accessoryList;
        private InventoryMain_ContentList specialEquipList;
        private InventoryMain_ContentList consumableList;
        private InventoryMain_ContentList ingredientList;

        [Header("Info")]
        internal InventoryMain_InfoPanel infoPanel;

        [Header("Text")]
        [SerializeField] private TMP_Text noneText;

        [Header("Need Component"), HideInInspector]
        internal PlayerInventory playerInventory;
        public List<InventoryMain_ContentList> ContentLists { get => contentLists; private set => contentLists = value; }
        public InventoryMain_ContentList WeaponList { get => weaponList; private set => weaponList = value; }
        public InventoryMain_ContentList TopsList { get => topsList; private set => topsList = value; }
        public InventoryMain_ContentList BottomsList { get => bottomsList; private set => bottomsList = value; }
        public InventoryMain_ContentList GlovesList { get => glovesList; private set => glovesList = value; }
        public InventoryMain_ContentList ShoesList { get => shoesList; private set => shoesList = value; }
        public InventoryMain_ContentList AccessoryList { get => accessoryList; private set => accessoryList = value; }
        public InventoryMain_ContentList SpecialEquipList { get => specialEquipList; private set => specialEquipList = value; }
        public InventoryMain_ContentList ConsumableList { get => consumableList; private set => consumableList = value; }
        public InventoryMain_ContentList IngredientList { get => ingredientList; private set => ingredientList = value; }
        public TMP_Text NoneText { get => noneText; private set => noneText = value; }

        public void Init()
        {
            weaponList = UtilHelper.Find<InventoryMain_ContentList>(transform, "WeaponList");
            if (weaponList != null)
            {
                weaponList.Init();
                contentLists.Add(weaponList);
            }


            topsList = UtilHelper.Find<InventoryMain_ContentList>(transform, "TopsList");
            if (topsList != null)
            {
                topsList.Init();
                contentLists.Add(topsList);
            }


            bottomsList = UtilHelper.Find<InventoryMain_ContentList>(transform, "BottomsList");
            if (bottomsList != null)
            {
                bottomsList.Init();
                contentLists.Add(bottomsList);
            }


            glovesList = UtilHelper.Find<InventoryMain_ContentList>(transform, "GlovesList");
            if (glovesList != null)
            {
                glovesList.Init();
                contentLists.Add(glovesList);
            }

            shoesList = UtilHelper.Find<InventoryMain_ContentList>(transform, "ShoesList");
            if (shoesList != null)
            {
                shoesList.Init();
                contentLists.Add(shoesList);
            }


            accessoryList = UtilHelper.Find<InventoryMain_ContentList>(transform, "AccessoryList");
            if (accessoryList != null)
            {
                accessoryList.Init();
                contentLists.Add(accessoryList);
            }


            specialEquipList = UtilHelper.Find<InventoryMain_ContentList>(transform, "SpecialEquipList");
            if (specialEquipList != null)
            {
                specialEquipList.Init();
                contentLists.Add(specialEquipList);
            }


            consumableList = UtilHelper.Find<InventoryMain_ContentList>(transform, "ConsumableList");
            if (consumableList != null)
            {
                consumableList.Init();
                contentLists.Add(consumableList);
            }


            ingredientList = UtilHelper.Find<InventoryMain_ContentList>(transform, "IngredientList");
            if (ingredientList != null)
            {
                ingredientList.Init();
                contentLists.Add(ingredientList);
            }

            infoPanel = GetComponentInChildren<InventoryMain_InfoPanel>(true);
            if (infoPanel != null)
                infoPanel.Init();


            noneText = UtilHelper.Find<TMP_Text>(transform, "NoneText");
            if (noneText != null)
                noneText.gameObject.SetActive(false);

            playerInventory = FindObjectOfType<PlayerInventory>();
        }

        public void SetActiveContentList(InventoryMain_ContentList list, bool state)
        {
            SetFalseAllContentList();
            list.gameObject.SetActive(state);
            list.SetBeforeSelectSlotInfoPanel();
        }

        public void SetFalseAllContentList()
        {
            for (int i = 0; i < contentLists.Count; i++)
            {
                contentLists[i].gameObject.SetActive(false);
            }
        }
        public void SetNoneText(bool state)
        {
            noneText.gameObject.SetActive(state);
            infoPanel.gameObject.SetActive(!state);
            if (state == true)
            {
                for (int i = 0; i < contentLists.Count; i++)
                {
                    Image contentImg = contentLists[i].GetComponent<Image>();
                    contentImg.color = new Color(contentImg.color.r, contentImg.color.g, contentImg.color.b, 0f);
                }
            }
            else
            {
                for (int i = 0; i < contentLists.Count; i++)
                {
                    Image contentImg = contentLists[i].GetComponent<Image>();
                    contentImg.color = new Color(contentImg.color.r, contentImg.color.g, contentImg.color.b, 1f);
                }
            }
        }
    }
}