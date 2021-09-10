using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class QuestPanel : MonoBehaviour
    {

        [SerializeField] private Button closeBtn;

        [Header("Quest List")]
        private List<Quest> questPanelList = new List<Quest>();

        [Header("GameObjects")]
        public GameObject detailsObject;
        public TMP_Text detailsDescriptionText;
        public TMP_Text rewardExpGoldText;

        [Header("Transforms")]
        [SerializeField] private Transform questListTransform;
        [SerializeField] private Transform objectiveDisplayTransform;
        [SerializeField] private Transform rewardListTransform;

        [Header("Prefabs")]
        [SerializeField] public QuestDisplay questDisplayPrefab;
        [SerializeField] public ObjectiveDisplay objectiveDisplayPrefab;
        [SerializeField] public InventoryContentSlot inventoryContentSlotPrefab;

        private InputHandler inputHandler;
        public void Init()
        {
            inputHandler = FindObjectOfType<InputHandler>();

            Transform t = transform.Find("Background");
            if (questListTransform == null)
                questListTransform = transform.Find("List Window/QuestList").transform;

            if (objectiveDisplayTransform == null)
                objectiveDisplayTransform = transform.Find("Details/Top/DetailsList/QuestDetail").transform;

            if (rewardListTransform == null)
                rewardListTransform = transform.Find("Details/Bottom/RewardList").transform;

            closeBtn = UtilHelper.Find<Button>(t, "Header/CloseBtn");
            if (closeBtn != null)
                closeBtn.onClick.AddListener(() => inputHandler.HandleMenuFlag());

            detailsObject = t.Find("Details").gameObject;

            if (detailsDescriptionText == null)
                detailsDescriptionText = UtilHelper.Find<TMP_Text>(detailsObject.transform, "Top/DetailsList/QuestDetail/Description");

            if (rewardExpGoldText == null)
                rewardExpGoldText = UtilHelper.Find<TMP_Text>(t, "Details/Bottom/RewardExpGoldText");

            CloseQuestPanel();
        }

        public void OpenQuestPanelEvent()
        {
            SetQuestPanel();
            OpenQuestPanel();
            detailsObject.SetActive(false);
        }

        public void OpenQuestPanel()
        {
            this.gameObject.SetActive(true);
        }

        public void CloseQuestPanel()
        {
            this.gameObject.SetActive(false);
        }

        private void SetQuestPanel()
        {
            //플레이어가 소유중인 퀘스트의 수만큼 QuestDisplay를 생성
            for (int i = 0; i < PlayerQuestInventory.Instance.Quests.Count; i++)
            {
                Quest quest = PlayerQuestInventory.Instance.Quests[i];
                QuestDisplay qd = Instantiate(questDisplayPrefab, questListTransform) as QuestDisplay;
                qd.Initialize();
                qd.SetQuestDisplay(quest);
                qd.AddClickBtnAction(() => QuestPanelAction(quest));
            }
        }

        private void QuestPanelAction(Quest quest)
        {
            SetObjectiveDisplay(quest);
            SetRewardListDisplay(quest);
            OpenDetails();
        }

        private void SetObjectiveDisplay(Quest quest)
        {
            for (int i = 0; i < quest.objectives.Count; i++)
            {
                ObjectiveDisplay od = Instantiate(objectiveDisplayPrefab, objectiveDisplayTransform) as ObjectiveDisplay;
                od.Initailize();
                od.SetObjectiveDisplay(quest.objectives[i]);
            }

            detailsDescriptionText.gameObject.transform.SetAsLastSibling();
            detailsDescriptionText.text = quest.description;
        }

        private void SetRewardListDisplay(Quest quest)
        {
            for (int i = 0; i < quest.rewardItemList.Count; i++)
            {
                InventoryContentSlot ics = Instantiate(inventoryContentSlotPrefab, rewardListTransform) as InventoryContentSlot;
            }
            SetRewardExpGoldText(quest);
        }

        private void SetRewardExpGoldText(Quest quest)
        {
            rewardExpGoldText.text = "보상 골드 : " + quest.rewardGold + " G" +
                                     "  보상 경험치 : " + quest.rewardExp + " Exp";
        }

        private void OpenDetails()
        {
            detailsObject.SetActive(true);
        }
    }
}
