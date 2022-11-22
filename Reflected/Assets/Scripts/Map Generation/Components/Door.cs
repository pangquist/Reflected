using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private enum State { Closing, Closed, Opening, Open }

    [Header("References")]

    [SerializeField] private GameObject portion1;
    [SerializeField] private GameObject portion2;

    [Header("Values")]

    [SerializeField] private float openingDuration;
    [SerializeField] private float closingDuration;

    [Header("Read Only")]

    [ReadOnly][SerializeField] private Room room;
    [ReadOnly][SerializeField] private State state;
    [ReadOnly][SerializeField] private float transition;
    
    private static float thickness;

    private Vector3 portion1ClosedPos;
    private Vector3 portion2ClosedPos;
    private Vector3 portion1OpenPos;
    private Vector3 portion2OpenPos;

    // Properties

    public static float Thickness => thickness;

    public float OpeningDuration => openingDuration;
    public float ClosingDuration => closingDuration;
    public bool IsOpen => state == State.Opening || state == State.Open;
    public Room Room => room;

    public static void StaticInitialize(float thickness)
    {
        Door.thickness = thickness;
    }

    public Door Initialize(CardinalDirection direction, Room room, Vector2 position, float width)
    {
        this.room = room;
        name = "Door " + direction.ToString();

        transform.position = new Vector3(position.x, 0, position.y);
        transform.localScale = new Vector3(width, Wall.Height, thickness);

        if (direction == CardinalDirection.West || direction == CardinalDirection.East)
            transform.Rotate(0f, 90f, 0f);

        portion1ClosedPos = portion1.transform.localPosition;
        portion2ClosedPos = portion2.transform.localPosition;

        portion1OpenPos = portion1ClosedPos - new Vector3(0.45f, 0, 0);
        portion2OpenPos = portion2ClosedPos + new Vector3(0.45f, 0, 0);

        CloseInstantly();

        return this;
    }

    private void Update()
    {
        if (state == State.Opening)
        {
            transition += 1f / openingDuration * Time.deltaTime;

            if (transition >= 1f)
            {
                transition = 1f;
                state = State.Open;
            }

            Animate();
        }

        else if (state == State.Closing)
        {
            transition -= 1f / closingDuration * Time.deltaTime;

            if (transition <= 0f)
            {
                transition = 0f;
                state = State.Closed;
            }

            Animate();
        }
    }

    private void Animate()
    {
        portion1.transform.localPosition = Vector3.Lerp(portion1ClosedPos, portion1OpenPos, transition.Smoothstep());
        portion2.transform.localPosition = Vector3.Lerp(portion2ClosedPos, portion2OpenPos, transition.Smoothstep());
    }

    [ContextMenu("Open")]
    public void Open()
    {
        state = State.Opening;
    }

    [ContextMenu("Close")]
    public void Close()
    {
        state = State.Closing;
    }

    [ContextMenu("Open instantly")]
    public void OpenInstantly()
    {
        state = State.Open;
        transition = 1f;
        Animate();
    }

    [ContextMenu("Close instantly")]
    public void CloseInstantly()
    {
        state = State.Closed;
        transition = 0f;
        Animate();
    }

}
