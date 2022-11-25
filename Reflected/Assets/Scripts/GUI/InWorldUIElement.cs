using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWorldUIElement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform objectToFollow;

    [Header("Values")]
    [SerializeField] private Vector3 screenOffset;

    [Tooltip("Scale children transforms when at a certain distance away from the camera")]
    [SerializeField] private bool shrinkChildrenAtDistance;

    [Tooltip("The distance at which this UI element is fully visible")]
    [SerializeField] private float minDistance;

    [Tooltip("The distance at which this UI element is no longer visible")]
    [SerializeField] private float maxDistance;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private bool hidden;

    private static Canvas canvas;
    private static Camera mainCamera;

    public Transform ObjectToFollow { get { return objectToFollow; } set { objectToFollow = value; } }
    public Vector3 ScreenOffset { get { return screenOffset; } set { screenOffset = value; } }

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

        // Show or hide

        float dotProduct = Vector3.Dot(mainCamera.transform.forward, objectToFollow.position - mainCamera.transform.position);

        if (hidden && dotProduct > 0f)
            Show();
        else if (!hidden && dotProduct < 0f)
            Hide();

        if (hidden)
            return;

        // Set position

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(objectToFollow.position) + screenOffset;

            if (transform.position != screenPoint)
                transform.position = screenPoint;
        }

        else if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Vector2 viewpointPoint = mainCamera.WorldToViewportPoint(objectToFollow.position) + screenOffset;

            if (rectTransform.anchorMin != viewpointPoint)
            {
                rectTransform.anchorMin = viewpointPoint;
                rectTransform.anchorMax = viewpointPoint;
            }
        }

        // Set distance visibility

        if (shrinkChildrenAtDistance)
        {
            float distance = Vector3.Distance(mainCamera.transform.position, objectToFollow.position);
            float childrenScale = 1f - distance.ValueToPercentageClamped(minDistance, maxDistance).CustomSmoothstep();

            foreach (Transform child in transform)
                child.localScale = Vector3.one * childrenScale;
        }
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
