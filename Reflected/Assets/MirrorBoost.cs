using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBoost : SwappingAbility
{
    [Header("Mirror Boost Specifics")]
    [SerializeField] int duration;
    [SerializeField] PlayerStatSystem playerStatSystem;
    Dictionary<string, float> buff = new Dictionary<string, float>();

    public override bool DoEffect()
    {
        base.DoEffect();

        Debug.Log("MIRROR BOOST!");

        if (DimensionManager.True)
        {
            buff = FindObjectOfType<UpgradeManager>().GetTrueNodes();
        }
        else
        {
            buff = FindObjectOfType<UpgradeManager>().GetMirrorNodes();
        }

        StartCoroutine(Boost());

        return true;
    }

    IEnumerator Boost()
    {
        playerStatSystem.AddStats(buff);

        yield return new WaitForSeconds(duration);

        StopBoost();
    }

    public void StopBoost()
    {
        playerStatSystem.SubtractStats(buff);
        StopCoroutine(Boost());
    }
}
