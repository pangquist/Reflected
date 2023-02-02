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
        StartCoroutine(DelayedStart(0.3f));

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

    private IEnumerator DelayedStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < amount; i++)
        {
            Enemy c = Instantiate(enemyClose, holdingPoint, Quaternion.identity, parent.transform).GetComponentInChildren<Enemy>();
            c.Deactivate(holdingPoint);
            enemyCloseList.Add(c);

            Enemy r = Instantiate(enemyRange, holdingPoint, Quaternion.identity, parent.transform).GetComponentInChildren<Enemy>();
            r.Deactivate(holdingPoint);
            enemyRangeList.Add(r);

            Enemy a = Instantiate(enemyAOE, holdingPoint, Quaternion.identity, parent.transform).GetComponentInChildren<Enemy>();
            a.Deactivate(holdingPoint);
            enemyAOEList.Add(a);

            Enemy d = Instantiate(enemyDOT, holdingPoint, Quaternion.identity, parent.transform).GetComponentInChildren<Enemy>();
            d.Deactivate(holdingPoint);
            enemyDOTList.Add(d);

            Enemy e = Instantiate(enemyElite, holdingPoint, Quaternion.identity, parent.transform).GetComponentInChildren<Enemy>();
            e.Deactivate(holdingPoint);
            enemyEliteList.Add(e);
        }
    }

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
