using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    [SerializeField] bool useGravity;
    [SerializeField] Vector3 velocity;
    [SerializeField] float gravity;
    [SerializeField] float damage;
    [SerializeField] Player player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        if (useGravity)
            velocity.y += gravity * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;
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

    public void SetVelocity(Vector3 newVelocity)
    {
        velocity = newVelocity;
    }
}
