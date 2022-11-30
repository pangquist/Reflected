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
    [Header("True Dimension")]
    [SerializeField] List<AudioSource> trueMusic = new List<AudioSource>();

    [Header("Mirror Dimension")]
    [SerializeField] List<AudioSource> mirrorMusic = new List<AudioSource>();

    [Header("Settings")]
    [SerializeField] float activeVolume;
    [SerializeField] float swapDuration;
    [SerializeField] int intensityLevel;

    [SerializeField] AudioSource currentTrack;
    Dimension currentDimension;

    private void Awake()
    {
        intensityLevel = 0;

        currentTrack = trueMusic[intensityLevel];
        SetMusic(Dimension.True, 0);
    }

    public void SwapMusicScore(Dimension dimension)
    {
        StartCoroutine(MusicSwap(dimension));
    }

    public void SetMusic(Dimension dimension, int intensityLevel)
    {
        StartCoroutine(_SetMusic(dimension, intensityLevel));
    }

    IEnumerator _SetMusic(Dimension dimension, int newIntensityLevel)
    {
        float progress = 0;
        float rate = 1 / swapDuration;

        intensityLevel = newIntensityLevel;
        currentDimension = dimension;

        while (progress < swapDuration)
        {
            currentTrack.volume = Mathf.Lerp(activeVolume, 0, progress);

            if (dimension == Dimension.True)
                trueMusic[intensityLevel].volume = Mathf.Lerp(0, activeVolume, progress);
            else
                mirrorMusic[intensityLevel].volume = Mathf.Lerp(0, activeVolume, progress);

            progress += rate * Time.deltaTime;

            if (progress >= swapDuration - 0.05f)
            {
                progress = swapDuration;

                currentTrack.volume = 0;

                if (currentDimension == Dimension.True)
                    trueMusic[intensityLevel].volume = activeVolume;
                else
                    mirrorMusic[intensityLevel].volume = activeVolume;
            }
            yield return null;
        }

        if (dimension == Dimension.True)
            currentTrack = trueMusic[intensityLevel];
        else
            currentTrack = mirrorMusic[intensityLevel];

        yield return 0;
    }

    IEnumerator MusicSwap(Dimension dimension)
    {
        float progress = 0;
        float rate = 1 / swapDuration;

        currentDimension = dimension;

        while (progress < swapDuration)
        {
            currentTrack.volume = Mathf.Lerp(activeVolume, 0, progress);

            if (dimension == Dimension.True)
                trueMusic[intensityLevel].volume = Mathf.Lerp(0, activeVolume, progress);
            else
                mirrorMusic[intensityLevel].volume = Mathf.Lerp(0, activeVolume, progress);

            progress += rate * Time.deltaTime;
            if (progress >= swapDuration - 0.05f)
            {
                progress = swapDuration;

                currentTrack.volume = 0;

                if (currentDimension == Dimension.True)
                    trueMusic[intensityLevel].volume = activeVolume;
                else
                    mirrorMusic[intensityLevel].volume = activeVolume;
            }

            yield return null;
        }

        if (dimension == Dimension.True)
            currentTrack = trueMusic[intensityLevel];
        else
            currentTrack = mirrorMusic[intensityLevel];

        yield return 0;
    }
}