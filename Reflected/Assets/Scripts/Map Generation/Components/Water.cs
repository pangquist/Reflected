using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Water : MonoBehaviour
{
    [Header("Read Only")]

    [ReadOnly][SerializeField] private Rect rect;

    // Properties

    public Rect Rect => rect;

    public Water Initialize(Rect rect, float waterLevel)
    {
        this.rect = rect;
        name = "Water";

        transform.position = new Vector3(rect.x, waterLevel, rect.y);
        transform.localScale = new Vector3(rect.width, 1, rect.height);

        return this;
    }

}
