using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterCheck : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] AiDirector aiDirector;
    void Start()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (!aiDirector) aiDirector = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AiDirector>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Enterd");
            aiDirector.EnterRoom();
        }
    }
}
