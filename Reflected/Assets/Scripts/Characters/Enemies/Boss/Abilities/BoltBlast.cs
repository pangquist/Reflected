using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltBlast : Ability
{
    [Header("Bolt Blast Specifics")]

    [SerializeField] GameObject bolt;
    [SerializeField] List<Transform> spawnPositions;
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
        GetComponent<Animator>().Play("Bolt Blaster");

        return base.DoEffect();
    }

    public void SpawnBolts()
    {
        StartCoroutine(_SpawnBolts());
    }

    IEnumerator _SpawnBolts()
    {
        int spawnedBolts = 0;
        int spawnPositionIndex = 0;

        while(spawnedBolts < amountOfBolts)
        {
            Debug.Log("Spawn bolt nr: " + (spawnedBolts+1));

            Vector3 spawnPosition = spawnPositions[spawnPositionIndex++].position;
            if (spawnPositionIndex == spawnPositions.Count)
                spawnPositionIndex = 0;

            Bolt newBolt = Instantiate(bolt, spawnPosition, Quaternion.identity).GetComponentInChildren<Bolt>();
            Transform parent = newBolt.gameObject.transform.parent.transform;

            player = FindObjectOfType<Player>().GetComponent<Player>();

            float randomOffset = Random.Range(-offsetRange, offsetRange + 1);

            Vector3 landPosition = new Vector3(
                player.transform.position.x + randomOffset, 
                player.transform.position.y, 
                player.transform.position.z + randomOffset);
            
            parent.rotation = Quaternion.LookRotation(landPosition);

            Vector3 diff = landPosition - spawnPosition;

            newBolt.SetVelocity(diff / airTime);

            spawnedBolts++;
            yield return new WaitForSeconds(abilityDuration / amountOfBolts);
        }

        GetComponent<Animator>().Play("Bolt Past");

        yield return 0;
    }
}
