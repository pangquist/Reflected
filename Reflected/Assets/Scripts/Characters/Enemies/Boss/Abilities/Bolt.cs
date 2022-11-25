using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    [SerializeField] bool useGravity;

    [SerializeField] Vector3 velocity;
    [SerializeField] float gravity;
    [SerializeField] float damage;
    [SerializeField] float explosionRange;
    [SerializeField] Player player;
    [SerializeField] GameObject landMarker;
    [SerializeField] LayerMask hitable;

    GameObject vfxObject;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponentInChildren<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        if (useGravity)
        {
            velocity.y += gravity * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(velocity);
        }

        transform.position += velocity * Time.deltaTime;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, explosionRange);
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
        }

        Vector3 landPosition = spawnPosition /*+ new Vector3(velocity.x * airTime, 0, velocity.z * airTime)*/;

        Instantiate(landMarker, landPosition, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInChildren<Player>())
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.GetComponent<Destructible>())
        {
            other.GetComponent<Destructible>().DestroyAnimation();
        }
        else if (other.gameObject.layer == 3 || other.gameObject.layer == 7)
        {
            Explode();
        }
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        velocity = newVelocity;
    }

    public bool UseGravity()
    {
        return useGravity;
    }

    public void SetVfx(GameObject newVfx)
    {
        vfxObject = newVfx;
    }

    public void SpawnEffect()
    {
        ParticleSystem particleSystem = Instantiate(vfxObject, transform.position, transform.rotation).GetComponent<ParticleSystem>();
        particleSystem.transform.parent = null;

        RaycastHit hit;
        if (Physics.Raycast(vfxObject.transform.position, Vector3.down, out hit, Mathf.Infinity, hitable))
        {
            ParticleSystemRenderer renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
            renderer.material = hit.transform.gameObject.GetComponent<Renderer>().material;
        }
    }

    public void Explode()
    {
        SpawnEffect();

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange);

        foreach(Collider collider in colliders)
        {
            if(collider.tag == "Player")
            {
                collider.GetComponentInChildren<Player>().TakeDamage(damage);
                break;
            }
        }

        Destroy(gameObject);
    }
}
