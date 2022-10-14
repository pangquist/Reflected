using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    [SerializeField] private Color color;

    public Pillar Initialize()
    {
        name = "Pillar";
        transform.GetComponentInChildren<MeshRenderer>().material.color = color;
        return this;
    }
}
