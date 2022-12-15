using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;

public class DynamicHealthBar : HealthBar
{
    [Header("References")]
    [SerializeField] private InWorldUIElement followInWorldObject;

    public InWorldUIElement FollowInWorldObject => followInWorldObject;

    public void PlayDestroyAnimation()
    {
        if (this != null)
            animator.SetTrigger("Destroy");
    }

    // Gets called by destory animation
    public void Destroy()
    {
        Destroy(gameObject);
    }

}
