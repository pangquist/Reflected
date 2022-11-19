using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationStart : MonoBehaviour
{
    private void Start()
    {
        Animation animation = GetComponent<Animation>();
        animation[animation.clip.name].time = Random.Range(0f, animation.clip.length);
    }
}
