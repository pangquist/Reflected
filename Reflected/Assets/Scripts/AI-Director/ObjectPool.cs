using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    private ObjectPool<GameObject> closePool;
    private ObjectPool<GameObject> rangePool;
    private ObjectPool<GameObject> aoePool;
    private ObjectPool<GameObject> dotPool;
    private ObjectPool<GameObject> elitePool;

    //List<GameObject> enemyCloseList = new List<GameObject>();
    //List<GameObject> enemyRangeList = new List<GameObject>();
    //List<GameObject> enemyAOEList = new List<GameObject>();
    //List<GameObject> enemyDOTList = new List<GameObject>();
    //List<GameObject> enemyEliteList = new List<GameObject>();

    [SerializeField] GameObject enemyClose;
    [SerializeField] GameObject enemyRange;
    [SerializeField] GameObject enemyAOE;
    [SerializeField] GameObject enemyDOT;
    [SerializeField] GameObject enemyElite;

    [SerializeField] int amount;
    static public Vector3 holdingPoint = new Vector3(-50, -50, -50);

    void Start()
    {
        closePool = new ObjectPool<GameObject>(createClose(), ActivateEnemy());
        rangePool = new ObjectPool<GameObject>(createRange(), ActivateEnemy());
        aoePool = new ObjectPool<GameObject>(createAoe(), ActivateEnemy());
        dotPool = new ObjectPool<GameObject>(createDot(), ActivateEnemy());
        elitePool = new ObjectPool<GameObject>(createElite(), ActivateEnemy());
    }

    private GameObject createClose() { return Instantiate(enemyClose); }
    private GameObject createRange() { return Instantiate(enemyRange); }
    private GameObject createAoe() { return Instantiate(enemyAOE); }
    private GameObject createDot() { return Instantiate(enemyDOT); }
    private GameObject createElite() { return Instantiate(enemyElite); }



    //private void CreatePool()
    //{
    //    for (int i = 0; i < amount; i++)
    //    {
    //        //GameObject c = Instantiate(enemyClose, holdingPoint, Quaternion.Euler(0, 0, 0));
    //        //c.GetComponent<Enemy>().Deactivate(holdingPoint);
    //        //enemyCloseList.Add(c);
    //        //GameObject r = Instantiate(enemyRange, holdingPoint, Quaternion.Euler(0, 0, 0));
    //        //r.GetComponent<Enemy>().Deactivate(holdingPoint);
    //        //enemyRangeList.Add(r);
    //        //GameObject a = Instantiate(enemyAOE, holdingPoint, Quaternion.Euler(0, 0, 0));
    //        //a.GetComponent<Enemy>().Deactivate(holdingPoint);
    //        //enemyAOEList.Add(a);
    //        //GameObject d = Instantiate(enemyDOT, holdingPoint, Quaternion.Euler(0, 0, 0));
    //        //d.GetComponent<Enemy>().Deactivate(holdingPoint);
    //        //enemyDOTList.Add(d);
    //        //GameObject e = Instantiate(enemyElite, holdingPoint, Quaternion.Euler(0, 0, 0));
    //        //e.GetComponent<Enemy>().Deactivate(holdingPoint);
    //        //enemyEliteList.Add(e);

    //        //enemyCloseList[i].GetComponent<Enemy>().Deactivate(holdingPoint);
    //        //enemyCloseList[i].SetActive(false);
    //        //enemyRangeList[i].GetComponent<Enemy>().Deactivate(holdingPoint);
    //        //enemyRangeList[i].SetActive(false);
    //        //enemyAOEList[i].GetComponent<Enemy>().Deactivate(holdingPoint);
    //        //enemyAOEList[i].SetActive(false);
    //        //enemyDOTList[i].GetComponent<Enemy>().Deactivate(holdingPoint);
    //        //enemyDOTList[i].SetActive(false);
    //        //enemyEliteList[i].GetComponent<Enemy>().Deactivate(holdingPoint);
    //        //enemyEliteList[i].SetActive(false);
    //    }
    //}

    public void ActivateEnemy(Transform spawnPoint, float adaptiveDifficulty)
    {
        gameObject.GetComponent<Enemy>().Activate(spawnPoint.position, adaptiveDifficulty);
    }
}
