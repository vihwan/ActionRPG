using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class InventoryWindowUI : MonoBehaviour
    {
        [Header("SpringBoard")]
        [SerializeField] private InventoryMenuSpringBoard springBoardMenu;

        [Header("Text")]
        [SerializeField] private TMP_Text userGoldText;

        [Header("Button")]
        [SerializeField] private Button closeBtn;
        [SerializeField] Button leftArrowBtn;
        [SerializeField] Button rightArrowBtn;

        [Header("Inventory UI Main Contents")]
        [SerializeField] internal InventoryMainContents mainContents;

        [Header("Need Component")]
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private PlayerSkillManager playerSkillManager;
        [SerializeField] private PlayerInventory playerInventory;
        public void Init()
        {
            inputHandler = FindObjectOfType<InputHandler>();


            springBoardMenu = GetComponentInChildren<InventoryMenuSpringBoard>();
            if (springBoardMenu != null)
                springBoardMenu.Init();

            userGoldText = UtilHelper.Find<TMP_Text>(transform, "UserGold/priceText");

            mainContents = GetComponentInChildren<InventoryMainContents>();
            if (mainContents != null)
                mainContents.Init();

            closeBtn = UtilHelper.Find<Button>(transform, "CloseBtn");
            if (closeBtn != null)
                closeBtn.onClick.AddListener(() => inputHandler.HandleMenuFlag());

            leftArrowBtn = UtilHelper.Find<Button>(transform, "LeftArrowBtn");
            if (leftArrowBtn != null)
                leftArrowBtn.onClick.AddListener(SetActiveLeftList);

            rightArrowBtn = UtilHelper.Find<Button>(transform, "RightArrowBtn");
            if (rightArrowBtn != null)
                rightArrowBtn.onClick.AddListener(SetActiveRightList);

            playerSkillManager = FindObjectOfType<PlayerSkillManager>();
            playerInventory = playerSkillManager.GetComponent<PlayerInventory>();
            playerInventory.AddUpdateGoldText(() => UpdateGoldText());
        }

        public void OnOpenPanel()
        {
            for (int i = 0; i < mainContents.ContentLists.Count; i++)
            {
                mainContents.ContentLists[i].UpdateUI();
                if (mainContents.ContentLists[i].inventoryContentSlots.Length != 0)
                    mainContents.infoPanel.SetParameter(mainContents.ContentLists[i].inventoryContentSlots[0]);
            }

            userGoldText.text = PlayerInventory.Instance.CurrentGold.ToString();
            mainContents.SetFalseAllContentList();
            springBoardMenu.ChangeSpringButtonColor(springBoardMenu.SpringMenuBtns[0]);
            mainContents.SetActiveContentList(mainContents.WeaponList, true);
            mainContents.infoPanel.SetParameter(mainContents.WeaponList.inventoryContentSlots[0]);
        }

        private void UpdateGoldText()
        {
            userGoldText.text = PlayerInventory.Instance.CurrentGold.ToString();
        }

        public void OnClosePanel()
        {
            playerSkillManager.SetPlayerHUDConsumableSlot(PlayerInventory.Instance.currentConsumableItem);
        }
        private void SetActiveLeftList()
        {
            for (int i = 0; i < mainContents.ContentLists.Count; i++)
            {
                if (mainContents.ContentLists[i].gameObject.activeSelf == true)
                {
                    int index = i;
                    mainContents.ContentLists[index].gameObject.SetActive(false);
                    springBoardMenu.ChangeSpringButtonColor(springBoardMenu.SpringMenuBtns[index]);

                    if (index - 1 < 0)
                    {
                        index = mainContents.ContentLists.Count;
                    }
                    mainContents.ContentLists[index - 1].gameObject.SetActive(true);
                    mainContents.ContentLists[index - 1].SetBeforeSelectSlotInfoPanel();
                    springBoardMenu.ChangeSpringButtonColor(springBoardMenu.SpringMenuBtns[index - 1]);
                    break;
                }
            }
        }

        private void SetActiveRightList()
        {
            for (int i = 0; i < mainContents.ContentLists.Count; i++)
            {
                if (mainContents.ContentLists[i].gameObject.activeSelf == true)
                {
                    int index = i;
                    mainContents.ContentLists[index].gameObject.SetActive(false);
                    springBoardMenu.ChangeSpringButtonColor(springBoardMenu.SpringMenuBtns[index]);

                    if (index + 1 >= mainContents.ContentLists.Count)
                    {
                        index = -1;
                    }
                    mainContents.ContentLists[index + 1].gameObject.SetActive(true);
                    mainContents.ContentLists[index + 1].SetBeforeSelectSlotInfoPanel();
                    springBoardMenu.ChangeSpringButtonColor(springBoardMenu.SpringMenuBtns[index + 1]);
                    break;
                }
            }
        }
    }
}
