using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public WeightedRandomList<Transform> lootTable;
    public Transform itemHolder;

    public bool isOpen;
    //public Animator animator;

    private void Start()
    {
        //animator = GetComponent<Animator>();
    }

    public void OpenChest()
    {
        if (!isOpen)
        {
            isOpen = true;
            Debug.Log("Chest is now open...");
            //animator.SetBool("IsOpen", isOpen);
            ShowItem();
        }
        else
        {
            HideItem();
        }
    }

    void HideItem()
    {
        itemHolder.localScale = Vector3.zero;
        itemHolder.gameObject.SetActive(false);

        foreach(Transform child in itemHolder)
        {
            Destroy(child.gameObject);
        }
    }

    void ShowItem()
    {
        Transform item = lootTable.GetRandom();
        Instantiate(item, itemHolder);
        itemHolder.gameObject.SetActive(true);
    }
}
