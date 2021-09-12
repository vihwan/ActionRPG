using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    //플레이어의 인벤토리를 관리하고 출력하는 패널 스크립트
    // 무기, 소비, 가방 등 여러 인벤토리 창을 관리하는 스크립트입니다.
    public class WindowPanel : MonoBehaviour
    {
        [Header("Window GameObject")]
        [SerializeField] internal CharacterWindowUI characterWindowUI;
        [SerializeField] internal InventoryWindowUI inventoryWindowUI;
        [SerializeField] internal EnforceWindowUI enforceWindowUI;

        public void Init()
        {

            characterWindowUI = UtilHelper.Find<CharacterWindowUI>(transform, "Character Window");
            if (characterWindowUI != null)
            {
                characterWindowUI.Init();
                characterWindowUI.gameObject.SetActive(false);
            }

            inventoryWindowUI = UtilHelper.Find<InventoryWindowUI>(transform, "Inventory Window");
            if(inventoryWindowUI != null)
            {
                inventoryWindowUI.Init();
                inventoryWindowUI.gameObject.SetActive(false);
            }

            enforceWindowUI = UtilHelper.Find<EnforceWindowUI>(transform, "Enforce Window");
            if (enforceWindowUI != null)
            {
                enforceWindowUI.Init();
                enforceWindowUI.gameObject.SetActive(false);
            }
        }

        #region Panel Window Controls
        public void OpenCharacterWindowPanel()
        {
            characterWindowUI.gameObject.SetActive(true);
            characterWindowUI.OpenStatusPanel();
        }

        public void CloseCharacterWindowPanel()
        {
            characterWindowUI.gameObject.SetActive(false);
        }

        public void OpenInventoryWindowPanel()
        {
            inventoryWindowUI.gameObject.SetActive(true);
            inventoryWindowUI.OnOpenPanel();
        }

        public void CloseInventoryWindowPanel()
        {
            inventoryWindowUI.OnClosePanel();
            inventoryWindowUI.gameObject.SetActive(false);
        }

        public void OpenEnforceWindowPanel()
        {
            enforceWindowUI.OnOpenPanel();
            enforceWindowUI.gameObject.SetActive(true);
        }

        public void CloseEnforceWindowPanel()
        {
            enforceWindowUI.gameObject.SetActive(false);
        }

        #endregion
    }
}

