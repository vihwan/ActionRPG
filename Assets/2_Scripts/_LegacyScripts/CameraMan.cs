using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    public Transform player;

/*    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }*/

    void Update()
    {
        transform.position = player.position;
    }
}