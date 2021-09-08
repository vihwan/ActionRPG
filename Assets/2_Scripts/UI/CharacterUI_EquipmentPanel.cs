using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Events;

namespace SG
{
    public class CharacterUI_EquipmentPanel : MonoBehaviour
    {
        [Header("Equipment Slot")]
        [SerializeField] internal EquipSlot[] equipSlots;

        [Header("Basic")]
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private TMP_Text equipEffectText;
        [SerializeField] private Transform setTemplates;

        [Header("Individual")]
        [SerializeField] internal GameObject individualPanel;
        [SerializeField] private TMP_Text equipName;
        [SerializeField] private TMP_Text equipKind;
        [SerializeField] private TMP_Text equipEnforceLevel;
        [SerializeField] private TMP_Text equipSetName;
        [SerializeField] private TMP_Text equipStatus;
        [SerializeField] private TMP_Text equipDurability;
        [SerializeField] private TMP_Text equipExplain;
        [SerializeField] private Transform weaponRarityTransform;
        [SerializeField] private List<GameObject> rareStars;
        [SerializeField] private GameObject currentEquipObject;

        [Header("Comparison")]
        [SerializeField] internal GameObject comparisonPanel;
        [SerializeField] private TMP_Text equipName_Cf;
        [SerializeField] private TMP_Text equipKind_Cf;
        [SerializeField] private TMP_Text equipEnforceLevel_Cf;
        [SerializeField] private TMP_Text equipSetName_Cf;
        [SerializeField] private TMP_Text equipStatus_Cf;
        [SerializeField] private TMP_Text equipDurability_Cf;
        [SerializeField] private TMP_Text equipExplain_Cf;
        [SerializeField] private Transform weaponRarityTransform_Cf;
        [SerializeField] private List<GameObject> rareStars_Cf;
        [SerializeField] private bool canOpenComparisonPanel = true;

        [Header("Prefab")]
        [SerializeField] private GameObject RareStar;

        [Header("Button")]
        [SerializeField] internal Button openLeftInventoryBtn;
        [SerializeField] internal Button changeEquipBtn;
        [SerializeField] internal Button comparisonEquipBtn;

        [Header("Current Select")]
        [SerializeField] private ItemType currentSelectObjectType;

        [Header("Need Component")]
        [SerializeField] internal PlayerInventory playerInventory;
        private CharacterWindowUI characterWindowUI; //상위 계층 

        public StringBuilder sb = new StringBuilder();
        public void Init()
        {
            Transform t;
            equipSlots = GetComponentsInChildren<EquipSlot>(true);
            if (equipSlots != null)
            {
                foreach (EquipSlot slot in equipSlots)
                {
                    slot.Init();
                }
            }

            mainPanel = transform.Find("UI Background Main").gameObject;
            if (mainPanel != null)
            {
                equipEffectText = UtilHelper.Find<TMP_Text>(mainPanel.transform, "EquipEffect/Status/Text");
                setTemplates = UtilHelper.Find<Transform>(transform, "UI Background Main/SetEffect/Status");
            }

            individualPanel = transform.Find("UI Background Individual").gameObject;
            if (individualPanel != null)
            {
                t = individualPanel.transform;
                equipName = UtilHelper.Find<TMP_Text>(t, "Name");
                equipKind = UtilHelper.Find<TMP_Text>(t, "Kind");
                equipEnforceLevel = UtilHelper.Find<TMP_Text>(t, "EnforceLevel");
                equipSetName = UtilHelper.Find<TMP_Text>(t, "SetName");
                equipStatus = UtilHelper.Find<TMP_Text>(t, "Status/Text");
                equipDurability = UtilHelper.Find<TMP_Text>(t, "Durability/Text");
                equipExplain = UtilHelper.Find<TMP_Text>(t, "Explain/ExplainText");
                weaponRarityTransform = t.Find("Rarity").transform;
                currentEquipObject = t.Find("CurrentState").gameObject;
            }

            comparisonPanel = transform.Find("UI Background Comparison").gameObject;
            if(comparisonPanel != null)
            {
                t = comparisonPanel.transform;
                equipName_Cf = UtilHelper.Find<TMP_Text>(t, "Name");
                equipKind_Cf = UtilHelper.Find<TMP_Text>(t, "Kind");
                equipEnforceLevel_Cf = UtilHelper.Find<TMP_Text>(t, "EnforceLevel");
                equipSetName_Cf = UtilHelper.Find<TMP_Text>(t, "SetName");
                equipStatus_Cf = UtilHelper.Find<TMP_Text>(t, "Status/Text");
                equipDurability_Cf = UtilHelper.Find<TMP_Text>(t, "Durability/Text");
                equipExplain_Cf = UtilHelper.Find<TMP_Text>(t, "Explain/ExplainText");
                weaponRarityTransform_Cf = t.Find("Rarity").transform;
            }


            #region Prefab Initailize
            RareStar = Resources.Load<Image>("Prefabs/RarityStar").gameObject;
            #endregion

            #region Button Initialize
            openLeftInventoryBtn = UtilHelper.Find<Button>(individualPanel.transform, "OpenPanelBtn");
            if (openLeftInventoryBtn != null)
                openLeftInventoryBtn.onClick.AddListener(OpenLeftEquipmentInventory);

            changeEquipBtn = UtilHelper.Find<Button>(individualPanel.transform, "ChangeEquipBtn");
            if (changeEquipBtn != null)
            {
                changeEquipBtn.gameObject.SetActive(false);
                //장비 교체는 인벤토리 슬롯을 클릭할 때마다 AddListener를 등록
            }
 

            comparisonEquipBtn = UtilHelper.Find<Button>(individualPanel.transform, "ComparisonEquipBtn");
            if (comparisonEquipBtn != null)
            {
                comparisonEquipBtn.onClick.AddListener(()=> SetComparisonPanel(canOpenComparisonPanel));
                comparisonEquipBtn.gameObject.SetActive(false);
            }


            #endregion

            #region Component Initialize
            playerInventory = FindObjectOfType<PlayerInventory>();
            if (playerInventory == null)
                Debug.LogWarning("playerInventory를 찾지 못했습니다.");

            characterWindowUI = GetComponentInParent<CharacterWindowUI>();
            #endregion
        }

        private void OnEnable()
        {
            SetParameterMain(playerInventory.currentEquipmentSlots);
            SetAllEquipSlots(playerInventory.currentEquipmentSlots);
        }

        //장비 슬롯들을 플레이어가 착용하고 있는 장비에 맞게 세팅하는 함수입니다.
        private void SetAllEquipSlots(EquipItem[] currentEquipmentSlots)
        {
            equipSlots[(int)ItemType.Tops].AddItem(currentEquipmentSlots[(int)ItemType.Tops]);
            equipSlots[(int)ItemType.Bottoms].AddItem(currentEquipmentSlots[(int)ItemType.Bottoms]);
            equipSlots[(int)ItemType.Gloves].AddItem(currentEquipmentSlots[(int)ItemType.Gloves]);
            equipSlots[(int)ItemType.Shoes].AddItem(currentEquipmentSlots[(int)ItemType.Shoes]);
            equipSlots[(int)ItemType.Accessory].AddItem(currentEquipmentSlots[(int)ItemType.Accessory]);
            equipSlots[(int)ItemType.SpecialEquip].AddItem(currentEquipmentSlots[(int)ItemType.SpecialEquip]);
        }
        private void UpdateSpecificEquipSlot(EquipItem equipItem)
        {
            equipSlots[(int)currentSelectObjectType].AddItem(equipItem);
        }
        private void SetAllEquipSlotsButtonInteractive(bool state)
        {
            for (int i = 0; i < equipSlots.Length; i++)
            {
                equipSlots[i].itemBtn.interactable = state;
            }
        }


        public void UpdateMainPanel()
        {
            SetParameterMain(playerInventory.currentEquipmentSlots);
        }

        private void SetParameterMain(EquipItem[] currentEquipmentSlots)
        {
            //장비 전체 효과를 설정하는 함수
            //플레이어가 착용하고 있는 각 장비들의 스테이터스를 전부 더해 보여주어야한다.

            int wholeHp = 0;
            int wholeAttack = 0;
            int wholeDefense = 0;
            int wholeCritical = 0;
            int wholeCriticalDMG = 0;
            int wholeStamina = 0;

            foreach (EquipItem item in currentEquipmentSlots)
            {
                wholeHp += item.itemAttributes[(int)Attribute.Hp].value;
                wholeAttack += item.itemAttributes[(int)Attribute.Attack].value;
                wholeDefense += item.itemAttributes[(int)Attribute.Defense].value;
                wholeCritical += item.itemAttributes[(int)Attribute.Critical].value;
                wholeCriticalDMG += item.itemAttributes[(int)Attribute.CriticalDamage].value;
                wholeStamina += item.itemAttributes[(int)Attribute.Stamina].value;
            }

            SetItemStatusText(equipEffectText, wholeHp, wholeAttack, wholeDefense, wholeCritical, wholeCriticalDMG, wholeStamina);
            //CreateSetItemTemplate(currentEquipmentSlots);
        }


        //장비 아이콘을 클릭할 경우, 아이템의 개별 상세정보창이 우측에 표시되도록 합니다.
        //해당 아이콘 버튼을 누르면, 장비 슬롯에 등록된 아이템 정보를 가져와 정보를 세팅하고 패널에 표시합니다.
        public void SetParameterIndividualEquipItem(EquipItem equipItem)
        {
            equipName.text = equipItem.itemName;
            equipKind.text = equipItem.kind;
            equipEnforceLevel.text = "+" + equipItem.enforceLevel;
            equipSetName.text = null;
            equipDurability.text = equipItem.currentDurability + " / " + equipItem.maxDurability;
            equipExplain.text = equipItem.itemDescription;

            SetItemStatusText(equipStatus, equipItem);
            CreateRarityStar(weaponRarityTransform, rareStars, equipItem);
            SetCurrentStateObjects(equipItem.isArmed);

            currentSelectObjectType = equipItem.itemType;

            if (changeEquipBtn.gameObject.activeSelf == true)
            {
                changeEquipBtn.onClick.RemoveAllListeners();
                changeEquipBtn.onClick.AddListener(() => ChangeEquipBtnEvent(equipItem));
                changeEquipBtn.onClick.AddListener(CloseComparisonPanel);
            }
        }

        public void SetParameterComparisonEquipItem(EquipItem equipItem)
        {
            equipName_Cf.text = equipItem.itemName;
            equipKind_Cf.text = equipItem.kind;
            equipEnforceLevel_Cf.text = "+" + equipItem.enforceLevel;
            equipSetName_Cf.text = null;
            equipDurability_Cf.text = equipItem.currentDurability + " / " + equipItem.maxDurability;
            equipExplain_Cf.text = equipItem.itemDescription;

            SetItemStatusText(equipStatus_Cf, equipItem);
            CreateRarityStar(weaponRarityTransform_Cf, rareStars_Cf, equipItem);
        }

        //장비 교체 버튼(changeEquipBtn)을 눌렀을 경우 발생하는 이벤트 함수
        private void ChangeEquipBtnEvent(EquipItem selectEquipItem)
        {
            playerInventory.ChangeCurrentEquipment(selectEquipItem);
            SetCurrentStateObjects(selectEquipItem.isArmed);
            characterWindowUI.equipmentInventoryList.UpdateSlots();
            UpdateSpecificEquipSlot(playerInventory.currentEquipmentSlots[(int)currentSelectObjectType]);
        }

        public void DeselectAllEquipSlots()
        {
            foreach (EquipSlot slot in equipSlots)
            {
                slot.isSelect = false;
                slot.ChangeFrameColor(slot.isSelect);
            }
        }

        public void OpenIndividualItemPanel()
        {
            individualPanel.SetActive(true);
            characterWindowUI.backBtn.gameObject.SetActive(true);
        }

        private void SetItemStatusText(TMP_Text tMP_Text, EquipItem equipItem)
        {
            sb.Length = 0;
            AddStatusText(equipItem.itemAttributes[(int)Attribute.Hp].value, "체력");
            AddStatusText(equipItem.itemAttributes[(int)Attribute.Attack].value, "공격력");
            AddStatusText(equipItem.itemAttributes[(int)Attribute.Defense].value, "방어력");
            AddStatusText(equipItem.itemAttributes[(int)Attribute.Critical].value, "치명타", isPercent: true);
            AddStatusText(equipItem.itemAttributes[(int)Attribute.CriticalDamage].value, "치명타 배율", isPercent: true);
            AddStatusText(equipItem.itemAttributes[(int)Attribute.Stamina].value, "스태미나");

            tMP_Text.text = sb.ToString();
        }

        private void SetCurrentStateObjects(bool state)
        {
            currentEquipObject.SetActive(state);
            changeEquipBtn.gameObject.SetActive(!state);
            comparisonEquipBtn.gameObject.SetActive(!state);
        }

        //장비 교체 버튼을 클릭했을 시 발생하는 이벤트 함수
        private void OpenLeftEquipmentInventory()
        {
            //왼쪽 장비 인벤토리 리스트 패널을 활성화
            characterWindowUI.equipmentInventoryList.SetEquipItemTypeToView(currentSelectObjectType);
            characterWindowUI.equipmentInventoryList.gameObject.SetActive(true);

            //모든 장비 슬롯의 버튼 interactive를 비활성화
            SetAllEquipSlotsButtonInteractive(false);

            //장비 교체 버튼 비활성화
            openLeftInventoryBtn.gameObject.SetActive(false);

            //뒤로가기 버튼 활성화
            if (characterWindowUI.backBtn.gameObject.activeSelf == false)
                characterWindowUI.backBtn.gameObject.SetActive(true);
        }

        public void CloseLeftEquipmentInventory()
        {
            //개별 아이템 정보를 갱신
            SetParameterIndividualEquipItem(playerInventory.currentEquipmentSlots[(int)currentSelectObjectType]);
            //모든 장비 슬롯의 버튼 interactive를 활성화
            SetAllEquipSlotsButtonInteractive(true);
        }

        private void SetComparisonPanel(bool state)
        {
            canOpenComparisonPanel = !canOpenComparisonPanel;
            comparisonPanel.SetActive(state);
            SetParameterComparisonEquipItem(playerInventory.currentEquipmentSlots[(int)currentSelectObjectType]);
        }
        public void CloseComparisonPanel()
        {
            canOpenComparisonPanel = true;
            comparisonPanel.SetActive(false);
        }

        private void CreateRarityStar(Transform transform, List<GameObject> rareStarsList, EquipItem equipItem)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            rareStarsList.Clear();

            int rarityCount = 0;
            if (rarityCount < equipItem.rarity)
            {
                while (rarityCount < equipItem.rarity)
                {
                    GameObject star = Instantiate(RareStar, transform);
                    rareStarsList.Add(star);
                    rarityCount++;
                }
            }
        }

        //착용하고 있는 아이템들을 서로 비교하여, 일정 갯수 이상 일 때 세트 아이템 효과를 출력하는 템플릿을 생성하는 함수
        private void CreateSetItemTemplate(EquipItem[] currentEquipmentSlots)
        {
            //int setItem_Count = 0;
            foreach (EquipItem item in currentEquipmentSlots)
            {

            }
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
        private void SetItemStatusText(TMP_Text tMP_Text, int hp, int attack, int defense, int critical, int criticalDMG, int stamina)
        {
            sb.Length = 0;
            AddStatusText(hp, "체력");
            AddStatusText(attack, "공격력");
            AddStatusText(defense, "방어력");
            AddStatusText(critical, "치명타", isPercent: true);
            AddStatusText(criticalDMG, "치명타 배율", isPercent: true);
            AddStatusText(stamina, "스태미나");


            if (sb.Length == 0)
            {
                sb.Append("효과 없음");
            }

            tMP_Text.text = sb.ToString();
        }
    }
}
