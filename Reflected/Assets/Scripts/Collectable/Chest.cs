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
        WeightedRandomList<GameObject> temp = FindObjectOfType<LootPoolManager>().GetPowerupPool(trueDimension);
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
        int oldindex = -1;
        int weaponIndex = -1;
        for (int i = 0; i < numberOfPickablePowerups; i++)
        {                     
            if (myRarity.rarity == "Legendary")
            {
                if(i == 0)
                {
                    weaponIndex = 0;//Random.Range(0, 3);
                    Debug.Log("0: " + weaponIndex);
                }
                else
                {
                    weaponIndex = 2; // (weaponIndex + 1) % powerups.list.Count;
                    Debug.Log("!0: " + weaponIndex);
                }
                //do
                //{
                //    weaponIndex = Random.Range(0, 3); //Need a for loop here if there are suposed to be more than two powerups to pick from
                //} while (weaponIndex != oldindex);
                //GameObject weaponPowerup = powerups.GetItem(powerups.Count - 1);
                pickablePowerUps.Add(powerups.GetItem(powerups.Count - 1));
                pickablePowerUps[i].GetComponent<InteractableWeaponPowerup>().SetProperties(weaponIndex);
                for (int j = 0; j < pickablePowerUps.Count; j++)
                {
                    Debug.Log("Iteration: " + i + " with desc: " + pickablePowerUps[j].GetComponent<InteractablePowerUp>().description);
                }
                oldindex = weaponIndex;
            }
            else
            {
                pickablePowerUps.Add(powerups.GetRandomAndRemove());             
                pickablePowerUps[i].GetComponent<InteractablePowerUp>().SetProperties(myRarity);
            }
                       
        }
    }

    public List<GameObject> GetUpgradeItems()
    {
        return pickablePowerUps;
    }
}
