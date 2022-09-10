using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private Rigidbody rb;
    public int damageAmount = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.tag != "Ground")
        //{          
        //    //Causes the object to get stuck like an arrow effect on the player being burrowed in
        //    //gameObject.transform.parent = other.gameObject.transform;            
        //    //Destroy(rb);
        //    //GetComponent<Collider>().enabled = false;
        //}

        if(other.tag == "Player")
        {
            Destroy(gameObject);

            var healthComponent = other.GetComponent<Health>();
            if(healthComponent != null)
            {
                Debug.Log("damaged");
                healthComponent.TakeDamage(damageAmount);
            }
        }
    }
}
