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

        [Header("CharacterUI Right Panel")]
        [SerializeField] private CharacterUI_StatusPanel statusPanel;
        [SerializeField] private CharacterUI_WeaponPanel weaponPanel;
        [SerializeField] private CharacterUI_EquipmentPanel equipmentPanel;

        [Header("Button")]
        [SerializeField] private Button closeBtn;
        [SerializeField] internal Button backBtn;

        public void Init()
        {
            WindowPanel windowPanel = GetComponentInParent<WindowPanel>();

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

            closeBtn = UtilHelper.Find<Button>(transform, "CloseBtn");
            if (closeBtn != null)
                closeBtn.onClick.AddListener(windowPanel.CloseCharacterWindowPanel);

            backBtn = UtilHelper.Find<Button>(transform, "BackBtn");
            if (backBtn != null)
            {
                backBtn.onClick.AddListener(CloseLeftPanel);
                backBtn.gameObject.SetActive(false);
            }


            weaponInventoryList = GetComponentInChildren<WeaponInventoryList>(true);
            if (weaponInventoryList != null)
                weaponInventoryList.Init();
        }

/*        public void UpdateCharacterWindowUI()
        {
            statusPanel.SetParameter();
            weaponPanel.SetParameter();
        }*/


        #region CharacterMenu Right Panel SetActive

        public void OpenStatusPanel()
        {
            CloseAllRightPanel();
            statusPanel.gameObject.SetActive(true);
            //UpdateCharacterWindowUI();
        }

        public void OpenWeaponPanel()
        {
            CloseAllRightPanel();
            weaponPanel.gameObject.SetActive(true);
        }

        public void OpenEquipmentPanel()
        {
            CloseAllRightPanel();
            equipmentPanel.gameObject.SetActive(true);
        }


        public void CloseAllRightPanel()
        {
            statusPanel.gameObject.SetActive(false);
            weaponPanel.gameObject.SetActive(false);
            equipmentPanel.gameObject.SetActive(false);
        }
        #endregion

        #region

        public void CloseLeftPanel()
        {
            if(weaponInventoryList.gameObject.activeSelf == true)
            {
                weaponInventoryList.DeSelectAllSlots();
                OpenWeaponPanel();
                weaponInventoryList.gameObject.SetActive(false);
                weaponPanel.openPanelBtn.gameObject.SetActive(true);
            }
            backBtn.gameObject.SetActive(false);
        }
        #endregion
    }
}
