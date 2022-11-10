using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordUpgrade : MonoBehaviour
{
    [SerializeField] protected List<StatusEffectData> data;
    int upgradeIndex;
    protected float damage;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            other.GetComponent<IEffectable>().ApplyEffect(data[upgradeIndex], damage);
        }
    }
}
