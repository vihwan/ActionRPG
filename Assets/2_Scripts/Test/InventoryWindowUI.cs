using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class InventoryWindowUI : MonoBehaviour
    {
        [Header("SpringBoard")]
        [SerializeField] private InventoryMenuSpringBoard springBoardMenu;

        [Header("Button")]
        [SerializeField] private Button closeBtn;

        [Header("Inventory UI Main Contents")]
        [SerializeField] internal InventoryMainContents mainContents;


        [Header("Button")]
        [SerializeField] Button leftBtn;
        [SerializeField] Button rightBtn;


        [SerializeField] private InputHandler inputHandler;
        public void Init()
        {
            inputHandler = FindObjectOfType<InputHandler>();

            springBoardMenu = GetComponentInChildren<InventoryMenuSpringBoard>();
            if (springBoardMenu != null)
                springBoardMenu.Init();

            mainContents = GetComponentInChildren<InventoryMainContents>();
            if (mainContents != null)
                mainContents.Init();

            closeBtn = GetComponentInChildren<Button>();
            if (closeBtn != null)
                closeBtn.onClick.AddListener(() => inputHandler.HandleMenuFlag());


            leftBtn = UtilHelper.Find<Button>(transform, "LeftArrowBtn");
            if (leftBtn != null)
                leftBtn.onClick.AddListener(SetActiveLeftList);

            rightBtn = UtilHelper.Find<Button>(transform, "RightArrowBtn");
            if (rightBtn != null)
                rightBtn.onClick.AddListener(SetActiveRightList);
        }

        private void OnEnable()
        {
            for (int i = 0; i < mainContents.ContentLists.Count; i++)
            {
                mainContents.ContentLists[i].UpdateList();
                mainContents.infoPanel.SetParameter(mainContents.ContentLists[i].inventoryContentSlots[0]);
            }

            mainContents.SetFalseAllContentList();
            springBoardMenu.ChangeSpringButtonColor(springBoardMenu.SpringMenuBtns[0]);
            mainContents.SetActiveContentList(mainContents.WeaponList, true);
            mainContents.infoPanel.SetParameter(mainContents.WeaponList.inventoryContentSlots[0]);
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
