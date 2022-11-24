using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    

    [Header("References")]
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Character character;

    [Header("Values")]
    [SerializeField] private HealthTextMode healthTextMode;

    [Header("Read Only")]
    [ReadOnly][SerializeField] private HoverHealthBar healthBar;

    private static Transform inWorldLayer;

    private void Awake()
    {
        if (inWorldLayer == null)
            inWorldLayer = GameObject.Find("Canvas").transform.Find("In-World Layer");

        healthBar = Instantiate(healthBarPrefab, inWorldLayer).GetComponent<HoverHealthBar>();
        healthBar.FollowInWorldObject.ObjectToFollow = transform;

        if (healthTextMode == HealthTextMode.None)
            healthBar.Text.text = "";

        character.HealthChanged.AddListener(UpdateHealthBar);
        character.Killed.AddListener(DestroyHealthBar);
    }

    private void Start()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.Slider.value = character.GetHealthPercentage();
        healthBar.Fill.color = healthBar.Gradient.Evaluate(character.GetHealthPercentage());

        if (healthTextMode == HealthTextMode.Current)
            healthBar.Text.text = (int)(character.GetCurrentHealth() + 0.5f) + "";

        else if (healthTextMode == HealthTextMode.Full)
            healthBar.Text.text = (int)(character.GetCurrentHealth() + 0.5f) + "/" + (int)(character.GetMaxHealth() + 0.5f);
    }

    private void DestroyHealthBar()
    {
        healthBar.Remove();
    }

    [ExecuteInEditMode]
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, 0.2f);
    }

}
