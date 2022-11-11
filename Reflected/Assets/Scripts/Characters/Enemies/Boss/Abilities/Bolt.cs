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
    [SerializeField] GameObject landMarker;

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

        transform.rotation = Quaternion.LookRotation(velocity);
    }

    public void ShowLandPlacement(Vector3 spawnPosition)
    {
        float airTime = 0;

        float xPosition = transform.position.x;
        float yPosition = transform.position.y;
        float zPosition = transform.position.z;

        float tempVelocity = velocity.y;
        GameObject ground = GameObject.FindGameObjectWithTag("Ground");

        for (int i = 0; i < 200; i++)
        {
            tempVelocity += gravity * Time.fixedDeltaTime;
            yPosition += tempVelocity * Time.fixedDeltaTime;
            xPosition += velocity.x * Time.fixedDeltaTime;
            zPosition += velocity.z * Time.fixedDeltaTime;

            airTime += Time.fixedDeltaTime;

            //if (yPosition <= spawnPosition.y)
            //    break;

            Vector3 tempPosition = new Vector3(xPosition, yPosition, zPosition);
            RaycastHit hit;

            //if (Physics.SphereCast(tempPosition, 1, transform.forward, out hit, 3))
            //{
            //    if(hit.collider.tag == "Ground")
            //    {
            //        break;
            //    }
            //}
        }

        Vector3 landPosition = spawnPosition /*+ new Vector3(velocity.x * airTime, 0, velocity.z * airTime)*/;

        Instantiate(landMarker, landPosition, Quaternion.identity);
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

    public bool UseGravity()
    {
        return useGravity;
    }
}
