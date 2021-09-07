using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class EnforceWindowUI : MonoBehaviour
    {
        [Header("SpringBoardMenu")]
        [SerializeField] private EnforceMenuSpringBoard enforceMenuSpringBoard;

        [Header("Text")]
        [SerializeField] private TMP_Text userGoldText;

        [Header("Enforce UI LeftPanel")]
        [SerializeField] internal WeaponEnforceList weaponEnforceList;
        [SerializeField] internal EquipmentEnforceList equipmentEnforceList;

        [Header("Enforce UI Right Panel")]
        [SerializeField] internal EnforceUI_RightPanel enforceUI_RightPanel;

        [Header("Enforce Material Select List")]
        [SerializeField] internal EnforceMaterialSelectList enforceMaterialSelectList;

        [Header("Button")]
        [SerializeField] private Button closeBtn;
        [SerializeField] internal Button backBtn;

        [SerializeField] private InputHandler inputHandler;
        public void Init()
        {
            inputHandler = FindObjectOfType<InputHandler>();

            weaponEnforceList = GetComponentInChildren<WeaponEnforceList>(true);
            if (weaponEnforceList != null)
                weaponEnforceList.Init();

            equipmentEnforceList = GetComponentInChildren<EquipmentEnforceList>(true);
            if (equipmentEnforceList != null)
                equipmentEnforceList.Init();

            enforceMenuSpringBoard = GetComponentInChildren<EnforceMenuSpringBoard>(true);
            if (enforceMenuSpringBoard != null)
                enforceMenuSpringBoard.Init();

            enforceUI_RightPanel = GetComponentInChildren<EnforceUI_RightPanel>(true);
            if (enforceUI_RightPanel != null)
                enforceUI_RightPanel.Init();

            enforceMaterialSelectList = GetComponentInChildren<EnforceMaterialSelectList>(true );
            if(enforceMaterialSelectList != null)
            {
                enforceMaterialSelectList.Init();
            }

            closeBtn = UtilHelper.Find<Button>(transform, "CloseBtn");
            if (closeBtn != null)
            {
                closeBtn.onClick.AddListener(() => inputHandler.HandleMenuFlag());
            }

            userGoldText = UtilHelper.Find<TMP_Text>(transform, "UserGold/priceText");

            PlayerInventory.Instance.AddUpdateGoldText(() => UpdateGoldText());
        }
        private void OnEnable()
        {
            userGoldText.text = PlayerInventory.Instance.CurrentGold.ToString();
            if (weaponEnforceList != null)
            {
                SetActiveTrueLeftInventory(ItemType.Weapon);
                enforceMenuSpringBoard.ChangeSpringButtonColor(enforceMenuSpringBoard.weaponBtn);
            }
        }

        private void UpdateGoldText()
        {
            userGoldText.text = PlayerInventory.Instance.CurrentGold.ToString();
        }
        public void SetActiveTrueLeftInventory(ItemType itemType)
        {
            weaponEnforceList.gameObject.SetActive(false);
            equipmentEnforceList.gameObject.SetActive(false);

            if (itemType == ItemType.Weapon)
            {
                weaponEnforceList.gameObject.SetActive(true);
                return;
            }

            switch (itemType)
            {
                case ItemType.Tops:         equipmentEnforceList.SetEquipItemTypeToView(ItemType.Tops); break;
                case ItemType.Bottoms:      equipmentEnforceList.SetEquipItemTypeToView(ItemType.Bottoms); break;
                case ItemType.Gloves:       equipmentEnforceList.SetEquipItemTypeToView(ItemType.Gloves); break;
                case ItemType.Shoes:        equipmentEnforceList.SetEquipItemTypeToView(ItemType.Shoes); break;
                case ItemType.Accessory:    equipmentEnforceList.SetEquipItemTypeToView(ItemType.Accessory); break;
                case ItemType.SpecialEquip: equipmentEnforceList.SetEquipItemTypeToView(ItemType.SpecialEquip); break;
            }

            equipmentEnforceList.gameObject.SetActive(true);
        }
    }
}
