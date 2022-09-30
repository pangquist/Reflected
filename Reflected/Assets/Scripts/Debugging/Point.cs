using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    public class Point : MonoBehaviour
    {
        [SerializeField] public float timeToLive;

        // Update is called once per frame
        void Update()
        {
            timeToLive -= Time.deltaTime;

            if (timeToLive <= 0.0f)
                Destroy(gameObject);
        }
    }
}

