using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEScript : MonoBehaviour
{
    public float upTime = 3f;
    private float despawnTimer;
    private float iFrames = 0.5f;
    private float damageTimer;

    private Rigidbody rb;
    [SerializeField] private float damageAmount = 1; //Base damage 
    [SerializeField] List<AudioClip> mistClips;

    void Start()
    {
        despawnTimer = 0f;
        damageTimer = 0f;
        //this.GetComponent<MeshRenderer>().material.color = new Color(177f, 66f, 238f, 1f);
    }

    
    void Update()
    {
        despawnTimer += Time.deltaTime;
        if (despawnTimer >= upTime)
        {
            Destroy(transform.parent.gameObject);
            despawnTimer = 0f;
        }
    }

    public void SetUp(float damageAmount, Vector3 aoeSize)
    {
        this.damageAmount = damageAmount;

        transform.localScale = aoeSize;
    }

    private void OnTriggerStay(Collider other)
    {
        damageTimer += Time.deltaTime;
        if (damageTimer >= iFrames)
        {
            if (other.tag == "Player")
            {
                var healthComponent = other.GetComponentInChildren<Player>();
                if (healthComponent != null)
                {
                    damageTimer = 0f;
                    healthComponent.TakeDamage(damageAmount);
                }
            }
            damageTimer = 0f;
        }
    }

    public void PlayAoeSFX() //Called from the animation
    {
        GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        GetComponent<AudioSource>().PlayOneShot(mistClips.GetRandom());
    }
}
