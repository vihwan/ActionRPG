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

        [Header("CharacterUI Panel")]
        [SerializeField] private CharacterUI_StatusPanel statusPanel;
        [SerializeField] private CharacterUI_WeaponPanel weaponPanel;
        [SerializeField] private CharacterUI_EquipmentPanel equipmentPanel;

        [Header("Button")]
        [SerializeField] private Button closeBtn;
        [SerializeField] private Button backBtn;

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
            equipmentPanel = GetComponentInChildren<CharacterUI_EquipmentPanel>();

            closeBtn = UtilHelper.Find<Button>(transform, "CloseBtn");
            if (closeBtn != null)
                closeBtn.onClick.AddListener(windowPanel.CloseCharacterWindowPanel);

            backBtn = UtilHelper.Find<Button>(transform, "BackBtn");
            if (backBtn != null)
                backBtn.gameObject.SetActive(false);
        }


        #region CharacterMenu Right Panel SetActive

        public void OpenStatusPanel()
        {
            CloseAllRightPanel();
            statusPanel.gameObject.SetActive(true);
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
    }
}
