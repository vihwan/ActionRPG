using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{

    //전체적인 UI 요소들을 가져와 컨트롤하는 스크립트. GUI의 최상단 매니저
    //각 UI 요소들을 참조하여 초기화하고 각 요소의 활성/비활성을 담당합니다.
    public class GUIManager : MonoBehaviour
    {
        [Header("HUD Windows")]
        [SerializeField] private GameObject hudWindows;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private QuickSlotUI quickSlotUI;
        [SerializeField] private InteractableUI interactableUI;

        [Header("UI Windows")]
        [SerializeField] private SelectMenu selectMenu;
        [SerializeField] internal WindowPanel windowPanel;


        // Start is called before the first frame update
        void Awake()
        {
            hudWindows = transform.Find("PlayerUI/HUD").gameObject;
            if (hudWindows == null)
                Debug.LogWarning("HUD 가 참조되지 않았습니다");

            healthBar = GetComponentInChildren<HealthBar>(true);
            if (healthBar != null)
                healthBar.Init();

            quickSlotUI = GetComponentInChildren<QuickSlotUI>(true);
            if(quickSlotUI != null)
                quickSlotUI.Init();

            interactableUI = GetComponentInChildren<InteractableUI>(true);
            if (interactableUI != null)
                interactableUI.Init();

            windowPanel = GetComponentInChildren<WindowPanel>(true);
            if (windowPanel != null)
                windowPanel.Init();

            selectMenu = GetComponentInChildren<SelectMenu>(true);
            if (selectMenu != null)
                selectMenu.Init();

            CloseSelectMenuWindow();
        }


        public void SetActiveGUIMenu(bool state)
        {
            SetActiveHudWindows(!state);

            if (state)
            {
                OpenSelectMenuWindow();
            }
            else
            {
                CloseSelectMenuWindow();
                windowPanel.CloseCharacterWindowPanel();
            }
        }

        public void SetActiveHudWindows(bool status)
        {
            hudWindows.gameObject.SetActive(status);
        }

        #region SelectMenu Controls

        public void OpenSelectMenuWindow()
        {
            selectMenu.gameObject.SetActive(true);
        }

        public void CloseSelectMenuWindow()
        {
            selectMenu.gameObject.SetActive(false);
        }

        #endregion
    }
}

