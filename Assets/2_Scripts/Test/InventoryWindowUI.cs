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
        [SerializeField] private InventoryMainContents mainContents;

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
        }
    }
}
