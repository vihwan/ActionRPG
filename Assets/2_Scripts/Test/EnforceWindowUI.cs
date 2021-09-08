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

        [Header("Enforce Animation")]
        [SerializeField] private Animator anim;
        [SerializeField] private TMPro.TMP_Text animText;

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
                enforceMaterialSelectList.Init();

            closeBtn = UtilHelper.Find<Button>(transform, "CloseBtn");
            if (closeBtn != null)
                closeBtn.onClick.AddListener(() => inputHandler.HandleMenuFlag());

            userGoldText = UtilHelper.Find<TMP_Text>(transform, "UserGold/priceText");

            anim = GetComponent<Animator>();
            animText = UtilHelper.Find<TMP_Text>(transform, "EnforceText");
            if (animText != null) animText.gameObject.SetActive(false);

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
            enforceUI_RightPanel.gameObject.SetActive(false);

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
        public void UseEnforceGold(int money)
        {
            PlayerInventory.Instance.UseGold(money);
            UpdateGoldText();
        }
        public void PlayEnforceAnimation(bool state)
        {
            if (state)
            {
                anim.SetTrigger("Success");
                animText.text = "강화 성공";
            }
            else
            {
                anim.SetTrigger("Fail");
                animText.text = "강화 실패";
            }

        }


        #region Enforce Execute Functions

        // 강화가 실행될 때 실행되는 이벤트
        // 아래의 함수들이 들어가있어야한다.
        // 1. 강화 이펙트 (성공, 실패) 애니메이션
        // 2. 강화 골드 소모 및 업데이트
        // 3. 소재로 사용한 아이템 삭제
        // 4. WeaponEnforceList, EquipmentEnforceList Update
        // 5. enforceUI_RightPanel 정보창 업데이트
        internal void SuccessEnforce()
        {
            //강화 성공 애니메이션
            PlayEnforceAnimation(true);
            //강화 비용 사용 및 골드 텍스트 업데이트
            UseEnforceGold(EnforceManager.Instance.EnforceNeedGold);
            //재료로 사용한 아이템을 인벤토리에서 삭제
            PlayerInventory.Instance.SaveDeleteItemToInventory(enforceUI_RightPanel.SelectMaterialItem);

            //아이템 타입에 따라 해당 아이템의 능력치를 강화, 강화할 아이템 리스트 업데이트, 강화한 아이템의 오른쪽 패널 정보를 갱신
            if (enforceUI_RightPanel.SelectMaterialItem.itemType == ItemType.Weapon)
            {
                PlayerInventory.Instance.SetItemEnforceStatusItem(enforceUI_RightPanel.CurrentSelectItem, EnforceManager.Instance.EnforceRiseStatus);
                weaponEnforceList.UpdateUI();
                weaponEnforceList.CurrentSelectSlot.SetIsSelectSlot(true);
            }
            else
            {
                PlayerInventory.Instance.SetItemEnforceStatusItem(enforceUI_RightPanel.CurrentSelectItem, EnforceManager.Instance.EnforceRiseStatus);
                equipmentEnforceList.UpdateUI(enforceUI_RightPanel.SelectMaterialItem.itemType);
                weaponEnforceList.CurrentSelectSlot.SetIsSelectSlot(true);
            }
            enforceUI_RightPanel.SetRightPanel(enforceUI_RightPanel.CurrentSelectItem);
        }

        internal void FailEnforce()
        {
            //강화 실패 애니메이션
            PlayEnforceAnimation(false);
            //강화 비용 사용 및 골드 텍스트 업데이트
            UseEnforceGold(EnforceManager.Instance.EnforceNeedGold);
            //재료로 사용한 아이템을 인벤토리에서 삭제
            PlayerInventory.Instance.SaveDeleteItemToInventory(enforceUI_RightPanel.SelectMaterialItem);

            //아이템 타입에 따라 강화할 아이템 리스트 업데이트
            if (enforceUI_RightPanel.SelectMaterialItem.itemType == ItemType.Weapon)
                weaponEnforceList.UpdateUI();
            else
                equipmentEnforceList.UpdateUI(enforceUI_RightPanel.SelectMaterialItem.itemType);

            enforceUI_RightPanel.SetRightPanel(enforceUI_RightPanel.CurrentSelectItem);
        }
        #endregion
    }
}
