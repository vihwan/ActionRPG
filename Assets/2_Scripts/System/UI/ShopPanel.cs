using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SG
{
    public class ShopPanel : MonoBehaviour
    {
        [Header("Main Contents")]
        private TMP_Text titleText;
        private TMP_Text userGoldText;
        private Transform shopListSlotCreateTransform;
        private ShopListSlot shopListSlotPrefab;
        private List<ShopListSlot> shopListSlots = new List<ShopListSlot>();

        [Header("Item Info Panel")]
        [SerializeField] internal Shop_InfoPanel infoPanel;


        [Header("CloseBtn")]
        private Button closeBtn;

        [Header("Need Component"), HideInInspector]
        private InputHandler inputHandler;
        private PlayerInventory playerInventory;

        public void Init()
        {
            inputHandler = FindObjectOfType<InputHandler>();

            Transform t = transform.Find("Main Contents");
            titleText = UtilHelper.Find<TMP_Text>(t, "TitleText");
            userGoldText = UtilHelper.Find<TMP_Text>(t, "UserGold/priceText");
            shopListSlotCreateTransform = t.Find("ItemList/Scroll View/Viewport/Content").transform;
            shopListSlotPrefab = Database.it.prefabDatabase.shopListSlot;

            infoPanel = GetComponentInChildren<Shop_InfoPanel>(true);
            if (infoPanel != null)
                infoPanel.Init();

            closeBtn = UtilHelper.Find<Button>(transform, "CloseBtn");
            if (closeBtn != null)
                closeBtn.onClick.AddListener(() => inputHandler.HandleMenuFlag()); //닫기 기능 수행


            playerInventory = inputHandler.GetComponent<PlayerInventory>();
            playerInventory.AddUpdateGoldText(() => UpdateUserGoldText());
            CloseShopPanel();
        }
        public void SetShopPanel(string shopName)
        {
            titleText.text = string.Format("상점 <size=28> / {0}", shopName);
            userGoldText.text = PlayerInventory.Instance.CurrentGold.ToString();
        }

        public void UpdateUserGoldText()
        {
            userGoldText.text = PlayerInventory.Instance.CurrentGold.ToString();
        }
        public void CreateItemList(List<Item> itemsList)
        {
            //외부로 부터 NPC가 가지고 있는 Item의 List를 받아와서, 그 아이템 슬롯을 스크롤뷰 안에 생성한다.
            ClearItemList();

            for (int i = 0; i < itemsList.Count; i++)
            {
                ShopListSlot slot = Instantiate(shopListSlotPrefab, shopListSlotCreateTransform);
                slot.SetSlot(itemsList[i]);
                shopListSlots.Add(slot);
            }

            //첫번째 슬롯 아이템 정보 보이게 하기
            if(shopListSlots[0] != null)
                shopListSlots[0].OnClickSlotBtn();
        }
        public void ClearItemList()
        {
            if (shopListSlots.Count > 0)
            {
                for (int i = 0; i < shopListSlotCreateTransform.childCount; i++)
                {
                    Destroy(shopListSlotCreateTransform.GetChild(i).gameObject);
                }
            }
            shopListSlots.Clear();
        }

        public void SetActiveShopPanel(bool status)
        {
            this.gameObject.SetActive(status);
        }

        public void CloseShopPanel()
        {
            this.gameObject.SetActive(false);
        }
    }
}
