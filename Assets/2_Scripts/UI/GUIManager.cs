using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{

    //전체적인 UI 요소들을 가져와 컨트롤하는 스크립트. GUI의 최상단 매니저
    //각 UI 요소들을 참조하여 초기화하고 각 요소의 활성/비활성을 담당합니다.
    public class GUIManager : MonoBehaviour
    {
        public static GUIManager instance; //Singleton

        [Header("HUD Windows")]
        [SerializeField] private GameObject hudWindows;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private ManaBar manaBar;
        [SerializeField] private StaminaBar staminaBar;
        [SerializeField] internal LevelWindow levelWindow;
        [SerializeField] internal QuickSlotUI quickSlotUI;
        [SerializeField] private InteractableUI interactableUI;
        [SerializeField] internal QuestAlertUI questAlertUI;
        [SerializeField] internal LevelUpAlertUI levelUpAlertUI;
        [SerializeField] internal EnemyBossHealthBarUI enemyBossHealthBarUI;

        [Header("UI Windows")]
        [SerializeField] private SelectMenu selectMenu;
        [SerializeField] internal WindowPanel windowPanel;
        [SerializeField] internal ShopPanel shopPanel;
        [SerializeField] internal QuestPanel questPanel;
        [SerializeField] internal AchievePanel achievePanel;
        [SerializeField] public GameObject dialogObject;
        [SerializeField] internal LootWindowUI lootWindow;

        [Header("PopUp Generator")]
        [SerializeField] internal PopUpGenerator popUpGenerator;


        // Start is called before the first frame update
        public void Init()
        {
            if (instance == null)
                instance = this;

            hudWindows = transform.Find("PlayerUI/HUD").gameObject;
            if (hudWindows == null)
                Debug.LogWarning("HUD 가 참조되지 않았습니다");

            healthBar = GetComponentInChildren<HealthBar>(true);
            if (healthBar != null)
                healthBar.Init();

            manaBar = GetComponentInChildren<ManaBar>(true);
            if (manaBar != null)
                manaBar.Init();

            staminaBar = GetComponentInChildren<StaminaBar>(true);
            if (staminaBar != null)
                staminaBar.Init();

            levelWindow = GetComponentInChildren<LevelWindow>(true);
            if (levelWindow != null)
                levelWindow.Init();

            quickSlotUI = GetComponentInChildren<QuickSlotUI>(true);
            if (quickSlotUI != null)
                quickSlotUI.Init();

            interactableUI = GetComponentInChildren<InteractableUI>(true);
            if (interactableUI != null)
                interactableUI.Init();

            questAlertUI = GetComponentInChildren<QuestAlertUI>(true);
            if (questAlertUI != null)
                questAlertUI.Init();

            levelUpAlertUI = GetComponentInChildren<LevelUpAlertUI>(true);
            if (levelUpAlertUI != null)
                levelUpAlertUI.Init();

            enemyBossHealthBarUI = GetComponentInChildren<EnemyBossHealthBarUI>(true);
            if(enemyBossHealthBarUI != null)
                enemyBossHealthBarUI.Init();

            windowPanel = GetComponentInChildren<WindowPanel>(true);
            if (windowPanel != null)
                windowPanel.Init();

            shopPanel = GetComponentInChildren<ShopPanel>(true);
            if (shopPanel != null)
                shopPanel.Init();

            questPanel = GetComponentInChildren<QuestPanel>(true);
            if (questPanel != null)
                questPanel.Init();

            achievePanel = GetComponentInChildren<AchievePanel>(true);
            if (achievePanel != null)
                achievePanel.Init();

            lootWindow = GetComponentInChildren<LootWindowUI>(true);
            if (lootWindow != null)
                lootWindow.Init();

            popUpGenerator = GetComponent<PopUpGenerator>();
            if (popUpGenerator != null)
                popUpGenerator.Init();

            selectMenu = GetComponentInChildren<SelectMenu>(true);
            if (selectMenu != null)
                selectMenu.Init();

            CloseSelectMenuWindow();
        }

        public bool IsActiveUIWindows()
        {
            if (selectMenu.gameObject.activeSelf.Equals(true) ||
               windowPanel.characterWindowUI.gameObject.activeSelf.Equals(true) ||
               windowPanel.inventoryWindowUI.gameObject.activeSelf.Equals(true) ||
               windowPanel.enforceWindowUI.gameObject.activeSelf.Equals(true) ||
               shopPanel.gameObject.activeSelf.Equals(true) ||
               lootWindow.gameObject.activeSelf.Equals(true) ||
               questPanel.gameObject.activeSelf.Equals(true) ||
               achievePanel.gameObject.activeSelf.Equals(true)
               )
            {
                return true;
            }
            else
            {
                return false;
            }
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
                windowPanel.CloseInventoryWindowPanel();
                windowPanel.CloseEnforceWindowPanel();
                shopPanel.CloseShopPanel();
                questPanel.CloseQuestPanelGameObject();
                achievePanel.CloseAchievePanel();
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

