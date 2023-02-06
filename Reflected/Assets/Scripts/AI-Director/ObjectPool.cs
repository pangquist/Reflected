using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;

public class ObjectPool : MonoBehaviour
{
    //private ObjectPool<GameObject> closePool;
    //private ObjectPool<GameObject> rangePool;
    //private ObjectPool<GameObject> aoePool;
    //private ObjectPool<GameObject> dotPool;
    //private ObjectPool<GameObject> elitePool;

    List<Enemy> enemyCloseList = new List<Enemy>();
    List<Enemy> enemyRangeList = new List<Enemy>();
    List<Enemy> enemyAOEList = new List<Enemy>();
    List<Enemy> enemyDOTList = new List<Enemy>();
    List<Enemy> enemyEliteList = new List<Enemy>();

    [SerializeField] GameObject enemyClose;
    [SerializeField] GameObject enemyRange;
    [SerializeField] GameObject enemyAOE;
    [SerializeField] GameObject enemyDOT;
    [SerializeField] GameObject enemyElite;

    GameObject parent;

    [SerializeField] int amount;
    static public Vector3 holdingPoint = new Vector3(-50, -50, -50);

    void Start()
    {
        parent = GameObject.Find("EnemyHolder");
        StartCoroutine(CreatePool(1f));

        //closePool = new ObjectPool<GameObject>(createClose(), ActivateEnemy());
        //rangePool = new ObjectPool<GameObject>(createRange(), ActivateEnemy());
        //aoePool = new ObjectPool<GameObject>(createAoe(), ActivateEnemy());
        //dotPool = new ObjectPool<GameObject>(createDot(), ActivateEnemy());
        //elitePool = new ObjectPool<GameObject>(createElite(), ActivateEnemy());
    }

    //private GameObject createClose() { return Instantiate(enemyClose); }
    //private GameObject createRange() { return Instantiate(enemyRange); }
    //private GameObject createAoe() { return Instantiate(enemyAOE); }
    //private GameObject createDot() { return Instantiate(enemyDOT); }
    //private GameObject createElite() { return Instantiate(enemyElite); }

    private IEnumerator CreatePool(float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < amount; i++)
        {
            Enemy c = Instantiate(enemyClose, holdingPoint, Quaternion.identity, parent.transform).GetComponentInChildren<Enemy>();
            enemyCloseList.Add(c);

            Enemy r = Instantiate(enemyRange, holdingPoint, Quaternion.identity, parent.transform).GetComponentInChildren<Enemy>();
            enemyRangeList.Add(r);

            Enemy a = Instantiate(enemyAOE, holdingPoint, Quaternion.identity, parent.transform).GetComponentInChildren<Enemy>();
            enemyAOEList.Add(a);

            Enemy d = Instantiate(enemyDOT, holdingPoint, Quaternion.identity, parent.transform).GetComponentInChildren<Enemy>();
            enemyDOTList.Add(d);

            Enemy e = Instantiate(enemyElite, holdingPoint, Quaternion.identity, parent.transform).GetComponentInChildren<Enemy>();
            enemyEliteList.Add(e);


            yield return new WaitForEndOfFrame();

            c.Deactivate(holdingPoint);
            r.Deactivate(holdingPoint);
            a.Deactivate(holdingPoint);
            d.Deactivate(holdingPoint);
            e.Deactivate(holdingPoint);
        }
    }

    //private void CreatePool()
    //{
    //    for (int i = 0; i < amount; i++)
    //    {
    //        Enemy c = Instantiate(enemyClose, holdingPoint, Quaternion.identity, parent.transform).GetComponentInChildren<Enemy>();
    //        enemyCloseList.Add(c);

    //        Enemy r = Instantiate(enemyRange, holdingPoint, Quaternion.identity, parent.transform).GetComponentInChildren<Enemy>();
    //        enemyRangeList.Add(r);

    //        Enemy a = Instantiate(enemyAOE, holdingPoint, Quaternion.identity, parent.transform).GetComponentInChildren<Enemy>();
    //        enemyAOEList.Add(a);

    //        Enemy d = Instantiate(enemyDOT, holdingPoint, Quaternion.identity, parent.transform).GetComponentInChildren<Enemy>();
    //        enemyDOTList.Add(d);

    //        Enemy e = Instantiate(enemyElite, holdingPoint, Quaternion.identity, parent.transform).GetComponentInChildren<Enemy>();
    //        enemyEliteList.Add(e);


    //        c.Deactivate(holdingPoint);
    //        r.Deactivate(holdingPoint);
    //        a.Deactivate(holdingPoint);
    //        d.Deactivate(holdingPoint);
    //        e.Deactivate(holdingPoint);
    //    }
    //}

    public void ActivateEnemy(string enemyToSpawn, Transform spawnPoint, float adaptiveDifficulty)
    {
        if(enemyToSpawn == "close")
        {
            foreach (Enemy enemy in enemyCloseList)
            {
                if (enemy.isActive()) continue;

                enemy.Activate(spawnPoint.position, adaptiveDifficulty);
                return;
            }
        }
        if (enemyToSpawn == "range")
        {
            foreach (Enemy enemy in enemyRangeList)
            {
                if (enemy.isActive()) continue;

                enemy.Activate(spawnPoint.position, adaptiveDifficulty);
                return;
            }
        }
        if (enemyToSpawn == "DOT")
        {
            foreach (Enemy enemy in enemyDOTList)
            {
                if (enemy.isActive()) continue;

                enemy.Activate(spawnPoint.position, adaptiveDifficulty);
                return;
            }
        }
        if (enemyToSpawn == "AOE")
        {
            foreach (Enemy enemy in enemyAOEList)
            {
                if (enemy.isActive()) continue;

                enemy.Activate(spawnPoint.position, adaptiveDifficulty);
                return;
            }
        }
        if (enemyToSpawn == "elite")
        {
            foreach (Enemy enemy in enemyEliteList)
            {
                if (enemy.isActive()) continue;

                enemy.Activate(spawnPoint.position, adaptiveDifficulty);
                return;
            }
        }
    }
}
