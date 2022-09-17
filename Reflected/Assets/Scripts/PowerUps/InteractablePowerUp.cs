using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePowerUp : MonoBehaviour
{
    public PowerUpEffect powerUpEffect;

    public void ApplyOnInteraction()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Destroy(gameObject);
        powerUpEffect.Apply(player.gameObject);
    }
}
