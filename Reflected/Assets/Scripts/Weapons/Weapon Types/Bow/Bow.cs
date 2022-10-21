using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    [Header("Bow Properties")]
    [SerializeField] float firePower;
    GameObject projectile;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject body;

    [SerializeField] Camera cam;
    [SerializeField] LayerMask hitableLayers;
    Transform targetTransform = null;

    [SerializeField] List<Projectile> projectiles = new List<Projectile>();    

    private void Start()
    {
        if (!cam) cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public override void WeaponEffect()
    {
        projectile = projectiles[powerUpIndex].gameObject;

        Projectile arrow = Instantiate(projectile, firePoint.position, body.transform.localRotation).GetComponent<Projectile>();
        arrow.Fire(firePower, damage);
    }

    protected override void Update()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, hitableLayers))
        {
            targetTransform = hit.transform;
        }

        transform.parent.rotation = cam.transform.rotation;
    }
}
