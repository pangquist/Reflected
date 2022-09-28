using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterCheck : MonoBehaviour
{
    [SerializeField] AiDirector aiDirector;
    void Start()
    {
        if (!aiDirector) aiDirector = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AiDirector>();
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("Enterd");
            aiDirector.EnterRoom();
        }
    }
}
