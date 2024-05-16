using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour
{
    public GameObject room;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            
            mainCamera.transform.position = new Vector3(room.transform.position.x, room.transform.position.y, -10);
        }
    }
}
