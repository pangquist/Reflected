using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    [SerializeField] Vector3 velocity;
    [SerializeField] float gravity;
    [SerializeField] float damage;
    [SerializeField] Player player;
    [SerializeField] float airTime;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        Vector3 diff = player.transform.position - transform.position;
        diff.y = 10;

        velocity = diff / 2;
    }

    // Update is called once per frame
    void Update()
    {
        velocity.y += gravity * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;
    }

    public void SetVelocity(Vector3 newVelocity)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInChildren<Player>())
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (other.tag == "Ground" || other.tag == "Player")
            Destroy(gameObject);
    }
}
