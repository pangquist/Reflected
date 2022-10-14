using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private Animator animator;

    [Header("Values")]

    [SerializeField] private float animationDuration;

    [Header("Read Only")]

    [ReadOnly][SerializeField] private Room room;
    [ReadOnly][SerializeField] private bool isOpen;

    private static float thickness;

    // Properties

    public static float Thickness => thickness;

    public float AnimationDuration => animationDuration;
    public bool IsOpen => isOpen;
    public Room Room => room;

    public static void StaticInitialize(float thickness)
    {
        Door.thickness = thickness;
    }

    public Door Initialize(CardinalDirection direction, Room room, Vector2 position, float width)
    {
        this.room = room;
        name = "Door " + direction.ToString();
        animator.speed = 1f / animationDuration;

        transform.position = new Vector3(position.x, 0, position.y);
        transform.localScale = new Vector3(width, Wall.Height, thickness);

        if (direction == CardinalDirection.West || direction == CardinalDirection.East)
            transform.Rotate(0f, 90f, 0f);

        return this;
    }

    public void Open()
    {
        if (isOpen)
            return;

        animator.SetTrigger("Open");
        isOpen = true;
    }

    public void Close()
    {
        if (!isOpen)
            return;

        animator.SetTrigger("Close");
        isOpen = false;
    }

}
