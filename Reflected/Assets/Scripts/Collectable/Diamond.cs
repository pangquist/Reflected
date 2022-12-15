using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour, ICollectable, IBuyable
{
    public static event HandleDiamondCollected OnDiamondCollected;
    public delegate void HandleDiamondCollected(ItemData itemData);
    //Delagate return type and arguments must match that of the Add in the inventory script
    public ItemData diamondData;
    //The lines above allows for the action to be handled adding the item to the inventory
    //using ascriptable object in the collectable folder

    [SerializeField] private AudioClip audioClip;

    private void Start()
    {
        //Destroy(gameObject, 30);
    }

    public void Collect()
    {
        PopUpText popUptext = PopUpTextManager.NewBasic(transform.position, "+1 Diamond");
        popUptext.Text.color = new Color(0f, 0.2f, 1f);

        GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(audioClip);

        Destroy(gameObject);
        OnDiamondCollected?.Invoke(diamondData);
    }

    public int GetValue()
    {
        return diamondData.value;
    }

    public string GetDescription()
    {
        return diamondData.description;
    }

    public void ScalePrice(int scale)
    {
        throw new System.NotImplementedException();
    }

}
