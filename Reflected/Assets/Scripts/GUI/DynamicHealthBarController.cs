using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicHealthBarController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Character character;

    [Header("Read Only")]
    [ReadOnly][SerializeField] private DynamicHealthBar healthBar;

    private static Transform inWorldLayer;

    private void Awake()
    {
        if (inWorldLayer == null)
            inWorldLayer = GameObject.Find("Canvas").transform.Find("In-World Layer");

        healthBar = Instantiate(healthBarPrefab, inWorldLayer).GetComponent<DynamicHealthBar>();
        healthBar.FollowInWorldObject.ObjectToFollow = transform;

        character.HealthChanged.AddListener(() => healthBar.UpdateHealthBar(character));
        character.Killed.AddListener(DestroyHealthBar);
    }

    private void Start()
    {
        healthBar.UpdateHealthBar(character);
    }

    private void DestroyHealthBar()
    {
        healthBar.PlayDestroyAnimation();
    }

    [ExecuteInEditMode]
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, 0.2f);
    }

}
