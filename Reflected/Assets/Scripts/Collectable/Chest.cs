using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chest : MonoBehaviour
{
    [SerializeField] protected WeightedRandomList<LootPool> lootTable;
    [SerializeField] protected List<GameObject> pickablePowerUps;
    [SerializeField] protected Transform itemHolder;
    protected GameObject spawnedObject;
    protected int numberOfPickablePowerups = 3;

    public bool isOpen;
    //public Animator animator;

    protected void Start()
    {
        //animator = GetComponent<Animator>();
    }

    public abstract void OpenChest(GameObject gameObject);

    public virtual void UseChest(GameObject gameObject)
    {
        if (!isOpen)
        {
            OpenChest(gameObject);
        }
        else
        {
            //PickItem();
        }
    }

    void HideItem()
    {
        //itemHolder.localScale = Vector3.zero;
        spawnedObject.gameObject.SetActive(false);
        isOpen = false;
    }

    void ShowItem()
    {
        spawnedObject.gameObject.SetActive(true);
    }

    protected void SpawnItem(int index)
    {
        if(index == -1)
        {
            spawnedObject = Instantiate(pickablePowerUps[index], itemHolder.position, itemHolder.rotation);
            spawnedObject.transform.parent = null;
            itemHolder.gameObject.SetActive(true);
        }        
    }

    protected void SetItems()
    {
        LootPool rarityOfPowerUps = lootTable.GetRandom();
        for (int i = 0; i < numberOfPickablePowerups; i++)
        {
            pickablePowerUps.Add(rarityOfPowerUps.GetItem());
        }
    }
}
