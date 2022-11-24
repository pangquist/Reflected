using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowInWorldObject : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;
    [SerializeField] private Vector3 offset;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private bool hidden;

    private static Canvas canvas;
    private static Camera mainCamera;

    public Transform ObjectToFollow { get { return objectToFollow; } set { objectToFollow = value; } }

    private void Awake()
    {
        if (canvas == null)
        {
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            mainCamera = Camera.main;
        }

        gameObject.AddComponent(typeof(CanvasGroup));
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        Show();
        Update();
    }

    private void Update()
    {
        if (objectToFollow == null)
            return;

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(objectToFollow.position + offset);

            if (transform.position != screenPoint)
                transform.position = screenPoint;
        }

        else if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Vector2 viewpointPoint = mainCamera.WorldToViewportPoint(objectToFollow.position + offset);

            if (rectTransform.anchorMin != viewpointPoint)
            {
                rectTransform.anchorMin = viewpointPoint;
                rectTransform.anchorMax = viewpointPoint;
            }
        }

        float dotProduct = Vector3.Dot(mainCamera.transform.forward, objectToFollow.position - mainCamera.transform.position);

        if (hidden && dotProduct > 0f)
            Show();
        else if (!hidden && dotProduct < 0f)
            Hide();
    }

    private void Hide()
    {
        hidden = true;
        canvasGroup.alpha = 0f;
    }

    private void Show()
    {
        hidden = false;
        canvasGroup.alpha = 1f;
    }

}
