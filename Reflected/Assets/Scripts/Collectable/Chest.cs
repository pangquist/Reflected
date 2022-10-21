using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chest : MonoBehaviour
{
    //[SerializeField] protected WeightedRandomList<LootPool> lootTable;
    [SerializeField] protected WeightedRandomList<GameObject> powerups;
    [SerializeField] protected WeightedRandomList<Rarity> rarityTiers;
    [SerializeField] protected List<GameObject> pickablePowerUps;
    [SerializeField] protected Transform itemHolder;
    protected GameObject spawnedObject;
    //protected GameObject itemToSpawn;
    [SerializeField] protected Rarity myRarity;
    protected int numberOfPickablePowerups = 3;

    public bool isOpen;
    //public Animator animator;

    protected virtual void Start()
    {
        //animator = GetComponent<Animator>();
        powerups = FindObjectOfType<LootPoolManager>().GetPowerupPool();
        myRarity = FindObjectOfType<LootPoolManager>().GetRandomRarity();
        SetItems();
        //itemToSpawn = lootTable.GetRandom().GetItem();
    }

    public abstract void OpenChest();

    public virtual void UseChest(GameObject gameObject)
    {
        if (!isOpen)
        {
            //OpenChest(gameObject);
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
        spawnedObject = Instantiate(pickablePowerUps[index], itemHolder.position, itemHolder.rotation);
        spawnedObject.GetComponent<InteractablePowerUp>().SetProperties(myRarity);
        //spawnedObject = Instantiate(itemToSpawn, itemHolder.position, itemHolder.rotation);
        spawnedObject.transform.parent = null;
        itemHolder.gameObject.SetActive(true);
    }

    protected void SetItems()
    {
        //LootPool rarityOfPowerUps = lootTable.GetRandom();
        for (int i = 0; i < numberOfPickablePowerups; i++)
        {
            //pickablePowerUps.Add(rarityOfPowerUps.GetItem());
            pickablePowerUps.Add(powerups.GetRandom());
            pickablePowerUps[i].GetComponent<InteractablePowerUp>().SetProperties(myRarity);            
        }
        //Debug.Log("Chest" + pickablePowerUps[0].GetComponent<InteractablePowerUp>().myRarity);
        //Debug.Log("Chest" + pickablePowerUps[0].GetComponent<InteractablePowerUp>().amount);
        //Debug.Log("Number of powerups to choose from is " + pickablePowerUps.Count);
    }
}
