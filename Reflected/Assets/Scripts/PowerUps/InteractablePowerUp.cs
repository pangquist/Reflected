using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePowerUp : MonoBehaviour
{
    public PowerUpEffect powerUpEffect;
    public WeightedRandomList<PowerUpEffect> RarityPool;

    private void Start()
    {
        powerUpEffect = RarityPool.GetRandom();
    }

    public void ApplyOnInteraction()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Destroy(gameObject);
        powerUpEffect.Apply(player.gameObject);
    }
}
