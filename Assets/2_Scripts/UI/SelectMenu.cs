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

        // Start is called before the first frame update
        public void Init()
        {
            characterButton = UtilHelper.Find<Button>(transform, "Select Character");
            if (characterButton != null)
            {
                characterButton.onClick.AddListener(GUIManager.instance.windowPanel.OpenCharacterWindowPanel);
                characterButton.onClick.AddListener(GUIManager.instance.CloseSelectMenuWindow);
            }

            inventoryButton = UtilHelper.Find<Button>(transform, "Select Inventory");    
            if (inventoryButton != null)
            {
                inventoryButton.onClick.AddListener(GUIManager.instance.windowPanel.OpenInventoryWindowPanel);
                inventoryButton.onClick.AddListener(GUIManager.instance.CloseSelectMenuWindow);
            }

            worldmapButton = UtilHelper.Find<Button>(transform, "Select WorldMap");
            questButton = UtilHelper.Find<Button>(transform, "Select Quest");
            if(questButton != null)
            {
                questButton.onClick.AddListener(GUIManager.instance.questPanel.OpenQuestPanelEvent);
                questButton.onClick.AddListener(GUIManager.instance.CloseSelectMenuWindow);
            }
            achieveButton = UtilHelper.Find<Button>(transform, "Select Achivement");
            if(achieveButton != null)
            {
                achieveButton.onClick.AddListener(GUIManager.instance.achievePanel.OpenAchievePanelEvent);
                achieveButton.onClick.AddListener(GUIManager.instance.CloseSelectMenuWindow);
            }
            screenshotButton = UtilHelper.Find<Button>(transform, "Select Screenshot");
            optionButton = UtilHelper.Find<Button>(transform, "Select GameOptions");
            exitButton = UtilHelper.Find<Button>(transform, "Select Exit");

            guideText = GetComponentInChildren<TMP_Text>(true);
            if (guideText != null)
                guideText.text = "";
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            string enterObject = eventData.pointerCurrentRaycast.gameObject.name;
            switch (enterObject)
            {          
                case "Select Character":   guideText.text = "캐릭터 정보"; break;
                case "Select Inventory":   guideText.text = "인벤토리"; break;
                case "Select WorldMap":    guideText.text = "월드맵"; break;
                case "Select Quest":       guideText.text = "퀘스트"; break;
                case "Select Achivement":  guideText.text = "업적"; break;
                case "Select Screenshot":  guideText.text = "스크린샷"; break;
                case "Select GameOptions": guideText.text = "환경설정"; break;
                case "Select Exit":        guideText.text = "게임 종료"; break;
            }
            if (enterObject.Equals("GuideText"))
                guideText.text = null;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject == null)
                guideText.text = null;
        }
    }
}

