using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coin : MonoBehaviour, ICollectable, IMagnetic
{
    public static event HandleCoinCollected OnCoinCollected;
    public delegate void HandleCoinCollected(ItemData itemData);
    //Delagate return type and arguments must match that of the Add in the inventory script
    public ItemData coinData;
    //The lines above allows for the action to be handled adding the item to the inventory
    //using ascriptable object in the collectable folder

    Rigidbody rb;
    bool hasTarget;
    Vector3 targetPosition;
    float moveSpeed = 5f;

    [SerializeField] private AudioClip audioClip;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();        
    }

    private void Start()
    {
        //coinData.amount = UnityEngine.Random.Range(1, 5);
        //coinData.value = coinData.amount;
        Destroy(gameObject, 20);
    }

    public void Collect()
    {
        PopUpText popUptext = PopUpTextManager.NewBasic(transform.position, "+1 Coin");
        popUptext.Text.color = new Color(1f, 0.9f, 0f);

        GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(audioClip);

        Destroy(gameObject);
        OnCoinCollected?.Invoke(coinData);
        gameObject.SetActive(false);
    }

    public void FixedUpdate()
    {
        if (hasTarget)
        {
            Vector3 targetDirection = (targetPosition - transform.position).normalized;
            rb.velocity = new Vector3(targetDirection.x, targetDirection.y, targetDirection.z) * moveSpeed;
            //moveSpeed += Time.deltaTime * 1;
        }
    }

    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
        hasTarget = true;
    }

}
