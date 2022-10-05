//
// Script created by Valter Lindecrantz at the Game Development Program, MAU, 2022.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MusicManager description
/// </summary>
public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource trueMusic;
    [SerializeField] AudioSource mirrorMusic;
    [SerializeField] float activeVolume;

    [SerializeField] float swapDuration;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void SwapMusicScore(Dimension dimension)
    {
        StartCoroutine(MusicSwap(dimension));
    }

    IEnumerator MusicSwap(Dimension dimension)
    {
        float progress = 0;

        float rate = 1 / swapDuration;
        while(progress < swapDuration)
        {
            if(dimension == Dimension.True)
            {
                trueMusic.volume = Mathf.Lerp(0, activeVolume, progress * rate);
                mirrorMusic.volume = Mathf.Lerp(activeVolume, 0, progress * rate);
            }
            else
            {
                trueMusic.volume = Mathf.Lerp(activeVolume, 0, progress * rate);
                mirrorMusic.volume = Mathf.Lerp(0, activeVolume, progress * rate);
            }
            progress += rate * Time.deltaTime;
            yield return null;
        }


        yield return 0;
    }
}