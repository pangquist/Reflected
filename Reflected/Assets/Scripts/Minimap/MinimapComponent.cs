using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class MinimapComponent : MonoBehaviour
{
    public enum CustomUpdate { None, Room, Chamber, Player, RoomType }

    [Header("References")]
    [SerializeField] private RectTransform parentRectTransform;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image image;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip hideClip;

    [Header("Read Only")]
    [ReadOnly][SerializeField] private MinimapComponentController controller;

    private static Minimap minimap;
    private bool removed;

    // Properties

    public RectTransform ParentRectTransform => parentRectTransform;
    public RectTransform RectTransform => rectTransform;
    public Image Image => image;
    public MinimapComponentController Controller => controller;

    private void Awake()
    {
        if (minimap == null)
            minimap = GameObject.Find("Minimap").GetComponent<Minimap>();
    }

    public void Initialize(MinimapComponentController controller)
    {
        this.controller = controller;
        parentRectTransform.name = controller.gameObject.name.Replace("(Clone)", "");
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
    }

    public void Show()
    {
        animator.SetTrigger("Show");
    }

    public void SetPosition(Vector2 position)
    {
        parentRectTransform.localPosition = new Vector3(position.x, position.y, parentRectTransform.localPosition.z);
    }

    public void SetRotation(float rotation)
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, rotation);
    }

    public void SetScale(float scale)
    {
        rectTransform.localScale = Vector3.one * scale;
    }

    public void SetSize(Vector2 size)
    {
        parentRectTransform.sizeDelta = size;
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent, false);
    }

    public void Remove()
    {
        removed = true;
        
        if (transform.parent.gameObject != null)
        {
            Hide();
            Destroy(transform.parent.gameObject, hideClip.length);
        }   
    }

    private void Update()
    {
        if (!removed)
        {
            if (controller.AutomaticPosition)
                SetPosition(controller.transform.position.XZ());

            if (controller.AutomaticRotation)
                SetRotation(controller.transform.rotation.y);

            if (controller.AutomaticCustomUpdate)
                CustomUpdate_Central();
        }

        if (controller.ConstantScale)
            SetScale(1f / minimap.WorldScale);
    }

    public void CustomUpdate_Central()
    {
        switch (controller.CustomUpdate)
        {
            case CustomUpdate.Room:
                CustomUpdate_Room();
                break;

            case CustomUpdate.Chamber:
                CustomUpdate_Chamber();
                break;

            case CustomUpdate.Player:
                CustomUpdate_Player();
                break;

            case CustomUpdate.RoomType:
                CustomUpdate_RoomType();
                break;
        }
    }

    // Update methods for each UpdateType

    private void CustomUpdate_Room()
    {
        Room room = controller.GetComponent<Room>();
        CustomUpdate_RoomAndChamber(room.Rect);
    }

    private void CustomUpdate_Chamber()
    {
        Chamber chamber = controller.GetComponent<Chamber>();
        CustomUpdate_RoomAndChamber(chamber.Rect);
    }

    private void CustomUpdate_RoomAndChamber(Rect rect)
    {
        Vector2Int size = new Vector2Int((int)rect.width + MapGenerator.ChunkSize * 2, (int)rect.height + MapGenerator.ChunkSize * 2);
        Rect sourceRect = new Rect(0, 0, size.x, size.y);

        RenderTexture renderTexture = new RenderTexture(size.x, size.y, 24);
        Texture2D texture = new Texture2D(size.x, size.y, TextureFormat.RGBA32, false);
        texture.wrapMode = TextureWrapMode.Clamp;

        minimap.Camera.targetTexture = renderTexture;
        minimap.Camera.orthographicSize = size.y * 0.5f;
        minimap.Camera.transform.position = new Vector3(rect.center.x, minimap.Camera.transform.position.y, rect.center.y);
        minimap.Camera.Render();

        RenderTexture currentRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;
        texture.ReadPixels(sourceRect, 0, 0);
        texture.Apply();

        minimap.Camera.targetTexture = null;
        RenderTexture.active = currentRenderTexture;
        Destroy(renderTexture);

        SetSprite(Sprite.Create(texture, sourceRect, Vector2.zero));
        SetPosition(rect.center);
        SetSize(size);
    }

    private void CustomUpdate_Player()
    {
        SetRotation(-Camera.main.transform.rotation.eulerAngles.y);
    }

    private void CustomUpdate_RoomType()
    {
        Room room = controller.GetComponent<Room>();

        if (room.Type == RoomType.Start)
        {
            SetSprite(minimap.GetSprite("Compass"));
            SetColor(Color.white);
        }

        else if (room.Type == RoomType.Boss)
        {
            SetSprite(minimap.GetSprite("Boss"));
            SetColor(Color.red);
        }

        else if (room.Type == RoomType.Shop)
        {
            SetSprite(minimap.GetSprite("Exclamation"));
            SetColor(Color.yellow);
        }

        else
        {
            Destroy(controller);
            return;
        }

        SetPosition(room.Rect.center);
    }

}
