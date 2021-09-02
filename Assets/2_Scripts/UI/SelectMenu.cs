using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace SG { 
    public class SelectMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        [SerializeField] private Button characterButton;
        [SerializeField] private Button inventoryButton;
        [SerializeField] private Button worldmapButton;
        [SerializeField] private Button questButton;
        [SerializeField] private Button achieveButton;
        [SerializeField] private Button screenshotButton;
        [SerializeField] private Button optionButton;
        [SerializeField] private Button exitButton;

        [SerializeField] private TMP_Text guideText;

        private GUIManager guiManager;

        // Start is called before the first frame update
        public void Init()
        {
            guiManager = GetComponentInParent<GUIManager>();
            characterButton = UtilHelper.Find<Button>(transform, "Select Character");
            if (characterButton != null)
            {
                characterButton.onClick.AddListener(guiManager.windowPanel.OpenCharacterWindowPanel);
                characterButton.onClick.AddListener(guiManager.CloseSelectMenuWindow);
            }

            inventoryButton = UtilHelper.Find<Button>(transform, "Select Inventory");    
            if (inventoryButton != null)
            {
                inventoryButton.onClick.AddListener(guiManager.windowPanel.OpenInventoryWindowPanel);
                inventoryButton.onClick.AddListener(guiManager.CloseSelectMenuWindow);
            }

            worldmapButton = UtilHelper.Find<Button>(transform, "Select WorldMap");
            questButton = UtilHelper.Find<Button>(transform, "Select Quest");
            achieveButton = UtilHelper.Find<Button>(transform, "Select Achivement");
            screenshotButton = UtilHelper.Find<Button>(transform, "Select Screenshot");
            optionButton = UtilHelper.Find<Button>(transform, "Select GameOptions");
            exitButton = UtilHelper.Find<Button>(transform, "Select Exit");

            guideText = GetComponentInChildren<TMP_Text>(true);
            if (guideText != null)
                guideText.text = "Now Ready";
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            string enterObject = eventData.pointerCurrentRaycast.gameObject.name;
            guideText.text = enterObject;
            if (enterObject == "GuideText")
                guideText.text = null;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject == null)
                guideText.text = null;
        }
    }
}

