using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCharge : MonoBehaviour, IBuyable, IMagnetic
{
    public PowerUpEffect powerUpEffect;
    //Check ui for health that the player gets back for the description and not the powerup effect

    Rigidbody rb;
    bool hasTarget, hasProperties;
    Vector3 targetPosition;
    float moveSpeed = 5f;
    float acceleration = 1.01f;
    int amount;
    string description;
    int value;

    [SerializeField] private AudioClip audioClip;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (!hasProperties)
        {
            amount = (int)powerUpEffect.amount;
            value = powerUpEffect.value;
            description = powerUpEffect.description + " " + amount.ToString();
        }
        Destroy(gameObject, 20);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            DimensionManager dimentionManager = FindObjectOfType<DimensionManager>();
            if(dimentionManager.GetCurrentCharges() < dimentionManager.GetMaxCharges())
            {
                PopUpText popUptext = PopUpTextManager.NewBasic(transform.position, "+1 Mirror Charge");
                popUptext.Text.color = new Color(0.7f, 0.5f, 0.3f);

                GameObject.Find("Player").GetComponent<AudioSource>().PlayOneShot(audioClip);
                Destroy(gameObject);
                powerUpEffect.Apply(other.gameObject, amount);
            }            
        }
    }

    public void FixedUpdate()
    {        
        if (hasTarget)
        {
            Vector3 targetDirection = (targetPosition - transform.position).normalized;
            rb.velocity = new Vector3(targetDirection.x, targetDirection.y, targetDirection.z) * moveSpeed;
            moveSpeed *= acceleration;
        }
    }

    public void SetTarget(Vector3 position)
    {
        DimensionManager dimentionManager = FindObjectOfType<DimensionManager>();
        if (dimentionManager.GetCurrentCharges() < dimentionManager.GetMaxCharges())
        {
            targetPosition = position;
            hasTarget = true;
        }
        
    }

    public void SetProperties()
    {
        amount = (int)powerUpEffect.amount;
        value = powerUpEffect.value;
        description = powerUpEffect.description + " " + amount.ToString();
        hasProperties = true;
    }

    public int GetValue()
    {
        return value;
    }

    public string GetDescription()
    {
        return description;
    }

    public void ScalePrice(int scale)
    {
        value = powerUpEffect.value * scale; 
    }

    public void ApplyOnPurchase() //Could check the current charges of the player here too
    {
        Player player = FindObjectOfType<Player>();
        powerUpEffect.Apply(player.gameObject, amount);
    }
}
