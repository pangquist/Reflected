using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chest : MonoBehaviour
{
    [SerializeField] protected WeightedRandomList<GameObject> powerups;
    //[SerializeField] protected WeightedRandomList<Rarity> rarityTiers;
    [SerializeField]  List<GameObject> pickablePowerUps;
    [SerializeField] protected Transform itemHolder;
    [SerializeField] protected Rarity myRarity;
    [SerializeField] bool trueDimension;

    protected GameObject spawnedObject;
    protected GameObject chestPrefab;    
    protected int numberOfPickablePowerups = 2;

    public bool isOpen;
    public Animator animator;
 
    protected virtual void Start()
    {
        trueDimension = DimensionManager.True;
        WeightedRandomList<GameObject> temp;
        if(myRarity.name == "Legendary")
        {
            temp = FindObjectOfType<LootPoolManager>().GetWeaponPowerupPool();
        }
        else
        {
            temp = FindObjectOfType<LootPoolManager>().GetPowerupPool(trueDimension);
        }
        
        powerups = new WeightedRandomList<GameObject>();
        for (int i = 0; i < temp.list.Count; i++)
        {
            powerups.Add(temp.list[i].item, temp.list[i].weight);
        }
        
        animator = GetComponentInChildren<Animator>();
        SetItems();
    }

    protected void Update()
    {
        if (isOpen)
        {
            animator.SetTrigger("open");
        }
        else
        {
            animator.SetTrigger("close");
        }
    }

    public abstract void OpenChest(int index);

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

    protected bool IsOpen() //Not used yet
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("ChestOpen");
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

    public void SpawnItem(int index)
    {
        spawnedObject = Instantiate(pickablePowerUps[index], itemHolder.position, itemHolder.rotation);
        spawnedObject.GetComponent<InteractablePowerUp>().SetProperties(myRarity);
        spawnedObject.transform.parent = null;
        itemHolder.gameObject.SetActive(true);
    }

    public List<GameObject> GetPickablePowerups()
    {
        return pickablePowerUps;
    }

    protected void SetItems()
    {
        for (int i = 0; i < numberOfPickablePowerups; i++)
        {
            pickablePowerUps.Add(powerups.GetRandomAndRemove());
            if (myRarity.name == "Legendary")
            {
                pickablePowerUps[i].GetComponent<InteractableWeaponPowerup>().SetProperties();
            }
            else
            {
                pickablePowerUps[i].GetComponent<InteractablePowerUp>().SetProperties(myRarity);
            }
            

        }
    }

    public List<GameObject> GetUpgradeItems()
    {
        return pickablePowerUps;
    }
}
