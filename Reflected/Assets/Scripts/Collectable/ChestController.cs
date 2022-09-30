using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    [SerializeField] WeightedRandomList<LootPool> lootTable;
    [SerializeField] Transform itemHolder;
    GameObject spawnedObject;
    bool hasSpawnedItem;

    public bool isOpen;
    //public Animator animator;

    private void Start()
    {
        //animator = GetComponent<Animator>();
    }

    public void OpenChest()
    {
        if (!isOpen )
        {
            isOpen = true;
            Debug.Log("Chest is now open...");
            //hasSpawnedItem = true;
            //animator.SetBool("IsOpen", isOpen);
            if (spawnedObject == null)
                SpawnItem();
            //else
            //    ShowItem();
        }
        else
        {
            //HideItem();
        }
    }

    void HideItem()
    {
        //itemHolder.localScale = Vector3.zero;
        spawnedObject.gameObject.SetActive(false);
        isOpen = false;
        //foreach(GameObject child in itemHolder)
        //{
        //    Destroy(child);
        //}
    }

    void ShowItem()
    {
        spawnedObject.gameObject.SetActive(true);
    }

    void SpawnItem()
    {
        //GameObject item = lootTable.GetRandom();
        spawnedObject = Instantiate(lootTable.GetRandom().GetItem(), itemHolder.position, itemHolder.rotation);
        spawnedObject.transform.parent = null;
        itemHolder.gameObject.SetActive(true);
    }
}
