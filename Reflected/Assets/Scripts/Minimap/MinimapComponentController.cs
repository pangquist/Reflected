using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class MinimapComponentController : MonoBehaviour
{
    [Header("Values")]

    [SerializeField] private MinimapComponent.CustomUpdate customUpdate = MinimapComponent.CustomUpdate.None;
    [SerializeField] private MinimapLayer layer = MinimapLayer.Ground;
    [SerializeField] private Sprite sprite = null;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private bool constantScale = false;
    [SerializeField] private bool automaticCustomUpdate = false;
    [SerializeField] private bool automaticPosition = true;
    [SerializeField] private bool automaticRotation = false;
    [SerializeField] private bool predeterminedSize = true;
    [SerializeField] private Vector2 size = new Vector2(25, 25);

    [Header("Read Only")]

    [ReadOnly][SerializeField] private MinimapComponent component;
    
    private static Minimap minimap;

    // Properties

    public MinimapComponent.CustomUpdate CustomUpdate => customUpdate;
    public MinimapLayer Layer => layer;
    public bool ConstantScale => constantScale;
    public bool AutomaticCustomUpdate => automaticCustomUpdate;
    public bool AutomaticPosition => automaticPosition;
    public bool AutomaticRotation => automaticRotation;
    public MinimapComponent Component => component;
    public bool HasCustomUpdate => customUpdate != MinimapComponent.CustomUpdate.None;

    private void Awake()
    {
        if (minimap == null)
            minimap = GameObject.Find("Minimap").GetComponent<Minimap>();

        component = minimap.NewComponent(this);
        component.Initialize(this);
        component.SetSprite(sprite);
        component.SetColor(color);

        if (predeterminedSize)
            component.SetSize(size);
    }

    private void OnDestroy()
    {
        component.Remove();
    }

}
