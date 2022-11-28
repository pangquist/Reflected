using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;

public class DynamicHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;
    [SerializeField] private InWorldUIElement followInWorldObject;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Animator animator;

    [Header("Values")]
    [SerializeField] private Gradient gradient;

    public Slider Slider => slider;
    public Image Fill => fill;
    public TextMeshProUGUI Text => text;
    public InWorldUIElement FollowInWorldObject => followInWorldObject;
    public Gradient Gradient => gradient;

    public void PlayDestroyAnimation()
    {
        animator.SetTrigger("Destroy");
    }

    // Gets called by destory animation
    public void Destroy()
    {
        Destroy(gameObject);
    }

}
