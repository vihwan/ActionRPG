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
        private List<QuestDisplay> questPanelList = new List<QuestDisplay>();
        private List<ObjectiveDisplay> objectiveDisplays = new List<ObjectiveDisplay>();
        private List<RewardItemSlot> rewardSlots = new List<RewardItemSlot>();

        [Header("GameObjects")]
        public GameObject detailsObject;
        public Button OnQuestHUDBtn;
        public TMP_Text detailsDescriptionText;
        public TMP_Text rewardExpGoldText;

        [Header("Transforms")]
        [SerializeField] private Transform questListTransform;
        [SerializeField] private Transform objectiveDisplayTransform;
        [SerializeField] private Transform rewardListTransform;

        [Header("Prefabs")]
        [SerializeField] public QuestDisplay questDisplayPrefab;
        [SerializeField] public ObjectiveDisplay objectiveDisplayPrefab;
        [SerializeField] public RewardItemSlot rewardItemSlotPrefab;

        private InputHandler inputHandler;
        public RewardItem_InfoPanel infoPanel;
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

            infoPanel = GetComponentInChildren<RewardItem_InfoPanel>(true);
            if (infoPanel != null)
                infoPanel.Init();

            rewardItemSlotPrefab = Database.Instance.prefabDatabase.rewardItemSlot;
            CloseQuestPanel();
        }

        public void OpenQuestPanelEvent()
        {
            SetQuestPanel();
            OpenQuestPanel();
            detailsObject.SetActive(false);
            infoPanel.gameObject.SetActive(false);
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
            //세팅 초기화. 
            if(questPanelList.Count > 0)
            {
                for (int i = 0; i < questPanelList.Count; i++)
                {
                    Destroy(questPanelList[i].gameObject);
                }
                questPanelList.Clear();
            }

            //플레이어가 소유중인 퀘스트의 수만큼 QuestDisplay를 생성
            for (int i = 0; i < PlayerQuestInventory.Instance.Quests.Count; i++)
            {
                Quest quest = PlayerQuestInventory.Instance.Quests[i];
                QuestDisplay qd = Instantiate(questDisplayPrefab, questListTransform) as QuestDisplay;
                qd.Init();
                qd.SetQuestDisplay(quest);
                qd.AddClickBtnAction(() => QuestPanelAction(quest));
                questPanelList.Add(qd);
            }
        }

        private void QuestPanelAction(Quest quest)
        {
            SetObjectiveDisplay(quest);
            SetRewardListDisplay(quest);
            OpenDetails();
            OnQuestHUDBtn.onClick.AddListener(()=> GUIManager.instance.questAlertUI.OnQuestAlertUI(quest));
        }

        private void SetObjectiveDisplay(Quest quest)
        {
            if(objectiveDisplays.Count > 0)
            {
                for (int i = 0; i < objectiveDisplays.Count; i++)
                {
                    Destroy(objectiveDisplays[i].gameObject);
                }
                objectiveDisplays.Clear();
            }

            for (int i = 0; i < quest.objectives.Count; i++)
            {
                if(quest.objectives[i].state.Equals(QuestObjectiveState.Active) || 
                   quest.objectives[i].state.Equals(QuestObjectiveState.Complete))
                {
                    ObjectiveDisplay od = Instantiate(objectiveDisplayPrefab, objectiveDisplayTransform) as ObjectiveDisplay;
                    od.Initailize();
                    od.SetObjectiveDisplay(quest.objectives[i]);
                    objectiveDisplays.Add(od);
                }
            }
            detailsDescriptionText.gameObject.transform.SetAsLastSibling();
            detailsDescriptionText.text = quest.description;
        }
        private void SetRewardListDisplay(Quest quest)
        {
            if(rewardSlots.Count > 0)
            {
                for (int i = 0; i < rewardSlots.Count; i++)
                {
                    Destroy(rewardSlots[i].gameObject);
                }
                rewardSlots.Clear();
            }

            for (int i = 0; i < quest.rewardItemList.Count; i++)
            {
                RewardItemSlot ris = Instantiate(rewardItemSlotPrefab, rewardListTransform) as RewardItemSlot;
                ris.Init();
                ris.AddItem(quest.rewardItemList[i].rewardItem, quest.rewardItemList[i].itemCount);
                rewardSlots.Add(ris);
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

        public void CloseDetails()
        {
            detailsObject.SetActive(false);
        }
    }
}
