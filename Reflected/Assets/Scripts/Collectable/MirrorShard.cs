using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MirrorShard : MonoBehaviour, ICollectable, IBuyable
{
    public static event HandleShardCollected OnShardCollected;
    public delegate void HandleShardCollected(ItemData itemData);
    //Delagate return type and arguments must match that of the Add in the inventory script
    public ItemData mirroShardData;
    //The lines above allows for the action to be handled adding the item to the inventory
    //using ascriptable object in the collectable folder

    [SerializeField] private AudioClip audioClip;

    private void Start()
    {
        Destroy(gameObject, 45);
    }

    public void Collect()
    {
        PopUpText popUptext = PopUpTextManager.NewBasic(transform.position, "+1 " + mirroShardData.displayName);
        popUptext.Text.color = new Color(0f, 0.5f, 1f);

        GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(audioClip);

        Destroy(gameObject);
        OnShardCollected?.Invoke(mirroShardData); //?. makes sure it's not null and that there are listeners to the event
    }

    public int GetValue()
    {
        return mirroShardData.value;
    }

    public string GetDescription()
    {
        return mirroShardData.description;
    }

    public void ScalePrice(int scale)
    {
        throw new NotImplementedException();
    }

    public void ApplyOnPurchase()
    {
        PopUpText popUptext = PopUpTextManager.NewBasic(transform.position, "+1 " + mirroShardData.displayName);
        popUptext.Text.color = new Color(0f, 0.5f, 1f);

        GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(audioClip);
        OnShardCollected?.Invoke(mirroShardData); //?. makes sure it's not null and that there are listeners to the event
    }
}
