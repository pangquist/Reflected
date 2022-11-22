using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider slider;
    [SerializeField] private InWorldUIElement followInWorldObject;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip hideClip;

    public Slider Slider => slider;
    public InWorldUIElement FollowInWorldObject => followInWorldObject;

    private void Start()
    {
        animator.SetTrigger("Show");
    }

    private void Update()
    {
        // Update delayed slider
    }

    public void Remove()
    {
        animator.SetTrigger("Hide");
        Destroy(gameObject, hideClip.length);
    }

}
