using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Minimap : MonoBehaviour
{
#pragma warning disable 0108

    public enum RoomReveal { Entered, Adjacent, All }

    [Header("References")]

    [SerializeField] private GameObject componentPrefab;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform worldTransform;
    [SerializeField] private RectTransform maskTransform;
    [SerializeField] private TextMeshProUGUI scaleText;
    [SerializeField] private Camera camera;

    [Header("Sprites")]

    [SerializeField] private string commonSpriteName;
    [SerializeField] private Sprite[] sprites;

    [Header("Start animation")]

    [SerializeField] private float timeBeforeStartAnimation;
    [SerializeField] private float startAnimationDuration;

    [Header("World")]

    [SerializeField] private RoomReveal roomReveal;
    [SerializeField] private bool clampWorldToMask;

    [Range(0.1f, 2f)]
    [SerializeField] private float worldScale;
    [SerializeField] private float minWorldScale;
    [SerializeField] private float maxWorldScale;
    [SerializeField] private float worldScaleSpeed;

    [Header("Frame")]

    [SerializeField] private Vector2 smallSize;
    [SerializeField] private Vector2 largeSize;
    [SerializeField] private float sizeTransitionDuration;

    [Header("Read Only")]

    [ReadOnly][SerializeField] private List<MinimapComponent> components;
    [ReadOnly][SerializeField] private MinimapComponent playerComponent;

    private float sizeTransition;

    // Properties

    public GameObject World => worldTransform.gameObject;
    public Camera Camera => camera;
    public float WorldScale => worldScale;

    private void Awake()
    {
        MapGenerator.Finished.AddListener(Initialize);
        Map.RoomEntered.AddListener(RevealRoom);
        Map.RoomCleared.AddListener(RevealChambers);
    }

    public void Initialize()
    {
        Map map = GameObject.Find("Map").GetComponent<Map>();
        worldTransform.sizeDelta = new Vector2(map.SizeX, map.SizeZ) * MapGenerator.ChunkSize;

        foreach (MinimapComponent component in components)
        {
            if (component.Controller.HasCustomUpdate)
                component.CustomUpdate_Central();
        }

        if (roomReveal != RoomReveal.All)
        {
            foreach (MinimapComponent component in components)
            {
                if (component.Controller.CustomUpdate == MinimapComponent.CustomUpdate.Room ||
                    component.Controller.CustomUpdate == MinimapComponent.CustomUpdate.Chamber ||
                    component.Controller.CustomUpdate == MinimapComponent.CustomUpdate.RoomType)
                {
                    component.Hide();
                }
            }
        }

        playerComponent = components.Find(component => component.Controller.Layer == MinimapLayer.Player);
        StartCoroutine(Coroutine_Show());
    }

    private IEnumerator Coroutine_Show()
    {
        rectTransform.localScale = Vector3.zero;
        yield return new WaitForSeconds(timeBeforeStartAnimation);

        for (float t = 0f; t < startAnimationDuration; t += Time.deltaTime)
        {
            rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, (t / startAnimationDuration).LerpValueSmoothstep());
            yield return null;
        }

        rectTransform.localScale = Vector3.one;
        yield return 0;
    }

    private void Update()
    {
        // Update world scale

        worldScale = Mathf.Clamp(worldScale + playerController.Zoom() * Time.deltaTime * worldScaleSpeed * worldScale, minWorldScale, maxWorldScale);
        worldTransform.localScale = new Vector3(worldScale, worldScale, 1f);
        scaleText.text = (int)(worldScale * 100f + 0.5f) + "%";

        // Center on player

        Vector3 focusPoint = playerComponent.ParentRectTransform.localPosition;

        if (clampWorldToMask)
        {
            float maskSizeX = rectTransform.sizeDelta.x * maskTransform.sizeDelta.x;
            float maskSizeY = rectTransform.sizeDelta.y * maskTransform.sizeDelta.y;

            focusPoint = new Vector3(
                Mathf.Clamp(focusPoint.x, maskSizeX, worldTransform.sizeDelta.x - maskSizeX),
                Mathf.Clamp(focusPoint.y, maskSizeY, worldTransform.sizeDelta.y - maskSizeY),
                0f);
        }

        worldTransform.localPosition = -focusPoint * worldScale;

        // Update minimap size

        if (playerController.ExpandInterface())
            sizeTransition += Time.deltaTime * 1f / sizeTransitionDuration;
        else
            sizeTransition -= Time.deltaTime * 1f / sizeTransitionDuration;

        sizeTransition = Mathf.Clamp01(sizeTransition);
        rectTransform.sizeDelta = Vector2.Lerp(smallSize, largeSize, sizeTransition.LerpValueCustomSmoothstep(0.6f));
    }

    public MinimapComponent NewComponent(MinimapComponentController componentController)
    {
        MinimapComponent component = Instantiate(componentPrefab, GetLayerTransform(componentController.Layer)).GetComponentInChildren<MinimapComponent>();
        components.Add(component);
        return component;
    }

    private Transform GetLayerTransform(MinimapLayer minimapLayer)
    {
        foreach (Transform layerTransform in worldTransform.transform)
            if (layerTransform.gameObject.name == minimapLayer.ToString())
                return layerTransform;

        return worldTransform.transform;
    }

    public Sprite GetSprite(string name)
    {
        foreach (Sprite icon in sprites)
            if (icon.name == commonSpriteName + " " + name)
                return icon;

        return null;
    }

    private void RevealRoom()
    {
        RevealRoom(Map.ActiveRoom, true);
    }

    private void RevealRoom(Room room, bool entered)
    {
        foreach (MinimapComponentController controller in room.GetComponents<MinimapComponentController>())
        {
            controller.Component.Show();

            if (entered && controller.CustomUpdate == MinimapComponent.CustomUpdate.Room)
                controller.Component.SetColor(Color.white);
        }
    }

    private void RevealChambers()
    {
        foreach (Chamber chamber in Map.ActiveRoom.Chambers)
        {
            MinimapComponent component = chamber.GetComponent<MinimapComponentController>().Component;
            component.Show();
            component.SetColor(Color.white);

            if (roomReveal == RoomReveal.Adjacent)
            {
                RevealRoom(chamber.Room1, false);
                RevealRoom(chamber.Room2, false);
            }
        }
    }

}
