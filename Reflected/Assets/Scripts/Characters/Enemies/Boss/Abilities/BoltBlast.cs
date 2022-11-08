using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltBlast : Ability
{
    [SerializeField] Transform spawnPosition;
    [SerializeField] float abilityDuration;
    [SerializeField] int amountOfBolts;
    [SerializeField] GameObject bolt;

    [SerializeField] List<GameObject> bolts;
    public override bool DoEffect()
    {
        SpawnBolts();

        return base.DoEffect();
    }

    public void SpawnBolts()
    {
        StartCoroutine(_SpawnBolts());
    }

    IEnumerator _SpawnBolts()
    {
        int spawnedBolts = 0;

        while(spawnedBolts < amountOfBolts)
        {
            Debug.Log("Spawn bolt nr: " + (spawnedBolts+1));
            Instantiate(bolt, spawnPosition.position, Quaternion.identity);
            spawnedBolts++;
            yield return new WaitForSeconds(abilityDuration / amountOfBolts);
        }

        yield return 0;
    }
}
