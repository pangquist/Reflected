using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterCheck : MonoBehaviour
{
    [SerializeField] AiDirector aiDirector;
    private bool collisionDecected;
    void Start()
    {
        if (!aiDirector) aiDirector = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AiDirector>();
        collisionDecected = false;
    }

    void OnTriggerExit(Collider collision)
    {
        if (!collisionDecected && collision.gameObject.tag == "Player")
        {
            aiDirector.EnterRoom();
            collisionDecected = true;
        }
    }
}
