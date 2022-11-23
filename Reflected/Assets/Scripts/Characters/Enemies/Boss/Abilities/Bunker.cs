using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunker : Ability
{
    [Header("Bunker Specifics")]

    [SerializeField] List<GameObject> junk;
    [SerializeField] Transform spawnPosition;
    [Range(0, 60)]
    [SerializeField] float radius;
    [Range(0, 60)]
    [SerializeField] float upwardsForce;
    [Range(0, 10)]
    [SerializeField] float abilityDuration;
    [Range(0, 60)]
    [SerializeField] int amountOfBolts;
    [Range(0, 5)]
    [SerializeField] float airTime;

    public override bool DoEffect()
    {
        GetComponent<Animator>().Play("Bunker Down");

        return base.DoEffect();
    }

    public void BunkerDown()
    {
        StartCoroutine(_BunkerDown());
    }

    IEnumerator _BunkerDown()
    {
        int spawnedBolts = 0;

        Boss boss = GetComponent<Boss>();

        while (spawnedBolts < amountOfBolts)
        {
            Debug.Log("Spawn bolt nr: " + (spawnedBolts + 1));

            for (int i = 0; i < 2; i++)
            {
                GameObject spawnedJunk = junk[Random.Range(0, junk.Count)];

                Bolt newBolt = Instantiate(spawnedJunk, spawnPosition.position, Quaternion.identity).GetComponent<Bolt>();

                Vector2 randomPosition = Random.insideUnitCircle * radius;
                Vector3 targetPosition = new Vector3(transform.position.x + randomPosition.x, player.transform.position.y, transform.position.z + randomPosition.y);

                Vector3 diff = targetPosition - spawnPosition.position;

                diff.y = upwardsForce;

                newBolt.SetVelocity(diff / airTime);

                //if (newBolt.UseGravity())
                //    newBolt.ShowLandPlacement(targetPosition);

            }

            spawnedBolts++;
            yield return new WaitForSeconds(abilityDuration / amountOfBolts);
        }

        //GetComponent<Animator>().Play("Bunker Up");

        yield return null;
    }
}
