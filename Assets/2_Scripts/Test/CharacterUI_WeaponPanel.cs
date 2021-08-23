using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

namespace SG
{
    public class CharacterUI_WeaponPanel : MonoBehaviour
    {
        [Header("Basic")]
        [SerializeField] private TMP_Text weaponName;
        [SerializeField] private TMP_Text weaponKind;
        [SerializeField] private TMP_Text weaponStatus;
        [SerializeField] private TMP_Text weaponDurability;
        [SerializeField] private Transform weaponRarityTransform;
        [SerializeField] private GameObject RareStar;
        [SerializeField] private List<GameObject> rareStars;
        [SerializeField] private TMP_Text weaponExplain;
        [SerializeField] private GameObject currentEquipObject;

        [Header("Comparison Panel")]
        [SerializeField] internal GameObject comparisonPanel;
        [SerializeField] private TMP_Text weaponName_Cf; //confer의 약어 : 비교하다라는 의미를 뜻함.
        [SerializeField] private TMP_Text weaponKind_Cf;
        [SerializeField] private TMP_Text weaponStatus_Cf;
        [SerializeField] private TMP_Text weaponDurability_Cf;
        [SerializeField] private Transform weaponRarityTransform_Cf;
        [SerializeField] private List<GameObject> rareStars_Cf;
        [SerializeField] private TMP_Text weaponExplain_Cf;
        [SerializeField] private bool canOpenComparisonPanel = true;

        [Header("Buttons")]
        [SerializeField] internal Button openPanelBtn;
        [SerializeField] internal Button changeWeaponBtn;
        [SerializeField] internal Button comparisonWeaponBtn;

        [Header("Need Component")]
        [SerializeField] internal PlayerInventory playerInventory;
        private CharacterWindowUI characterWindowUI; //상위 계층 

        private StringBuilder sb = new StringBuilder();
        public void Init()
        {
            #region Panel Initialize
            weaponName = UtilHelper.Find<TMP_Text>(transform, "UI Background/Name");
            weaponKind = UtilHelper.Find<TMP_Text>(transform, "UI Background/Kind");
            weaponStatus = UtilHelper.Find<TMP_Text>(transform, "UI Background/Status/Text");
            weaponDurability = UtilHelper.Find<TMP_Text>(transform, "UI Background/Durability/Text");
            weaponRarityTransform = transform.Find("UI Background/Rarity");
            weaponExplain = UtilHelper.Find<TMP_Text>(transform, "UI Background/Explain/ExplainText");
            currentEquipObject = transform.Find("UI Background/CurrentState").gameObject;
            RareStar = Resources.Load<Image>("Prefabs/RarityStar").gameObject;
            #endregion

            #region Comparison Panel Initialize
            comparisonPanel = transform.Find("UI Background Comparison").gameObject;
            weaponName_Cf = UtilHelper.Find<TMP_Text>(transform, "UI Background Comparison/Name");
            weaponKind_Cf = UtilHelper.Find<TMP_Text>(transform, "UI Background Comparison/Kind");
            weaponStatus_Cf = UtilHelper.Find<TMP_Text>(transform, "UI Background Comparison/Status/Text");
            weaponDurability_Cf = UtilHelper.Find<TMP_Text>(transform, "UI Background Comparison/Durability/Text");
            weaponRarityTransform_Cf = transform.Find("UI Background Comparison/Rarity");
            weaponExplain_Cf = UtilHelper.Find<TMP_Text>(transform, "UI Background Comparison/Explain/ExplainText");
            #endregion

            #region Button Initialize
            openPanelBtn = UtilHelper.Find<Button>(transform, "UI Background/OpenPanelBtn");
            if (openPanelBtn != null)
                openPanelBtn.onClick.AddListener(OpenWeaponLeftInventory);

            changeWeaponBtn = UtilHelper.Find<Button>(transform, "UI Background/ChangeWeaponBtn");
            if (changeWeaponBtn != null)
            {
                changeWeaponBtn.onClick.AddListener(null);
                changeWeaponBtn.gameObject.SetActive(false);
            }

            comparisonWeaponBtn = UtilHelper.Find<Button>(transform, "UI Background/ComparisonWeaponBtn");
            if (comparisonWeaponBtn != null)
            {
                comparisonWeaponBtn.onClick.AddListener(() => SetComparisonPanel(canOpenComparisonPanel));
                comparisonWeaponBtn.gameObject.SetActive(false);
            }
            #endregion

            #region Component Initialize
            playerInventory = FindObjectOfType<PlayerInventory>();
            if (playerInventory == null)
                Debug.LogWarning("playerInventory를 찾지 못했습니다.");

            characterWindowUI = GetComponentInParent<CharacterWindowUI>();
            #endregion

            comparisonPanel.SetActive(false);
        }

        public void OnEnable()
        {
            SetParameter(playerInventory.currentWeapon);
        }


        //무기 교체 버튼을 눌렀을 경우 발생하는 이벤트 함수
        private void ChangeWeaponBtnEvent(WeaponItem selectWeapon)
        {
            playerInventory.ChangeCurrentWeapon(selectWeapon);
            SetCurrentStateObjects(selectWeapon.isArmed);
            characterWindowUI.weaponInventoryList.UpdateSlots();
        }

        private void AddStatusText(int value, string statName, bool isPercent = false)
        {
            if (value != 0)
            {
                if (sb.Length > 0)
                    sb.AppendLine();

                sb.Append(statName);
                switch (statName.Length)
                {
                    case 2:
                        sb.Append("                      ");
                        break;

                    case 3:
                        sb.Append("                   ");
                        break;

                    case 4:
                        sb.Append("                ");
                        break;

                    case 6:
                        sb.Append("           ");
                        break;
                }


                if (value > 0)
                {
                    sb.Append("+");
                    sb.Append(" ");
                }

                if (isPercent)
                {
                    sb.Append(value);
                    sb.Append("%");
                }
                else
                {
                    sb.Append(value);
                }
            }
        }
        private void SetItemStatusText(TMP_Text tMP_Text, WeaponItem playerWeapon)
        {
            sb.Length = 0;
            AddStatusText(playerWeapon.itemAttributes[(int)Attribute.Hp].value, "체력");
            AddStatusText(playerWeapon.itemAttributes[(int)Attribute.Attack].value, "공격력");
            AddStatusText(playerWeapon.itemAttributes[(int)Attribute.Defense].value, "방어력");
            AddStatusText(playerWeapon.itemAttributes[(int)Attribute.Critical].value, "치명타", isPercent: true);
            AddStatusText(playerWeapon.itemAttributes[(int)Attribute.CriticalDamage].value, "치명타 배율", isPercent: true);
            AddStatusText(playerWeapon.itemAttributes[(int)Attribute.Stamina].value, "스태미나");

            tMP_Text.text = sb.ToString();
        }

        public void SetParameter(WeaponItem playerWeapon)
        {
            weaponName.text = playerWeapon.itemName;
            weaponKind.text = playerWeapon.kind;

            SetItemStatusText(weaponStatus, playerWeapon);

            weaponDurability.text =
                playerWeapon.currentDurability.ToString() + " / " + playerWeapon.maxDurability.ToString();

            CreateRarityStar(weaponRarityTransform, rareStars , playerWeapon);
            weaponExplain.text = playerWeapon.itemDescription;
            SetCurrentStateObjects(playerWeapon.isArmed);

            if (changeWeaponBtn.gameObject.activeSelf == true)
            {
                changeWeaponBtn.onClick.RemoveAllListeners();
                changeWeaponBtn.onClick.AddListener(() => ChangeWeaponBtnEvent(playerWeapon));
                changeWeaponBtn.onClick.AddListener(CloseComparisonPanel);
            }
        }
        public void SetParameter_Comparision(WeaponItem currentWeapon)
        {
            weaponName_Cf.text = currentWeapon.itemName;
            weaponKind_Cf.text = currentWeapon.kind;

            SetItemStatusText(weaponStatus_Cf, currentWeapon);

            weaponDurability_Cf.text =
                currentWeapon.currentDurability.ToString() + " / " + currentWeapon.maxDurability.ToString();

            CreateRarityStar(weaponRarityTransform_Cf, rareStars_Cf , currentWeapon);
            weaponExplain_Cf.text = currentWeapon.itemDescription;
        }

        private void SetCurrentStateObjects(bool state)
        {
            //현재 장착중인 아이템이라면, 무기 교체 버튼이 사라져야함
            //장착중인 아이템이 아니라면, 무기 교체 버튼이 나타나야함
            currentEquipObject.SetActive(state);
            changeWeaponBtn.gameObject.SetActive(!state);
            comparisonWeaponBtn.gameObject.SetActive(!state);
        }

        private void CreateRarityStar(Transform transform, List<GameObject> rareStarsList, WeaponItem playerWeapon)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            rareStarsList.Clear();

            int rarityCount = 0;
            if (rarityCount < playerWeapon.rarity)
            {
                while (rarityCount < playerWeapon.rarity)
                {
                    GameObject star = Instantiate(RareStar, transform);
                    rareStarsList.Add(star);
                    rarityCount++;
                }
            }
/*            else if (rarityCount > playerWeapon.rarity)
            {
                while (rarityCount > playerWeapon.rarity)
                {
                    GameObject deleteStar = rareStars[0];
                    rareStarsList.Remove(deleteStar);
                    Destroy(deleteStar);
                    rarityCount--;
                }
            }
            return;*/
        }


        //무기 교체 버튼을 누르면, 왼쪽 패널의 무기 인벤토리를 엽니다.
        private void OpenWeaponLeftInventory()
        {
            //왼쪽 패널에 무기 인벤토리를 생성
            characterWindowUI.weaponInventoryList.gameObject.SetActive(true);
            //뒤로가기 버튼 활성화
            characterWindowUI.backBtn.gameObject.SetActive(true);
            openPanelBtn.gameObject.SetActive(false);
        }

        private void SetComparisonPanel(bool state)
        {
            canOpenComparisonPanel = !canOpenComparisonPanel;
            comparisonPanel.SetActive(state);
            SetParameter_Comparision(playerInventory.currentWeapon);
        }

        public void CloseComparisonPanel()
        {
            canOpenComparisonPanel = true;
            comparisonPanel.SetActive(false);
        }
    }
}
