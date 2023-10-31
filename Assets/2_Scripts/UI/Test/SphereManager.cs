using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using System;

public class SphereManager : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    Rigidbody characterRigidbody;
    [SerializeField, ReadOnly]
    float inputX;
    [SerializeField, ReadOnly]
    float inputZ;

    void Start()
    {
        Debug.Log("Start Move By UDP");
        UdpManager udp_manager = new UdpManager("127.0.0.1", 50002); //#UDP 서버 주소, 포트번호
        udp_manager.Init();
        udp_manager.SetMessageCallback(new CallbackDirection(OnDirection));
    }

    void Update()
    {
        Vector3 velocity = new Vector3(inputX / 3, 0, inputZ / 3);
        velocity *= speed;
        characterRigidbody.velocity = velocity;
    }

    void OnDirection(JsonData message)
    {
        string direction = message.direction;
        Debug.Log("CharacterMove : " + direction);
        switch (direction)
        {
            case "up":
                inputZ = 1f;
                inputX = 0f;
                break;
            case "down":
                inputZ = -1f;
                inputX = 0f;
                break;
            case "right":
                inputX = 1f;
                inputZ = 0f;
                break;
            case "left":
                inputX = -1f;
                inputZ = 0f;
                break;
        }
    }
}