using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    List<GameObject> enemyCloseList = new List<GameObject>();
    List<GameObject> enemyRangeList = new List<GameObject>();
    List<GameObject> enemyAOEList = new List<GameObject>();
    List<GameObject> enemyDOTList = new List<GameObject>();
    List<GameObject> enemyEliteList = new List<GameObject>();

    [SerializeField] GameObject enemyClose;
    [SerializeField] GameObject enemyRange;
    [SerializeField] GameObject enemyAOE;
    [SerializeField] GameObject enemyDOT;
    [SerializeField] GameObject enemyElite;

    [SerializeField] int amount;
    static public Vector3 holdingPoint = new Vector3(-50, -50, -50);

    void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < amount; i++)
        {
            enemyCloseList.Add(Instantiate(enemyClose, holdingPoint, Quaternion.Euler(0, 0, 0)));
            enemyRangeList.Add(Instantiate(enemyRange, holdingPoint, Quaternion.Euler(0, 0, 0)));
            enemyAOEList.Add(Instantiate(enemyAOE, holdingPoint, Quaternion.Euler(0, 0, 0)));
            enemyDOTList.Add(Instantiate(enemyDOT, holdingPoint, Quaternion.Euler(0, 0, 0)));
            enemyEliteList.Add(Instantiate(enemyElite, holdingPoint, Quaternion.Euler(0, 0, 0)));

            enemyCloseList[i].GetComponent<Enemy>().Deactivate(holdingPoint);
            enemyCloseList[i].SetActive(false);
            enemyRangeList[i].GetComponent<Enemy>().Deactivate(holdingPoint);
            enemyRangeList[i].SetActive(false);
            enemyAOEList[i].GetComponent<Enemy>().Deactivate(holdingPoint);
            enemyAOEList[i].SetActive(false);
            enemyDOTList[i].GetComponent<Enemy>().Deactivate(holdingPoint);
            enemyDOTList[i].SetActive(false);
            enemyEliteList[i].GetComponent<Enemy>().Deactivate(holdingPoint);
            enemyEliteList[i].SetActive(false);
        }
    }

    public void ActivateEnemy(string enemyType, Transform spawnPoint, float adaptiveDifficulty)
    {
        if (enemyType == "close")
        {
            foreach (GameObject e in enemyCloseList)
            {
                if (!e.GetComponent<Enemy>().isActive())
                {
                    e.GetComponent<Enemy>().Activate(spawnPoint.position, adaptiveDifficulty);
                    return;
                }
            }
        }
        if (enemyType == "range")
        {
            foreach (GameObject e in enemyRangeList)
            {
                if (!e.GetComponent<Enemy>().isActive())
                {
                    e.GetComponent<Enemy>().Activate(spawnPoint.position, adaptiveDifficulty);
                    return;
                }
            }
        }
        if (enemyType == "AOE")
        {
            foreach (GameObject e in enemyAOEList)
            {
                if (!e.GetComponent<Enemy>().isActive())
                {
                    e.GetComponent<Enemy>().Activate(spawnPoint.position, adaptiveDifficulty);
                    return;
                }
            }
        }
        if (enemyType == "DOT")
        {
            foreach (GameObject e in enemyDOTList)
            {
                if (!e.GetComponent<Enemy>().isActive())
                {
                    e.GetComponent<Enemy>().Activate(spawnPoint.position, adaptiveDifficulty);
                    return;
                }
            }
        }
        if (enemyType == "elite")
        {
            foreach (GameObject e in enemyEliteList)
            {
                if (!e.GetComponent<Enemy>().isActive())
                {
                    e.GetComponent<Enemy>().Activate(spawnPoint.position, adaptiveDifficulty);
                    return;
                }
            }
        }
    }
}
