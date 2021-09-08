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
        [SerializeField] private GameObject nextStatusObject;
        [SerializeField] private TMP_Text nextStatusText;
        [SerializeField] private TMP_Text nextUpStatusText;
        [SerializeField] private Button materialSlotBtn;
        [SerializeField] private Image materialSlotIcon;
        [SerializeField] private TMP_Text successProbText;
        [SerializeField] private TMP_Text needGoldText;
        [SerializeField] private Button enforceBtn;
        [SerializeField] private TMP_Text disableMaxLevelText;

        [Header("Item")]
        [SerializeField] private Item currentSelectItem;
        [SerializeField] private Item selectMaterialItem;

        private StringBuilder sb = new StringBuilder();
        private EnforceWindowUI enforceWindowUI;

        public Item CurrentSelectItem { get => currentSelectItem; private set => currentSelectItem = value; }
        public Item SelectMaterialItem { get => selectMaterialItem; private set => selectMaterialItem = value; }


        public void Init()
        {
            Transform t = transform.Find("UI Background").transform;
            nameText = UtilHelper.Find<TMP_Text>(t, "Name");
            enforceLevelText = UtilHelper.Find<TMP_Text>(t, "EnforceLevel");
            statusText = UtilHelper.Find<TMP_Text>(t, "Status/Text");
            nextStatusObject = t.Find("NextLevelStatus").gameObject;
            nextStatusText = UtilHelper.Find<TMP_Text>(t, "NextLevelStatus/Text");
            nextUpStatusText = UtilHelper.Find<TMP_Text>(t, "NextLevelStatus/UpStatusText");
            successProbText = UtilHelper.Find<TMP_Text>(t, "SuccessProb");
            needGoldText = UtilHelper.Find<TMP_Text>(t, "NeedGold/priceText");
            materialSlotIcon = UtilHelper.Find<Image>(t, "MaterialSlotBtn/Icon");
            materialSlotBtn = UtilHelper.Find<Button>(t, "MaterialSlotBtn");
            disableMaxLevelText = UtilHelper.Find<TMP_Text>(t, "DisableTextMaxLevel");
            enforceBtn = UtilHelper.Find<Button>(t, "EnforceBtn");
            if (enforceBtn != null)
                enforceBtn.onClick.AddListener(OnClickEnforceBtn);

            enforceWindowUI = GetComponentInParent<EnforceWindowUI>();

            this.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (SelectMaterialItem != null)
                ClearSelectMaterialItem();
        }
        private void OnClickEnforceBtn()
        {
            Debug.Log("강화하기 실행");
            if (SelectMaterialItem == null)
            {
                Debug.LogWarning("강화재료가 없는데 강화를 시도했다.");
                return;
            }

            //강화 비용이상 소지하고 있다면 강화 가능
            if (PlayerInventory.Instance.HaveGold(EnforceManager.Instance.EnforceNeedGold))
            {
                //EnforceManager에서 강화를 시도
                if (EnforceManager.Instance.TryEnforceItem()) enforceWindowUI.SuccessEnforce();
                else enforceWindowUI.FailEnforce();
            }
            else 
                Debug.Log("<color=#B51717>소지한 골드가 부족합니다.</color>");

        }
        public void SetMaterialSlot(Item item)
        {
            SelectMaterialItem = item;
            materialSlotIcon.sprite = SelectMaterialItem.itemIcon;
            enforceBtn.enabled = true;
        }
        internal void ClearSelectMaterialItem()
        {
            SelectMaterialItem = null;
            materialSlotIcon.sprite = null;
            materialSlotBtn.onClick.RemoveAllListeners();
            enforceBtn.enabled = false;
        }

        public void SetRightPanel(Item item)
        {
            CurrentSelectItem = item;
            EnforceManager.Instance.SetEnforceItem(item);
            ClearSelectMaterialItem();

            if (CurrentSelectItem != null)
            {
                if (CurrentSelectItem.itemType == ItemType.Weapon)
                {
                    SetRightPanelAsWeapon(CurrentSelectItem as WeaponItem);
                }
                else
                {
                    SetRightPanelAsEquipment(CurrentSelectItem as EquipItem);
                }
            }
        }
        private void SetRightPanelAsWeapon(WeaponItem weaponItem)
        {
            nameText.text = weaponItem.itemName;
            enforceLevelText.text = "< " + weaponItem.enforceLevel + "단계 >";
            SetitemStatusText(statusText, weaponItem);
            SetitemStatusText(nextStatusText, weaponItem, isRiseStatus: true);
            SetItemUpStatusText(nextUpStatusText, weaponItem);

            SetSuccessProbText();
            SetNeedGoldText();

            /* 최고등급 아이템 구별 */
            if (weaponItem.IsMaxEnforceLevel())
            {
                disableMaxLevelText.gameObject.SetActive(true);
                materialSlotBtn.gameObject.SetActive(false);
                nextStatusObject.SetActive(false);
                nextStatusText.text = null;
                nextUpStatusText.text = null;
                successProbText.text = null;
                needGoldText.text = "0";
            }
            else
            {
                disableMaxLevelText.gameObject.SetActive(false);
                nextStatusObject.SetActive(true);
                materialSlotBtn.gameObject.SetActive(true);
                materialSlotBtn.onClick.RemoveAllListeners();
                materialSlotBtn.onClick.AddListener(() => OnClickMaterialSlotBtn(weaponItem));
            }
        }
        private void SetRightPanelAsEquipment(EquipItem equipItem)
        {
            nameText.text = equipItem.itemName;
            enforceLevelText.text = "< " + equipItem.enforceLevel + "단계 >";
            SetitemStatusText(statusText, equipItem);
            SetitemStatusText(nextStatusText, equipItem, isRiseStatus: true);
            SetItemUpStatusText(nextUpStatusText, equipItem);

            SetSuccessProbText();
            SetNeedGoldText();

            /* 최고등급 아이템 구별 */
            if (equipItem.IsMaxEnforceLevel())
            {
                disableMaxLevelText.gameObject.SetActive(true);
                materialSlotBtn.gameObject.SetActive(false);
                nextStatusText.text = null;
                nextUpStatusText.text = null;
                successProbText.text = null;
                needGoldText.text = "0";
            }
            else
            {
                disableMaxLevelText.gameObject.SetActive(false);
                materialSlotBtn.gameObject.SetActive(true);
                materialSlotBtn.onClick.RemoveAllListeners();
                materialSlotBtn.onClick.AddListener(() => OnClickMaterialSlotBtn(equipItem));
            }
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
            if (PlayerInventory.Instance.HaveGold(EnforceManager.Instance.EnforceNeedGold))
                needGoldText.color = Color.white;
            else
                needGoldText.color = Color.red;
        }

        #region SetStatus Text Functions 
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
            if (value > 0)
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

        #endregion
    }

}
