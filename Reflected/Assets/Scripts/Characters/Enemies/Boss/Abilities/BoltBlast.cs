using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltBlast : Ability
{
    [Header("Bolt Blast Specifics")]

    [SerializeField] GameObject bolt;
    [SerializeField] Transform spawnPosition;
    [Range(0, 10)]
    [SerializeField] float offsetRange;
    [Range(0, 5)]
    [SerializeField] float abilityDuration;
    [Range (0, 20)]
    [SerializeField] int amountOfBolts;
    [Range(0, 4)]
    [SerializeField] float airTime;

    //List<GameObject> bolts;
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
            Bolt newBolt = Instantiate(bolt, spawnPosition.position, Quaternion.identity).GetComponent<Bolt>();

            player = GameObject.FindWithTag("Player").GetComponent<Player>();

            float randomOffset = Random.Range(-offsetRange, offsetRange + 1);

            Vector3 diff = new Vector3(
                player.transform.position.x + randomOffset,
                player.transform.position.y,
                player.transform.position.z + randomOffset
                ) - spawnPosition.position;

            newBolt.SetVelocity(diff / airTime);

            spawnedBolts++;
            yield return new WaitForSeconds(abilityDuration / amountOfBolts);
        }

        yield return 0;
    }
}
