using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class EnforceUI_RightPanel : MonoBehaviour
    {
        [Header("Enforce Panel")]
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text enforceLevelText;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private TMP_Text nextStatusText;
        [SerializeField] private TMP_Text nextUpStatusText;
        [SerializeField] private Button materialSlotBtn;
        [SerializeField] private Image materialSlotIcon;
        [SerializeField] private TMP_Text successProbText;
        [SerializeField] private TMP_Text needGoldText;
        [SerializeField] private Button enforceBtn;

        [Header("Selected Material Item")]
        [SerializeField] private Item selectMaterialItem;

        private StringBuilder sb = new StringBuilder();
        private EnforceWindowUI enforceWindowUI;
        public void Init()
        {
            Transform t = transform.Find("UI Background").transform;
            nameText = UtilHelper.Find<TMP_Text>(t, "Name");
            enforceLevelText = UtilHelper.Find<TMP_Text>(t, "EnforceLevel");
            statusText = UtilHelper.Find<TMP_Text>(t, "Status/Text");
            nextStatusText = UtilHelper.Find<TMP_Text>(t, "NextLevelStatus/Text");
            nextUpStatusText = UtilHelper.Find<TMP_Text>(t, "NextLevelStatus/UpStatusText");
            successProbText = UtilHelper.Find<TMP_Text>(t, "SuccessProb");
            needGoldText = UtilHelper.Find<TMP_Text>(t, "NeedGold/priceText");
            materialSlotIcon = UtilHelper.Find<Image>(t, "MaterialSlotBtn/Icon");
            materialSlotBtn = UtilHelper.Find<Button>(t, "MaterialSlotBtn");
            enforceBtn = UtilHelper.Find<Button>(t, "EnforceBtn");
            if (enforceBtn != null)
                enforceBtn.onClick.AddListener(OnClickEnforceBtn);

            enforceWindowUI = GetComponentInParent<EnforceWindowUI>();

            this.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (selectMaterialItem != null)
                ClearSelectedItem();
        }
        private void OnClickEnforceBtn()
        {
            Debug.Log("강화하기 실행");
            if(selectMaterialItem == null)
            {
                Debug.LogWarning("강화재료가 없는데 강화를 시도했다.");
                return;
            }

            //EnforceManager에서 강화를 시도
            if (EnforceManager.Instance.TryEnforceItem())
            {
                //강화 성공시 실행할 메소드
            }
            else
            {
                //강화 실패시 실행할 메소드
            }
        }
        public void SetMaterialSlot(Item item)
        {
            selectMaterialItem = item;
            materialSlotIcon.sprite = selectMaterialItem.itemIcon;
            enforceBtn.enabled = true;
        }
        private void ClearSelectedItem()
        {
            selectMaterialItem = null;
            materialSlotIcon.sprite = null;
            materialSlotBtn.onClick.RemoveAllListeners();
            enforceBtn.enabled = false;
        }

        public void SetRightPanel(Item item)
        {
            EnforceManager.Instance.SetEnforceItem(item);
            ClearSelectedItem();

            if (item.itemType == ItemType.Weapon)
            {
                SetRightPanelAsWeapon(item as WeaponItem);
            }
            else
            {
                SetRightPanelAsEquipment(item as EquipItem);
            }
        }
        private void SetRightPanelAsWeapon(WeaponItem weaponItem)
        {
            nameText.text = weaponItem.itemName;
            enforceLevelText.text = "< " + weaponItem.enforceLevel + "단계 >";
            SetitemStatusText(statusText, weaponItem);
            SetitemStatusText(nextStatusText, weaponItem , isRiseStatus: true);
            SetItemUpStatusText(nextUpStatusText, weaponItem);

            SetSuccessProbText();      
            SetNeedGoldText();

            materialSlotBtn.onClick.RemoveAllListeners();
            materialSlotBtn.onClick.AddListener(() => OnClickMaterialSlotBtn(weaponItem));
        }


        private void SetRightPanelAsEquipment(EquipItem equipItem)
        {
            nameText.text = equipItem.itemName;
            enforceLevelText.text = "< " + equipItem.enforceLevel + "단계 >";
            SetitemStatusText(statusText, equipItem);
            SetitemStatusText(nextStatusText, equipItem, isRiseStatus : true);
            SetItemUpStatusText(nextUpStatusText, equipItem);

            SetSuccessProbText();
            SetNeedGoldText();

            materialSlotBtn.onClick.RemoveAllListeners();
            materialSlotBtn.onClick.AddListener(() => OnClickMaterialSlotBtn(equipItem));
        }

        private void OnClickMaterialSlotBtn(Item item)
        {
            //Enforce Material List를 열기
            enforceWindowUI.enforceMaterialSelectList.OpenList(item);
        }

        private void SetSuccessProbText()
        {
            successProbText.text = "성공확률 : " + EnforceManager.Instance.SuccessProb + "%";
        }
        private void SetNeedGoldText()
        {
            needGoldText.text = EnforceManager.Instance.EnforceNeedGold.ToString();
        }
        private void SetitemStatusText(TMP_Text tmpText, WeaponItem weaponItem, bool isRiseStatus = false)
        {
            sb.Length = 0;
            int count = 0;
            if (isRiseStatus.Equals(true))
                count = EnforceManager.Instance.EnforceRiseStatus;

            AddStatusText(weaponItem.itemAttributes[(int)Attribute.Hp].value + count, "체력");
            AddStatusText(weaponItem.itemAttributes[(int)Attribute.Attack].value + count, "공격력");
            AddStatusText(weaponItem.itemAttributes[(int)Attribute.Defense].value + count, "방어력");
            AddStatusText(weaponItem.itemAttributes[(int)Attribute.Critical].value + count, "치명타", isPercent: true);
            AddStatusText(weaponItem.itemAttributes[(int)Attribute.CriticalDamage].value + count, "치명타 배율", isPercent: true);
            AddStatusText(weaponItem.itemAttributes[(int)Attribute.Stamina].value + count, "스태미나");

            tmpText.text = sb.ToString();
        }
        private void SetitemStatusText(TMP_Text tmpText, EquipItem equipItem, bool isRiseStatus = false)
        {
            sb.Length = 0;
            int count = 0;
            if (isRiseStatus.Equals(true))
                count = EnforceManager.Instance.EnforceRiseStatus;

            AddStatusText(equipItem.itemAttributes[(int)Attribute.Hp].value + count, "체력");
            AddStatusText(equipItem.itemAttributes[(int)Attribute.Attack].value + count, "공격력");
            AddStatusText(equipItem.itemAttributes[(int)Attribute.Defense].value + count, "방어력");
            AddStatusText(equipItem.itemAttributes[(int)Attribute.Critical].value + count, "치명타", isPercent: true);
            AddStatusText(equipItem.itemAttributes[(int)Attribute.CriticalDamage].value + count, "치명타 배율", isPercent: true);
            AddStatusText(equipItem.itemAttributes[(int)Attribute.Stamina].value + count, "스태미나");

            tmpText.text = sb.ToString();
        }
        private void SetItemUpStatusText(TMP_Text nextUpStatusText, WeaponItem weaponItem)
        {
            int count = EnforceManager.Instance.EnforceRiseStatus;
            sb.Length = 0;
            AddUpStatusText(count);
            AddUpStatusText(count);
            AddUpStatusText(count);
            AddUpStatusText(count, isPercent: true);
            AddUpStatusText(count, isPercent: true);
            AddUpStatusText(count);

            nextUpStatusText.text = sb.ToString();
        }
        private void SetItemUpStatusText(TMP_Text nextUpStatusText, EquipItem equipItem)
        {
            int count = EnforceManager.Instance.EnforceRiseStatus;
            sb.Length = 0;
            AddUpStatusText(count);
            AddUpStatusText(count);
            AddUpStatusText(count);
            AddUpStatusText(count, isPercent: true);
            AddUpStatusText(count, isPercent: true);
            AddUpStatusText(count);

            nextUpStatusText.text = sb.ToString();
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
        private void AddUpStatusText(int value, bool isPercent = false)
        {
            if(value > 0)
            {
                if (sb.Length > 0)
                    sb.AppendLine();

                sb.Append("(+");
                sb.Append(value);
                if (isPercent)
                    sb.Append("%");
                sb.Append(")");
            }
        }
    }

}
