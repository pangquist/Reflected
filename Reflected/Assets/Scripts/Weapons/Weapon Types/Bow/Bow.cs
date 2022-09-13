using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    [SerializeField] float firePower;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject body;

    [SerializeField] Camera cam;
    Transform targetTransform = null;
    public override void DoAttack()
    {
        anim.Play("Attack");
    }

    public override void DoSpecialAttack()
    {
        anim.Play("SpecialAttack");
    }

    public override void WeaponEffect()
    {
        Projectile arrow = Instantiate(projectile, firePoint.position, Quaternion.LookRotation(firePoint.transform.forward)).GetComponent<Projectile>();
        arrow.Fire(firePower, damage);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

        

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            targetTransform = hit.transform;
        }

        Debug.DrawRay(ray.origin, ray.direction, Color.red);
    }
}
