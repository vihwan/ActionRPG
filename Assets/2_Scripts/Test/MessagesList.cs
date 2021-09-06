using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class MessagesList : MonoBehaviour
    {
        [SerializeField] public List<GameObject> getMessages_List = new List<GameObject>();
        [SerializeField] public Transform CreateMessageTransform;

        public void Init()
        {
            CreateMessageTransform = transform.Find("CreateTransform").transform;
        }

        public void LocateNewMessage(GameObject gameObject)
        {
            if (getMessages_List.Count > 0)
            {
                foreach (GameObject go in getMessages_List)
                {
                    go.GetComponent<Message_GetItem>().targetPosition += new Vector3(0f, 90f , 0f);
                    go.GetComponent<Message_GetItem>().canMove = true;
                }
            }
            gameObject.transform.position = CreateMessageTransform.position;
            gameObject.GetComponent<Message_GetItem>().targetPosition = gameObject.transform.position;
            getMessages_List.Add(gameObject);

            if (getMessages_List.Count > 4)
            {
                PopUpGenerator.Instance.ReturnObjectToPool(getMessages_List[0]);          
            }
        }


    }
}
