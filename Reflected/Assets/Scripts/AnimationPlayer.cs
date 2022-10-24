//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AnimationPlayer description
/// </summary>

[RequireComponent(typeof(Animator))]
public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] List<AnimationClip> clipList;
    [SerializeField] DimensionManager dimManager;
    [SerializeField] Button button;

    int index;

    Animator anim;
    bool animationLock;
    private void Start()
    {
        anim = GetComponent<Animator>();
        index = 0;
    }

    public void PlayAnimation()
    {
        if (!animationLock)
        {
            index++;

            if (index >= clipList.Count)
                index = 0;

            anim.Play(clipList[index].name);
            if (dimManager)
                dimManager.ForcedSwap();
        }
    }

    public void LockAnimation()
    {
        animationLock = true;
        if (button)
            button.interactable = false;
    }

    public void UnlockAnimation()
    {
        animationLock = false;
        if (button)
            button.interactable = true;
    }
}