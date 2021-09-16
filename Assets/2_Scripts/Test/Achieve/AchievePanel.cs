using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class AchievePanel : MonoBehaviour
    {

        [SerializeField] private Button closeBtn;

        [Header("Achieve List")]
        private List<AchieveListDisplay> achieveListDisplays = new List<AchieveListDisplay>();
        private List<AchieveMissionDisplay> achieveMissions = new List<AchieveMissionDisplay>();


        [Header("Transform")]
        [SerializeField] private Transform achieveListTransform;
        [SerializeField] private Transform achieveMissionTransform;

        [Header("Prefabs"), Space(10)]
        [SerializeField] private AchieveListDisplay achieveListDisplayPrefab;
        [SerializeField] private AchieveMissionDisplay achieveMissionPanelPrefab;

        [Header("GameObject")]
        [SerializeField] private AchieveMissionTitle achieveMissionTitle;
        [SerializeField] private GameObject detailRight;
        [SerializeField] private GameObject scrollViewContent;



        private InputHandler inputHandler;

        public void Init()
        {
            inputHandler = FindObjectOfType<InputHandler>();

            Transform t = transform.Find("Background");
            closeBtn = UtilHelper.Find<Button>(t, "Header/CloseBtn");
            if (closeBtn != null)
                closeBtn.onClick.AddListener(() => inputHandler.HandleMenuFlag());

            if (achieveListTransform == null)
                achieveListTransform = transform.Find("List Window/AchieveList").transform;

            achieveMissionTitle = GetComponentInChildren<AchieveMissionTitle>(true);

            CloseAchievePanel();
        }

        public void OpenAchievePanelEvent()
        {
            Debug.Log("업적 버튼 클릭");
            OpenAchievePanel();
            SetAchievePanel();
            AppearFirstAchieveInfomation();
        }

        private void AppearFirstAchieveInfomation()
        {
            if(achieveListDisplays.Count > 0)
            {
                achieveListDisplays[0].OnClickBtn();
            }
        }

        private void OpenAchievePanel()
        {
            this.gameObject.SetActive(true);
            Debug.Log("업적 패널 열기");
        }
        public void CloseAchievePanel()
        {
            this.gameObject.SetActive(false);
        }

        private void SetAchievePanel()
        {
            //목록 초기화
            for (int i = 0; i < achieveListDisplays.Count; i++)
            {
                Destroy(achieveListDisplays[i].gameObject);
            }
            achieveListDisplays.Clear();

            // 업적 리스트를 가져와 생성합니다.

            for (int i = 0; i < AchieveManager.Instance.achieves.Count; i++)
            {
                Achieve achieve = AchieveManager.Instance.achieves[i];
                achieve.Init();
                AchieveListDisplay ald = Instantiate(achieveListDisplayPrefab, achieveListTransform) as AchieveListDisplay;
                ald.Init();
                ald.SetActiveListDisplay(achieve);
                ald.AddDisplayBtnLister(() => AchievePanelAction(achieve));
                achieveListDisplays.Add(ald);
            }
        }

        private void AchievePanelAction(Achieve achieve)
        {
            SetAchieveMission(achieve);
            OpenDetails();
        }

        private void OpenDetails()
        {
            detailRight.gameObject.SetActive(true);
        }

        private void CloseDetails()
        {
            detailRight.gameObject.SetActive(false);
        }

        private void SetAchieveMission(Achieve achieve)
        {
            achieveMissionTitle.SetMissionTitle(achieve);

            for (int i = 0; i < achieveMissions.Count; i++)
            {
                Destroy(achieveMissions[i].gameObject);
            }
            achieveMissions.Clear();

            for (int i = 0; i < achieve.achieveMissions.Count; i++)
            {
                AchieveMissionDisplay amp = Instantiate(achieveMissionPanelPrefab, achieveMissionTransform);
                amp.Init();
                amp.SetMission(achieve.achieveMissions[i]);
                achieveMissions.Add(amp);
            }

            SetScrollViewContentHeight(135 * achieveMissions.Count);
        }

        private void SetScrollViewContentHeight(float height)
        {
            scrollViewContent.GetComponent<RectTransform>().sizeDelta =
                    new Vector2(scrollViewContent.GetComponent<RectTransform>().sizeDelta.x, height);
        }
    }
}
