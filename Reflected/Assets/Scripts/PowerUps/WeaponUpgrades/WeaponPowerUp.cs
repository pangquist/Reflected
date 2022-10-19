using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerUp : MonoBehaviour //Make one for each powerup that adds a status effect
{
    [SerializeField] StatusEffectData data;

    public void ApplyOnInteraction()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Destroy(gameObject);
        player.GetComponent<Sword>().AddStatusEffect(data);
        //target.GetComponent<Sword>().enabled = true;
    }


}
