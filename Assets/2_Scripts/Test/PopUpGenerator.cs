using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class PopUpGenerator : MonoBehaviour
    {
        public static PopUpGenerator Instance;

        [Header("Prefab")]
        public GameObject popUp_GetItem_Prefab; //에디터 지정
        public GameObject popUpMessage_Prefab;
        public GameObject popUp_MultiSelection_Prefab;
        [SerializeField] private Sprite goldImage;

        [Header("ObjectPool")]
        Queue<GameObject> messageGetItem_OP = new Queue<GameObject>();

        [Header("Saver Transform")]
        public Transform popUp_GetItem_Transform; //에디터 지정

        [Header("Component")]
        [SerializeField] internal MessagesList messagesList;

        public void Init()
        {
            Instance = this;

            messagesList = GetComponentInChildren<MessagesList>(true);
            if (messagesList != null)
                messagesList.Init();

            MakeObjectPoolObject(10);

            popUp_GetItem_Prefab = Resources.Load<GameObject>("Prefabs/PopupGetItem");
            popUpMessage_Prefab = Resources.Load<GameObject>("Prefabs/PopUpMsg");
            popUp_MultiSelection_Prefab = Resources.Load<GameObject>("Prefabs/MultiSelectionPopup");
            goldImage = Resources.Load<Sprite>("Sprites/Gold");
        }

        #region ObjectPool
        private void MakeObjectPoolObject(int count)
        {
            for (int i = 0; i < count; i++)
            {
                messageGetItem_OP.Enqueue(CreateNewObject());
            }
        }

        private GameObject CreateNewObject()
        {
            GameObject newobj = Instantiate(popUp_GetItem_Prefab,
                                     transform.position,
                                     Quaternion.identity);
            newobj.SetActive(false);
            newobj.transform.SetParent(popUp_GetItem_Transform);
            return newobj;
        }

        public GameObject GetMessageGetGold(int amount)
        {
            //오브젝트 풀에 잔여 오브젝트가 있으면
            //그대로 가져다 쓰기
            if (Instance.messageGetItem_OP.Count > 0)
            {
                var obj = Instance.messageGetItem_OP.Dequeue();
                obj.gameObject.SetActive(true);
                obj.GetComponent<Message_GetItem>().SetGetGoldMessage(goldImage, amount);
                obj.transform.SetParent(messagesList.CreateMessageTransform);
                messagesList.LocateNewMessage(obj);
                return obj;
            }
            else
            //없으면 새로 만들어서 가져다쓰기
            {
                var newObj = Instance.CreateNewObject();
                newObj.gameObject.SetActive(true);
                newObj.GetComponent<Message_GetItem>().SetGetGoldMessage(goldImage, amount);
                newObj.transform.SetParent(messagesList.CreateMessageTransform);
                messagesList.LocateNewMessage(newObj);
                return newObj;
            }
        }

        public GameObject GetMessageGetItemObject(Item item, int amount)
        {
            //오브젝트 풀에 잔여 오브젝트가 있으면
            //그대로 가져다 쓰기
            if (Instance.messageGetItem_OP.Count > 0)
            {
                var obj = Instance.messageGetItem_OP.Dequeue();
                obj.gameObject.SetActive(true);
                obj.GetComponent<Message_GetItem>().SetGetItemMessage(item, amount);
                obj.transform.SetParent(messagesList.CreateMessageTransform);
                messagesList.LocateNewMessage(obj);
                return obj;
            }
            else
            //없으면 새로 만들어서 가져다쓰기
            {
                var newObj = Instance.CreateNewObject();
                newObj.gameObject.SetActive(true);
                newObj.GetComponent<Message_GetItem>().SetGetItemMessage(item, amount);
                newObj.transform.SetParent(messagesList.CreateMessageTransform);
                messagesList.LocateNewMessage(newObj);
                return newObj;
            }
        }
        public void ReturnObjectToPool(GameObject obj)
        {
            for (int i = 0; i < messagesList.getMessages_List.Count; i++)
            {
                if (messagesList.getMessages_List[i] == obj)
                {
                    messagesList.getMessages_List.RemoveAt(i);
                    obj.gameObject.SetActive(false);
                    obj.transform.SetParent(Instance.popUp_GetItem_Transform);
                    Instance.messageGetItem_OP.Enqueue(obj);
                    break;
                }
            }
        }
        #endregion


        //소비, 재료용 다중선택 팝업
        public PopUpMultiSelection CreatePopupMultiSelection(Transform parent, Item item, ItemType itemType, string tradeType = "판매")
        {
            GameObject popupObj = Instantiate(popUp_MultiSelection_Prefab, parent, false);
            popupObj.SetActive(true);

            PopUpMultiSelection popUpMulti = popupObj.GetComponent<PopUpMultiSelection>();

            switch (itemType)
            {
                case ItemType.Tops: popUpMulti.SetOpenAmountInputPopup(item as EquipItem, tradeType);  break;
                case ItemType.Bottoms:  goto case ItemType.Tops;
                case ItemType.Gloves:   goto case ItemType.Tops;
                case ItemType.Shoes:  goto case ItemType.Tops;
                case ItemType.Accessory:   goto case ItemType.Tops;
                case ItemType.SpecialEquip:    goto case ItemType.Tops;
                case ItemType.Weapon:   popUpMulti.SetOpenAmountInputPopup(item as WeaponItem, tradeType);   break;
                case ItemType.Consumable:   popUpMulti.SetOpenAmountInputPopup(item as ConsumableItem, tradeType);       break;
                case ItemType.Ingredient:    popUpMulti.SetOpenAmountInputPopup(item as IngredientItem, tradeType);     break;
                default:     Debug.LogWarning("버그남");     break;
            }

            return popUpMulti;
        }

        //소비, 재료용 함수
        public PopUpMessage CreatePopupMessage(Transform parent, string title, string yesText, string noText, string addText = null)
        {
            GameObject popupObj = Instantiate(popUpMessage_Prefab, parent, false);
            popupObj.SetActive(true);

            PopUpMessage popUpMessage = popupObj.GetComponent<PopUpMessage>();
            popUpMessage.SetOpenMessagePopup(title, yesText, noText, addText);

            return popUpMessage;
        }
    }
}
