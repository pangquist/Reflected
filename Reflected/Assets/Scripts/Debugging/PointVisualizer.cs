using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PointVisualizer : MonoBehaviour
{
    [SerializeField] public GameObject pointPrefab;
    static new GameObject gameObject; // Static reference to Point Visualizer GameObject
    static PointVisualizer pointVisualizer; // Static reference to PointVisualizer component of gameObject

    void Start()
    {
        gameObject = GameObject.Find("Point Visualizer");
        pointVisualizer = gameObject.GetComponent<PointVisualizer>();
    }

    /// <summary>
    /// Instantiates a Point as a child of Point Visualizer which visualizes a position in the world for a duration of time.
    /// </summary>
    public static void AddPoint(Vector3 position, float timeToLive = 1.0f)
    {
        Tanks.Point point = Instantiate(pointVisualizer.pointPrefab, gameObject.transform).GetComponent<Tanks.Point>();
        point.timeToLive = timeToLive;
        point.transform.position = position;
        point.hideFlags = HideFlags.HideInHierarchy;
    }
}
