using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SG
{
    public class CharacterWindowUI : MonoBehaviour
    {
        [Header("SpringBoard")]
        [SerializeField] private CharacterMenuSpringBoard springBoardMenu;

        [Header("CharacterUI Left Panel")]
        [SerializeField] internal WeaponInventoryList weaponInventoryList;
        [SerializeField] internal EquipmentInventoryList equipmentInventoryList;

        [Header("CharacterUI Right Panel")]
        [SerializeField] private CharacterUI_StatusPanel statusPanel;
        [SerializeField] private CharacterUI_WeaponPanel weaponPanel;
        [SerializeField] private CharacterUI_EquipmentPanel equipmentPanel;
        [SerializeField] internal CharacterUI_SkillPanel skillPanel;
        [SerializeField] private CharacterUI_DataPanel dataPanel;

        [Header("Button")]
        [SerializeField] private Button closeBtn;

        [SerializeField] internal Button backBtn;

        [SerializeField] private PlayerSkillManager playerSkillManager;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private AnimationLayerHandler animationLayerHandler;

        public void Init()
        {
            WindowPanel windowPanel = GetComponentInParent<WindowPanel>();

            playerSkillManager = FindObjectOfType<PlayerSkillManager>();
            inputHandler = playerSkillManager.gameObject.GetComponent<InputHandler>();
            animationLayerHandler = FindObjectOfType<AnimationLayerHandler>();

            springBoardMenu = UtilHelper.Find<CharacterMenuSpringBoard>(transform,"SpringBoardMenu");
            if (springBoardMenu != null)
                springBoardMenu.Init();

            statusPanel = GetComponentInChildren<CharacterUI_StatusPanel>();
            if (statusPanel != null)
                statusPanel.Init();

            weaponPanel = GetComponentInChildren<CharacterUI_WeaponPanel>();
            if (weaponPanel != null)
                weaponPanel.Init();

            equipmentPanel = GetComponentInChildren<CharacterUI_EquipmentPanel>();
            if (equipmentPanel != null)
                equipmentPanel.Init();

            skillPanel = GetComponentInChildren<CharacterUI_SkillPanel>();
            if (skillPanel != null)
                skillPanel.Init();

            dataPanel = GetComponentInChildren<CharacterUI_DataPanel>();
            if (dataPanel != null)
                dataPanel.Init();


            closeBtn = UtilHelper.Find<Button>(transform, "CloseBtn");
            if (closeBtn != null)
            {
                closeBtn.onClick.AddListener(() => inputHandler.HandleMenuFlag());
                closeBtn.onClick.AddListener(() => animationLayerHandler.SetAnimaionLayerWeightCloseCharacterPanel());
            }
                
            backBtn = UtilHelper.Find<Button>(transform, "BackBtn");
            if (backBtn != null)
            {
                backBtn.onClick.AddListener(CloseLeftPanel);
                backBtn.gameObject.SetActive(false);
            }

            weaponInventoryList = GetComponentInChildren<WeaponInventoryList>(true);
            if (weaponInventoryList != null)
                weaponInventoryList.Init();

            equipmentInventoryList = GetComponentInChildren<EquipmentInventoryList>(true);
            if (equipmentInventoryList != null)
                equipmentInventoryList.Init();
        }

        private void OnDisable()
        {
            playerSkillManager.SetPlayerHUDSkillSlot(skillPanel.currentSkillList);
        }


        #region Right Panel SetActive

        public void OpenStatusPanel()
        {
            CloseAllRightPanel();
            statusPanel.gameObject.SetActive(true);
            statusPanel.OnOpenPanel();
            springBoardMenu.OnClickBtnChangeColor(springBoardMenu.statusBtn);           
            animationLayerHandler.HandlePlayerOnWeaponEquipPanelAnimation(true);
            animationLayerHandler.SetPlayerAnimationIsUnWeaponOnWeaponEquipPanel();
            
        }

        public void OpenWeaponPanel()
        {
            CloseAllRightPanel();
            weaponPanel.gameObject.SetActive(true);
            weaponPanel.OnOpenPanel();
            animationLayerHandler.HandlePlayerOnWeaponEquipPanelAnimation(true);
            animationLayerHandler.SetPlayerAnimationIsWeaponOnWeaponEquipPanel();
        }

        public void OpenEquipmentPanel()
        {
            CloseAllRightPanel();
            equipmentPanel.gameObject.SetActive(true);
            equipmentPanel.OnOpenPanel();
            animationLayerHandler.HandlePlayerOnWeaponEquipPanelAnimation(true);
            animationLayerHandler.SetPlayerAnimationIsWeaponOnWeaponEquipPanel();
        }

        public void OpenSkillPanel()
        {
            CloseAllRightPanel();
            skillPanel.gameObject.SetActive(true);
            skillPanel.OnOpenPanel();
            animationLayerHandler.HandlePlayerOnWeaponEquipPanelAnimation(true);
            animationLayerHandler.SetPlayerAnimationIsUnWeaponOnWeaponEquipPanel();
        }

        public void OpenDataPanel()
        {
            CloseAllRightPanel();
            dataPanel.gameObject.SetActive(true);
            dataPanel.OnOpenPanel();
            animationLayerHandler.HandlePlayerOnWeaponEquipPanelAnimation(true);
            animationLayerHandler.SetPlayerAnimationIsUnWeaponOnWeaponEquipPanel();
        }
        public void CloseAllRightPanel()
        {
            statusPanel.gameObject.SetActive(false);
            weaponPanel.gameObject.SetActive(false);
            equipmentPanel.gameObject.SetActive(false);
            skillPanel.gameObject.SetActive(false);
            dataPanel.gameObject.SetActive(false);
        }
        #endregion

        #region Close Left Panel

        public void CloseLeftPanel()
        {
            if(weaponInventoryList.gameObject.activeSelf == true)
            {
                weaponInventoryList.SetAllSlotsDeselect();
                //weaponPanel.playerInventory.SortWeaponInventory(weaponPanel.playerInventory.currentWeapon);
                OpenWeaponPanel();
                weaponInventoryList.gameObject.SetActive(false);
                weaponPanel.openPanelBtn.gameObject.SetActive(true);

                if (weaponPanel.comparisonPanel.activeSelf == true)
                    weaponPanel.comparisonPanel.SetActive(false);
            }

            if(equipmentInventoryList.gameObject.activeSelf == true)
            {
                equipmentInventoryList.SetAllSlotsDeselect();
                equipmentInventoryList.gameObject.SetActive(false);
                equipmentPanel.CloseLeftEquipmentInventory();
                equipmentPanel.openLeftInventoryBtn.gameObject.SetActive(true);

                if (equipmentPanel.comparisonPanel.activeSelf == true)
                    equipmentPanel.comparisonPanel.SetActive(false);

                return;
            }

            if(equipmentPanel.individualPanel.activeSelf == true)
            {
                foreach (EquipSlot slot in equipmentPanel.equipSlots)
                {
                    slot.ChangeFrameColor(false);
                }
                equipmentPanel.individualPanel.SetActive(false);
                equipmentPanel.UpdateMainPanel();
            }

            backBtn.gameObject.SetActive(false);
        }
        #endregion
    }
}
