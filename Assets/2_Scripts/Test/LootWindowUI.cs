using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;


namespace SG
{
    /*    
	- 몬스터가 드랍한 아이템의 리스트를 보여주는 UI 생성
	- 아이템 리스트는 아이템 이름, 아이콘이 포함된 패널
	- 아이템 리스트는 Grid Layout으로, 스크롤뷰 컨텐츠로 표시
	- 마우스 휠로 얻고 싶은 아이템을 고를 수 있음.
	- F키를 누르면 하나씩 아이템을 획득
	- F키를 꾹 누르면 모든 아이템을 획득
	- 아이템을 전부 획득하면 윈도우 창이 비활성화
    */
    public class LootWindowUI : MonoBehaviour
    {
        [Header("Basics")]
        [SerializeField] private GameObject currentLootItemObject;
        [SerializeField] private List<LootItemPanel> lootItemPanelList = new List<LootItemPanel>();
        [SerializeField] private Transform itemViewTransform;
        [SerializeField] private GameObject FBtnIcon;
        [SerializeField] private int currentFIconIndex;

        [Header("Prefab")]
        public GameObject LootItemPanelPrefab;

        [Header("Get Item Time Limit")]
        private float getItemLimitTime = 0f;

        public int CurrentFIconIndex
        {
            get => currentFIconIndex;
            private set
            {
                if (value < 0)
                    value = 0;

                if (value >= lootItemPanelList.Count)
                    value = lootItemPanelList.Count - 1;

                currentFIconIndex = value;
            }
        }

        public void Init()
        {
            Transform t = transform.Find("UI Background").transform;
            itemViewTransform = t.Find("Scroll View/Viewport/Content").transform;
            FBtnIcon = t.Find("F_Icon").gameObject;

            if (this.gameObject.activeSelf.Equals(true))
                this.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (this.gameObject.activeSelf.Equals(true))
            {
                getItemLimitTime += Time.deltaTime;
                SelectLootItem();
                GetLootItem();           
            }
        }
        private void LateUpdate()
        {
            SetFBtnPosition(CurrentFIconIndex);
        }
        private void SelectLootItem()
        {
            //마우스 휠을 인식하여 아이템을 고를 수 있음
            //Debug.Log("마우스 휠 상태 : " + Mouse.current.scroll.y.ReadValue());

            if (lootItemPanelList.Count > 0)
            {
                //위로 마우스 휠업을 하면, F버튼 아이콘이 상위 아이템 패널을 가리키게 된다.
                if (Mouse.current.scroll.y.ReadValue() > 0)
                {
                    CurrentFIconIndex -= 1;
                }
                else if (Mouse.current.scroll.y.ReadValue() < 0)
                {
                    CurrentFIconIndex += 1;
                }
            }
        }

        private void GetLootItem()
        {
            //F키를 누르면 F버튼 아이콘이 가리키고 있는 아이템을 얻을 수 있음.
            if (Keyboard.current.fKey.isPressed)
            {
                //꾹 눌러도 상관없지만, 그러면 너무 빨리 획득하기 때문에 시간 간격을 따로줘서 생성
                if (getItemLimitTime >= 0.4f)
                {
                    PlayerInventory.Instance.SaveGetItemToInventory(lootItemPanelList[currentFIconIndex].item,
                                                lootItemPanelList[currentFIconIndex].item.quantity);
                    Destroy(lootItemPanelList[CurrentFIconIndex].gameObject);
                    lootItemPanelList.RemoveAt(CurrentFIconIndex);

                    //획득할 아이템이 없다면 루트 윈도우 창을 닫는다.
                    if (lootItemPanelList.Count == 0)
                    {
                        EmptyLootItemObject();
                        CloseLootWindow();
                        return;
                    }

                    CurrentFIconIndex -= 1;
                    getItemLimitTime = 0f;
                }
            }
        }

        private void EmptyLootItemObject()
        {
            if (UtilHelper.HasComponent<DropItemBox>(currentLootItemObject))
                Destroy(currentLootItemObject);
        }

        public void SetLootWindow(GameObject LootItemObject, List<Item> itemList)
        {
            currentLootItemObject = LootItemObject;

            for (int i = 0; i < lootItemPanelList.Count; i++)
            {
                Destroy(lootItemPanelList[i].gameObject);
            }
            lootItemPanelList.Clear();

            if (itemList.Count > 0)
            {
                List<Item> newList = SortLootItemPanelList(itemList);

                for (int i = 0; i < newList.Count; i++)
                {
                    GameObject newGO = Instantiate(LootItemPanelPrefab, itemViewTransform);
                    LootItemPanel itemPanel = newGO.GetComponent<LootItemPanel>();
                    itemPanel.SetLootItemPanel(newList[i]);
                    lootItemPanelList.Add(itemPanel);
                }
            }
        }

        //추가 된 아이템을 무기, 장비, 소비, 재료 순으로, 또한 그 안에서도 레어도가 높은 순으로 정렬되게끔 순서를 바꿔주는 메소드
        private List<Item> SortLootItemPanelList(List<Item> items)
        {
            var newSortList = from item in items
                              orderby item.itemType, item.rarity descending
                              select item;

            return newSortList.ToList();
        }

        private void SetFBtnPosition(int currentIndex)
        {
            if (lootItemPanelList[currentIndex] == null)
                return;

            FBtnIcon.transform.position
                = new Vector2(FBtnIcon.transform.position.x, lootItemPanelList[currentIndex].gameObject.transform.position.y);
        }
        public void OpenLootWindow()
        {
            this.gameObject.SetActive(true);
            FBtnIcon.SetActive(true);
            CurrentFIconIndex = 0;
        }

        public void CloseLootWindow()
        {
            this.gameObject.SetActive(false);
        }
    }
}
