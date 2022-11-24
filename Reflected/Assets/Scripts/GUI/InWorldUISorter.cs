using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InWorldUISorter : MonoBehaviour
{
    private void Update()
    {
        Dictionary<float, Transform> children = new Dictionary<float, Transform>();
        List<float> distances = new List<float>();

        foreach (Transform child in transform)
        {
            float distance = Vector3.Distance(Camera.main.transform.position, child.GetComponent<FollowInWorldObject>().ObjectToFollow.position);
            children.Add(distance, child);
            distances.Add(distance);
        }

        distances.Sort();
        distances.Reverse();

        for (int i = 0; i < distances.Count; ++i)
        {
            children[distances[i]].SetSiblingIndex(i);
        }


    }
}
