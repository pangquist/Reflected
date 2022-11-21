using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowInWorldObject : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;
    [SerializeField] private Vector3 offset;

    private RectTransform rectTransform;

    private static Canvas canvas;
    private static Camera mainCamera;

    private void Awake()
    {
        if (canvas == null)
        {
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            mainCamera = Camera.main;
        }

        rectTransform = GetComponent<RectTransform>();
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
    }

    public void SetObjectToFollow(Transform objectToFollow)
    {
        this.objectToFollow = objectToFollow;
    }
}
