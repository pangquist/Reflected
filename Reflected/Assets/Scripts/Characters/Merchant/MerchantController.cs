using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantController : MonoBehaviour
{
    [SerializeField] GameObject merchant;
    Animator merchantAnimation;
    private float animationTimer = 5.0f;

    private void Awake()
    {
        DimensionManager.DimensionChanged.AddListener(SetMerchantActive);
    }
    // Start is called before the first frame update
    void Start()
    {
        merchantAnimation.GetComponent<Animator>();
        SetMerchantActive();
    }

    // Update is called once per frame
    void Update()
    {
        //if (merchant.activeSelf)
        //{
        //    animationTimer -= Time.deltaTime;
        //    if(animationTimer <= 0)
        //    {
        //        Debug.Log("Timer working");
        //        switch(Random.Range(0, 3))
        //        {
        //            case 0:
        //                merchantAnimation.Play("SenseSomethingSearching_MagicWand");
        //                Debug.Log("Animation 0 working");
        //                break;
        //            case 1:
        //                merchantAnimation.Play("Dance_MagicWand");
        //                Debug.Log("Animation 1 working");
        //                break;
        //            case 2:
        //                merchantAnimation.Play("LevelUp_MagicWand");
        //                Debug.Log("Animation 2 working");
        //                break;
        //            default:
        //                merchantAnimation.Play("Idle_Battle_MagicWand");
        //                break;

        //        }
        //        animationTimer = 20.0f;
        //    }
        //}
    }

    private void SetMerchantActive()
    {
        if (DimensionManager.CurrentDimension == Dimension.True)
        {
            merchant.SetActive(false);
        }
        else
        {
            merchant.SetActive(true);
        }
    }
}
