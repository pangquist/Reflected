using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;

public class HoverHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;
    [SerializeField] private InWorldUIElement followInWorldObject;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip hideClip;

    [Header("Values")]
    [SerializeField] private Gradient gradient;

    public Slider Slider => slider;
    public Image Fill => fill;
    public TextMeshProUGUI Text => text;
    public InWorldUIElement FollowInWorldObject => followInWorldObject;
    public Gradient Gradient => gradient;

    private void Start()
    {
        animator.SetTrigger("Show");
    }

    public void Remove()
    {
        animator.SetTrigger("Hide");
        Destroy(gameObject, hideClip.length);
    }

}
