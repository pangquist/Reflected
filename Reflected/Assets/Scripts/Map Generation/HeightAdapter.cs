using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightAdapter : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float offset = 0f;

    private static RaycastHit raycastHit;

    private void Awake()
    {
        ObjectPlacer.Finished.AddListener(Adapt);
    }

    private void Adapt()
    {
        if (Physics.Raycast(new Vector3(transform.position.x, 100f, transform.position.z), Vector3.down, out raycastHit, 150f, layerMask) == false)
        {
            Destroy(gameObject);
            return;
        }

        if (offset == 0f)
            transform.position = raycastHit.point;
        else
            transform.position = new Vector3(raycastHit.point.x, raycastHit.point.y + offset, raycastHit.point.z);

        Destroy(this);
    }
}
