using System.Collections.Generic;
using UnityEngine;

public class InWorldUISorter : MonoBehaviour
{
    private Dictionary<float, Transform> childrenAtDistance = new Dictionary<float, Transform>();
    private List<float> distances = new List<float>();

    private void Update()
    {
        childrenAtDistance.Clear();
        distances.Clear();

        foreach (Transform child in transform)
        {
            float distance = Vector3.Distance(Camera.main.transform.position, child.GetComponent<InWorldUIElement>().ObjectToFollow.position);

            while (childrenAtDistance.ContainsKey(distance))
                distance += 0.0001f;

            childrenAtDistance[distance] = child;
            distances.Add(distance);
        }

        distances.Sort();
        distances.Reverse();

        for (int i = 0; i < distances.Count; ++i)
        {
            childrenAtDistance[distances[i]].SetSiblingIndex(i);
        }
    }

}
